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
        public QuestionPackViewModel? ActivePack{ get => mainWindowViewModel?.ActivePack;}
        public ObservableCollection<QuestionPackViewModel> Packs { get => mainWindowViewModel.Packs; }
        public  ObservableCollection<Category> AllCategories { get; set; } //TODO: fixa en metod som lägge till kategorierna här

        private readonly MainWindowViewModel? mainWindowViewModel;

        private Question? _activeQuestion;

        private QuestionPack? _newQuestionPack;


        private Category? _selectedCategoryToEdit; //TODO: ha som string istället?

        private Category? _selectedCategory; //TODO: ha som string istället?


        public Category? SelectedCategoryToEdit
        {
            get => _selectedCategoryToEdit;
            set
            {
                _selectedCategoryToEdit = value;
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

        private readonly CategoryRepository _categoryRepository;//

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            ActiveQuestion = ActivePack?.Questions.FirstOrDefault();

            AddQuestionsCommand = new DelegateCommand(AddQuestionToActivePack, CanAddQuestionToActivePack);

            RemoveQuestionsCommand = new DelegateCommand(RemoveQuestionFromActivePack, CanRemoveQuestionFromActivePack);

            CreateQuestionPacksCommand = new DelegateCommand(CreatePack);
            DeleteQuestionPacksCommand = new DelegateCommand(DeletePack);

            _categoryRepository = new CategoryRepository();

            // Initiera ObservableCollection
            AllCategories = new ObservableCollection<Category>(_categoryRepository.GetAllCategories());
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
            return ActivePack != null;
           
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
            var newPack = new QuestionPackViewModel(new QuestionPack(NewQuestionPack.Name, NewQuestionPack.Category, NewQuestionPack.Difficulty, NewQuestionPack.TimeLimitInSeconds));
            Packs.Add(newPack);

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
    }
}


