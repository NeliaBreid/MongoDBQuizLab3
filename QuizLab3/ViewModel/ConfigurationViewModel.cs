using MongoDB.Bson;
using QuizLab3.Command;
using QuizLab3.Data;
using QuizLab3.Model;
using QuizLab3.Repositories;
using System.Collections.ObjectModel;
using System.Windows;

namespace QuizLab3.ViewModel
{/// <summary>
/// ////
/// </summary>
    class ConfigurationViewModel : ViewModelBase
    {
        private readonly CategoryRepository _categoryRepository;

        private readonly QuestionPackRepository _questionPackRepository;
        public QuestionPackViewModel? ActivePack{ get => mainWindowViewModel?.ActivePack;}
        public ObservableCollection<QuestionPackViewModel> Packs { get => mainWindowViewModel.Packs; }
        public  ObservableCollection<Category> AllCategories { get; set; } //TODO: fixa en metod som lägge till kategorierna här

        private readonly MainWindowViewModel? mainWindowViewModel;

        private Question? _activeQuestion;

        private QuestionPack? _newQuestionPack;

        private Category? _selectedCategoryToEdit; //TODO: ha som string istället?

        private Category? _newCategoryName; //TODO: ha denna prop?

       
        public Category CurrentCategory
        {
            get => _newCategoryName;
            set
            {
                _newCategoryName = value;
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

            ActiveQuestion = ActivePack?.Questions.FirstOrDefault();

            AddQuestionsCommand = new DelegateCommand(AddQuestionToActivePack, CanAddQuestionToActivePack);

            RemoveQuestionsCommand = new DelegateCommand(RemoveQuestionFromActivePack, CanRemoveQuestionFromActivePack);

            CreateQuestionPacksCommand = new DelegateCommand(CreatePack);
            DeleteQuestionPacksCommand = new DelegateCommand(DeletePack);

            UpdateCategoryCommand = new DelegateCommand(UpdateOrAddCategory, CanUpdateOrAddCategory);
            DeleteCategoryCommand = new DelegateCommand(RemoveCategory);
            ClearCategoryNameCommand = new DelegateCommand(ClearTextBox);
            SaveQuestionCommand = new DelegateCommand(SaveChangesInQuestion); //Avvakta
            SaveQuestionPackCommand = new DelegateCommand(SaveChangesInPack);

            _categoryRepository = new CategoryRepository();
            _questionPackRepository = new QuestionPackRepository();
            LoadCategories(); //TODO: sätta den här i början av allt.
        }

        private void SaveChangesInPack(object obj)
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

            _questionPackRepository.UpdateQuestionPackInDb(packToSave); 
            mainWindowViewModel.LoadQuestionsInPack(); //Laddar om frågorna
        }

        private void SaveChangesInQuestion(object obj)
        {
            if (ActiveQuestion != null)
            {
            var questionToSave = new Question(
             ActiveQuestion.Query,
             ActiveQuestion.CorrectAnswer,
             ActiveQuestion.IncorrectAnswers
            );
                ActiveQuestion = questionToSave;

                _questionPackRepository.UpdateQuestionInDb(ActivePack.Id, questionToSave);
                mainWindowViewModel.LoadQuestionsInPack(); //Laddar om frågorna
            }
        }

        public void LoadCategories() //TODO: flytta. Laddar in Categorier, om det inte finns några Categorier så laddar den in default kategorier
        {
            if (AllCategories == null)
            {
                DataBaseInitializer.SetDefaultCategory();
            }
                AllCategories = new ObservableCollection<Category>(_categoryRepository.GetAllCategories());
            RaisePropertyChanged(nameof(AllCategories));
        }
        private void AddQuestionToActivePack(object parameter)
        {
            var newQuestion = new Question(
            query: "New Question",
            correctAnswer: string.Empty,
            incorrectAnswer1: string.Empty,
            incorrectAnswer2: string.Empty,
            incorrectAnswer3: string.Empty);

            ActiveQuestion = newQuestion;

            RemoveQuestionsCommand.RaiseCanExecuteChanged();
            mainWindowViewModel?.ShowPlayerViewCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(ActivePack));

            _questionPackRepository.UpdateQuestionInDb(ActivePack.Id, newQuestion);//Ska uppdatera frågan eller lägga till den
            mainWindowViewModel.LoadQuestionsInPack(); //Laddar om frågorna

        }
        private bool CanAddQuestionToActivePack(object parameter)
        {
            return true; //ActivePack != null; //TODO: DEN HÄR SLUTADE FUNGERA
           
        }
        private void RemoveQuestionFromActivePack(object parameter)
        {
            if (ActivePack != null && ActivePack.Questions.Any()) // Kontrollera att ActivePack och frågor finns
            {
                ActiveQuestion = ActivePack.Questions.LastOrDefault();

                if (ActiveQuestion != null)
                {
                    _questionPackRepository.RemoveQuestionFromDb(ActivePack.Id, ActiveQuestion); // Tar bort frågan från databasen
                    //ActivePack.Questions.Remove(ActiveQuestion); //Behöver jag den här fortfarande?
                    RemoveQuestionsCommand.RaiseCanExecuteChanged();
                    mainWindowViewModel?.ShowPlayerViewCommand.RaiseCanExecuteChanged();

                    RaisePropertyChanged();
                    RaisePropertyChanged(nameof(ActivePack));
                    mainWindowViewModel.LoadQuestionsInPack(); //Laddar om frågorna
                }
            }
        }

        private bool CanRemoveQuestionFromActivePack(object parameter)
        {
            return true; // ActivePack != null && ActivePack.Questions.Any(); //TODO: DEN HÄR SLUTADE FUNGERA
        }

        private void CreatePack(object? parameter)
        {
            var questionPack = new QuestionPack(NewQuestionPack.Name, NewQuestionPack.Category, NewQuestionPack.Difficulty, NewQuestionPack.TimeLimitInSeconds);

            var newPack = new QuestionPackViewModel(questionPack);
            Packs.Add(newPack);
           
            _questionPackRepository.AddQuestionPack(questionPack); //TODO: HJÄLP ELLER, category blir tokigt. Just nu läses inget in vid app start.
            mainWindowViewModel.ActivePack = newPack;
            RaisePropertyChanged(nameof(ActivePack));
        }


        private void DeletePack(object parameter)
        {
           var result = MessageBox.Show("Are you sure you want to delete this questionpack?", "Confirmation", MessageBoxButton.YesNo);


            if (ActivePack != null && Packs.Contains(ActivePack) && result == MessageBoxResult.Yes)
            {
                _questionPackRepository.DeleteQuestionPack(ActivePack.Id);
                                    
                mainWindowViewModel.ActivePack = null;
                DeleteQuestionPacksCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(ActivePack));
               mainWindowViewModel.LoadQuestionPacks(); 
            }

            else if (result == MessageBoxResult.No)
            {
                return;
            }
        }

        //Allt som har med Categorier att göra----------------------------------------------------------------
        private void ClearTextBox(object parameter)
        {
            SelectedCategoryToEdit = null;
            CurrentCategory.Name = string.Empty;
        }
        private void ClearNewCategory()
        {
            SelectedCategoryToEdit = null;
            CurrentCategory.Name = string.Empty;
        }

        private void UpdateOrAddCategory(object parameter) //TODO: Den här fungerar inte alls
        {

            if (string.IsNullOrEmpty(CurrentCategory.Name)) //Kan inte lämna den tom och trycka update
            {
                MessageBox.Show("Kategorins namn kan inte vara tomt.");
                return;
            }

            if (CurrentCategory.Id == ObjectId.Empty) // Kontrollera om det är en ny kategori jämför på Id
            {
                var existingCategory = AllCategories.FirstOrDefault(c =>
                    c.Name.Equals(CurrentCategory.Name, StringComparison.OrdinalIgnoreCase));

                if (existingCategory != null) //Kan inte lägga till två av samma
                {
                    MessageBox.Show("En kategori med samma namn finns redan.");
                    return;
                }
                
                _categoryRepository.AddCategory(CurrentCategory); // Lägg till ny kategori
                MessageBox.Show("Ny kategori har lagts till.");
            }
            else
            {
                _categoryRepository.UpdateCategory(CurrentCategory); // Uppdatera befintlig kategori
                MessageBox.Show("Kategorin har uppdaterats.");
            }

            LoadCategories();
            ClearNewCategory();
        }
        private bool CanUpdateOrAddCategory(object parameter)
        {
            // Kan bara uppdatera eller lägga till om en kategori är vald eller en ny kategori finns
            return SelectedCategoryToEdit != null || CurrentCategory != null;
        }
        private void RemoveCategory(object parameter)
        {
            _categoryRepository.RemoveCategory(SelectedCategoryToEdit);
            LoadCategories();
            ClearNewCategory();
            RaisePropertyChanged(nameof(AllCategories));
        }
    }
}


