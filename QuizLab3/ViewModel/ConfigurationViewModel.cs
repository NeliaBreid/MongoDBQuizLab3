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

        private Category? _selectedCategory; //TODO: ha som string istället?
        private Category? _newCategoryName; //TODO: ha denna prop?
        public Category NewCategory
        {
            get => _newCategoryName;
            set
            {
                _newCategoryName = value;
                RaisePropertyChanged(nameof(NewCategory));
            }
        }

        public Category? SelectedCategoryToEdit
        {
            get => _selectedCategoryToEdit;
            set
            {
                _selectedCategoryToEdit = value;
                NewCategory = value != null ? new Category { Id = value.Id, Name = value.Name } : new Category();
                RaisePropertyChanged(nameof(SelectedCategoryToEdit));
            }
        }
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged(nameof(SelectedCategory));
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


        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            ActiveQuestion = ActivePack?.Questions.FirstOrDefault();

            AddQuestionsCommand = new DelegateCommand(AddQuestionToActivePack, CanAddQuestionToActivePack);

            RemoveQuestionsCommand = new DelegateCommand(RemoveQuestionFromActivePack, CanRemoveQuestionFromActivePack);

            CreateQuestionPacksCommand = new DelegateCommand(CreatePack);
            DeleteQuestionPacksCommand = new DelegateCommand(DeletePack);

            UpdateCategoryCommand = new DelegateCommand(UpdateCategory);
            DeleteCategoryCommand = new DelegateCommand(RemoveCategory);

            _categoryRepository = new CategoryRepository();
            _questionPackRepository = new QuestionPackRepository();
            LoadCategories();
        }

        public void LoadCategories()
        {
            if (AllCategories == null)
            {
                DataBaseInitializer.SetDefaultCategory();
            }
            AllCategories = new ObservableCollection<Category>(_categoryRepository.GetAllCategories());
            RaisePropertyChanged(nameof(AllCategories));

        }
        private void ClearNewCategory()
        {
            SelectedCategoryToEdit = null;
            NewCategory = new Category("");//
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

            ActivePack?.Questions.Add(newQuestion);

            RemoveQuestionsCommand.RaiseCanExecuteChanged();

            mainWindowViewModel?.ShowPlayerViewCommand.RaiseCanExecuteChanged();

            RaisePropertyChanged(nameof(ActivePack)); 

        }
        private bool CanAddQuestionToActivePack(object parameter)
        {
            return true; //ActivePack != null; //TODO: DEN HÄR SLUTADE FUNGERA
           
        }
        private void RemoveQuestionFromActivePack(object parameter)
        {
            ActiveQuestion = ActivePack?.Questions.LastOrDefault();

            if (ActivePack != null && ActiveQuestion != null)
            {
                ActivePack.Questions.Remove(ActiveQuestion);
                RemoveQuestionsCommand.RaiseCanExecuteChanged();
                mainWindowViewModel?.ShowPlayerViewCommand.RaiseCanExecuteChanged();
            }
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(ActivePack));
        }

        private bool CanRemoveQuestionFromActivePack(object parameter)
        {
            return ActivePack != null && ActivePack.Questions.Any();
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
                Packs.Remove(ActivePack);
                mainWindowViewModel.ActivePack = null;
                DeleteQuestionPacksCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(ActivePack));
            }

            else if (result == MessageBoxResult.No)
            {
                return;
            }
        }

        private void UpdateCategory(object parameter)
        {
            _categoryRepository.UppdateCategories(SelectedCategoryToEdit);
                LoadCategories();
                ClearNewCategory();
        }
        private void RemoveCategory(object parameter)
        {
            _categoryRepository.RemoveCategory(SelectedCategoryToEdit);
            LoadCategories();
            ClearNewCategory();
        }
    }
}


