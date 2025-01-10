using QuizLab3.Command;
using QuizLab3.Model;
using System.Collections.ObjectModel;

namespace QuizLab3.ViewModel
{/// <summary>
/// ////
/// </summary>
    class ConfigurationViewModel : ViewModelBase
    {
        public QuestionPackViewModel? ActivePack{ get => mainWindowViewModel?.ActivePack;}
        public ObservableCollection<QuestionPackViewModel> Packs { get => mainWindowViewModel.Packs; }
        
        private readonly MainWindowViewModel? mainWindowViewModel;

        private Question? _activeQuestion;

        private QuestionPack? _newQuestionPack;

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

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            ActiveQuestion = ActivePack?.Questions.FirstOrDefault();

            AddQuestionsCommand = new DelegateCommand(AddQuestionToActivePack, CanAddQuestionToActivePack);

            RemoveQuestionsCommand = new DelegateCommand(RemoveQuestionFromActivePack, CanRemoveQuestionFromActivePack);

            CreateQuestionPacksCommand = new DelegateCommand(CreatePack);
            DeleteQuestionPacksCommand = new DelegateCommand(DeletePack);
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
            var newPack = new QuestionPackViewModel(new QuestionPack(NewQuestionPack.Name, NewQuestionPack.Difficulty, NewQuestionPack.TimeLimitInSeconds));
            Packs.Add(newPack);

            mainWindowViewModel.ActivePack = newPack;
            RaisePropertyChanged(nameof(ActivePack));
        }

        private bool CanCreatePack()
        {
            return true;
        }
        private void DeletePack(object parameter)
        {
            if (ActivePack != null && Packs.Contains(ActivePack))
            {
                Packs.Remove(ActivePack);
                mainWindowViewModel.ActivePack = null;
                DeleteQuestionPacksCommand.RaiseCanExecuteChanged();
            }
            RaisePropertyChanged(nameof(ActivePack));
        }
    }
}


