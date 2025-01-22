using System.Collections.ObjectModel;
using System.Windows;
using MongoDB.Bson;
using QuizLab3.Command;
using QuizLab3.Data;
using QuizLab3.Model;
using QuizLab3.Repositories;

namespace QuizLab3.ViewModel
{/// <summary>
/// ////
/// </summary>
    class ConfigurationViewModel : ViewModelBase
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly QuestionPackRepository _questionPackRepository;
        private readonly QuestionRepository _questionRepository;

        public QuestionPackViewModel? ActivePack{ get => mainWindowViewModel?.ActivePack;}
        public ObservableCollection<QuestionPackViewModel> Packs { get => mainWindowViewModel.Packs; }
        public  ObservableCollection<Category> AllCategories { get; set; } 

        private readonly MainWindowViewModel? mainWindowViewModel;

        private Question? _activeQuestion;

        private QuestionPack? _newQuestionPack;

        private Category? _selectedCategoryToEdit;

        private Category? _currentCategory;

        public Category CurrentCategory
        {
            get => _currentCategory;
            set
            {
                _currentCategory = value;
                RaisePropertyChanged(nameof(CurrentCategory));
            }
        }

        public Category? SelectedCategoryToEdit
        {
            get => _selectedCategoryToEdit;
            set
            {
                _selectedCategoryToEdit = value;
                CurrentCategory = value != null ? new Category { Id = value.Id, Name = value.Name } : new Category();
                RaisePropertyChanged(nameof(SelectedCategoryToEdit));
            }
        }
        public QuestionPack? NewQuestionPack
        {
            get => _newQuestionPack;
            set
            {
                _newQuestionPack = value;
                RaisePropertyChanged(nameof(NewQuestionPack));
            }
        }
        public Question? ActiveQuestion
        {
            get => _activeQuestion;
             set 
            {
                _activeQuestion = value;
                mainWindowViewModel.IsQuestionSideVisible = _activeQuestion != null;
                RaisePropertyChanged();
            }
        }
        public DelegateCommand AddQuestionsCommand { get; }
        public DelegateCommand RemoveQuestionsCommand { get; }
        public DelegateCommand CreateQuestionPacksCommand { get; }
        public DelegateCommand DeleteQuestionPacksCommand { get; }
        public DelegateCommand UpdateCategoryCommand { get; }
        public DelegateCommand DeleteCategoryCommand { get; }
        public DelegateCommand ClearCategoryNameCommand { get; }
        public DelegateCommand SaveQuestionCommand { get; }
        public DelegateCommand SaveQuestionPackCommand { get; }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            _categoryRepository = new CategoryRepository();
            _questionPackRepository = new QuestionPackRepository();
            _questionRepository = new QuestionRepository();

            ActiveQuestion = ActivePack?.Questions.FirstOrDefault();


            CreateQuestionPacksCommand = new DelegateCommand(CreatePack);
            DeleteQuestionPacksCommand = new DelegateCommand(DeletePack);
            SaveQuestionCommand = new DelegateCommand(SaveChangesInQuestion); 

            UpdateCategoryCommand = new DelegateCommand(UpdateOrAddCategory);
            DeleteCategoryCommand = new DelegateCommand(RemoveCategory);

            ClearCategoryNameCommand = new DelegateCommand(ClearTextBox);

            AddQuestionsCommand = new DelegateCommand(AddQuestionToActivePack, CanAddQuestionToActivePack);
            RemoveQuestionsCommand = new DelegateCommand(RemoveQuestionFromActivePack, CanRemoveQuestionFromActivePack);
            SaveQuestionPackCommand = new DelegateCommand(SaveChangesInPack);

            LoadCategories();
        }

        private async void SaveChangesInPack(object obj)
        {
            if (ActivePack == null)
            {
                MessageBox.Show("You got to choose a pack to edit");
            }
            else
            {
                var packToSave = new QuestionPack
                {
                    Id = ActivePack.Id, // Behåll det befintliga Id
                    Name = ActivePack.Name,
                    Category = ActivePack.Category,
                    Difficulty = ActivePack.Difficulty,
                    TimeLimitInSeconds = ActivePack.TimeLimitInSeconds,
                    Questions = ActivePack.Questions.ToList()
                };

                await _questionPackRepository.UpdateQuestionPackInDbAsync(packToSave);
                mainWindowViewModel?.LoadQuestionsInPack(); 
                
            }
        
        }

        private async void SaveChangesInQuestion(object obj)
        {
            if (ActiveQuestion != null)
            {
            var questionToSave = new Question(
             ActiveQuestion.QuestionId, //
             ActiveQuestion.Query,
             ActiveQuestion.CorrectAnswer,
             ActiveQuestion.IncorrectAnswers
            );
                ActiveQuestion = questionToSave;
                await _questionRepository.UpdateQuestionInDbAsync(ActivePack.Id, questionToSave);
                mainWindowViewModel?.LoadQuestionsInPack(); 
            }
        }

        public async void LoadCategories() //TODO: flytta.
        {
            try
            {

            if (AllCategories == null)
            {
                DataBaseInitializer.SetDefaultCategory();
            }

            var categories = await _categoryRepository.GetAllCategoriesAsync();
            AllCategories=new ObservableCollection<Category>(categories);

            RaisePropertyChanged(nameof(AllCategories));
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"An error occurred while loading the categories: {ex.Message}",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void AddQuestionToActivePack(object? parameter)
        {
            if (ActivePack!= null)
            {
                var newQuestion = new Question(
                    query: "New Question",
                    correctAnswer: string.Empty,
                    incorrectAnswer1: string.Empty,
                    incorrectAnswer2: string.Empty,
                    incorrectAnswer3: string.Empty);

                ActiveQuestion = newQuestion;

                mainWindowViewModel?.ShowPlayerViewCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(ActivePack));
                await _questionRepository.UpdateQuestionInDbAsync(ActivePack.Id, newQuestion);//Ska uppdatera frågan eller lägga till den
                mainWindowViewModel?.LoadQuestionsInPack(); //Laddar om frågorna

           
            }
        }
        private bool CanAddQuestionToActivePack(object? parameter)
        {
            return ActivePack != null; //TODO: DEN HÄR SLUTADE FUNGERA
           
        }
        private async void RemoveQuestionFromActivePack(object? parameter)
        {
            if (ActivePack != null && ActivePack.Questions.Any()) // Kontrollera att ActivePack och frågor finns
            {
                ActiveQuestion = ActivePack.Questions.LastOrDefault();

                if (ActiveQuestion != null)
                {
                    await _questionRepository.RemoveQuestionFromDbAsync(ActivePack.Id, ActiveQuestion); // Tar bort frågan från databasen
                    mainWindowViewModel?.ShowPlayerViewCommand.RaiseCanExecuteChanged();
                    ActivePack.Questions.Clear();
                    RaisePropertyChanged(nameof(ActivePack));
                    mainWindowViewModel?.LoadQuestionsInPack(); //Laddar om frågorna
                    
                }
            }
        }

        private bool CanRemoveQuestionFromActivePack(object? parameter) 
        {
            return ActivePack != null && ActivePack.Questions.Any(); //TODO: DEN HÄR SLUTADE FUNGERA 
        }

        private async void CreatePack(object? parameter)
        {
            var questionPack = new QuestionPack(NewQuestionPack.Name, NewQuestionPack.Category, NewQuestionPack.Difficulty, NewQuestionPack.TimeLimitInSeconds);

            var newPack = new QuestionPackViewModel(questionPack);
            Packs.Add(newPack);
           
            await _questionPackRepository.AddQuestionPackAsync(questionPack);
            mainWindowViewModel.ActivePack = newPack;
            AddQuestionsCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(ActivePack));
        }


        private async void DeletePack(object? parameter)
        {
           var result = MessageBox.Show("Are you sure you want to delete this questionpack?", "Confirmation", MessageBoxButton.YesNo);

            if (ActivePack != null && Packs.Contains(ActivePack) && result == MessageBoxResult.Yes)
            {
               
                await _questionPackRepository.DeleteQuestionPackAsync(ActivePack.Id);
                                    
                mainWindowViewModel.ActivePack = null; //TODO: Vad händer om man tar bort den här?
                AddQuestionsCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(ActivePack));
                mainWindowViewModel.Packs.Clear();
                mainWindowViewModel.LoadQuestionPacks();
                    
            }

            else if (result == MessageBoxResult.No)
            {
                return;
            }
        }

        // This Part is about Categories ----------------------------------------------------------------------------------------------------------------
        private void ClearTextBox(object? parameter)
        {
            SelectedCategoryToEdit = null;
            CurrentCategory.Name = string.Empty;
        }
        private void ClearNewCategory() 
        {
            SelectedCategoryToEdit = null;
            CurrentCategory.Name = string.Empty;
        }

        private async void UpdateOrAddCategory(object? parameter) 
        {
            if (string.IsNullOrEmpty(CurrentCategory.Name)) //Kan inte lämna den tom och trycka update
            {
                MessageBox.Show("You have to give the category a name");
                return;
            }

            if (CurrentCategory.Id == ObjectId.Empty) // Kontrollera om det är en ny kategori jämför på Id
            {
                var existingCategory = AllCategories.FirstOrDefault(c =>
                    c.Name.Equals(CurrentCategory.Name, StringComparison.OrdinalIgnoreCase));

                if (existingCategory != null) //Kan inte lägga till två av samma
                {
                    MessageBox.Show("A category with this name already exists.");
                    return;
                }
 
                await _categoryRepository.AddCategory(CurrentCategory); // Lägg till ny kategori
                
            }
            else
            {
                await _categoryRepository.UpdateCategory(CurrentCategory); // Uppdatera befintlig kategori
            }

            LoadCategories();
            ClearNewCategory();
        }
        private async void RemoveCategory(object? parameter)
        {
            await _categoryRepository.RemoveCategory(SelectedCategoryToEdit);
            LoadCategories();
            ClearNewCategory();
            RaisePropertyChanged(nameof(AllCategories));
        }
    }
}


