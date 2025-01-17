using QuizLab3.Command;
using QuizLab3.Data;
using QuizLab3.Dialogs;
using QuizLab3.Model;
using QuizLab3.Repositories;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Windows;
using System.Windows.Input;

namespace QuizLab3.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public List<Question> ShuffledQuestions { get; set; }
        public List<Question> ShuffledAnswers { get; set; }
        public PlayerViewModel PlayerViewModel { get; } 
        public ConfigurationViewModel ConfigurationViewModel { get; }

        private QuestionPackViewModel? _activePack;

        private bool _isPlayerMode = false;


        private bool _isConfigurationMode = true; 

        public QuestionPackViewModel? ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged(nameof(ActivePack));
                ConfigurationViewModel?.RaisePropertyChanged();

            }
        }
        public bool IsConfigurationMode
        {
            get => _isConfigurationMode;
            set
            {
                _isConfigurationMode = value;
                RaisePropertyChanged(nameof(IsConfigurationMode));
                RaisePropertyChanged(nameof(IsPlayerMode));

            }
        }
        public bool IsPlayerMode
        {
            get => _isPlayerMode;
            set
            {
                _isPlayerMode = value;
                RaisePropertyChanged(nameof(IsConfigurationMode));
                RaisePropertyChanged(nameof(IsPlayerMode));

            }
        }
 
        public DelegateCommand NewPackDialog { get; }
        public DelegateCommand PackOptionsDialog { get; }
        public DelegateCommand EditCategoryDialog { get; }
        public DelegateCommand SetActivePackCommand { get; }
        public DelegateCommand ShowConfigurationViewCommand { get; }
        public DelegateCommand ShowPlayerViewCommand { get; }
        public DelegateCommand FullScreenCommand { get; }

     
        private readonly QuestionPackRepository _questionPackRepository;
        public MainWindowViewModel()
        {
            Packs = new ObservableCollection<QuestionPackViewModel>();

            PlayerViewModel = new PlayerViewModel(this);

            ConfigurationViewModel = new ConfigurationViewModel(this);

            NewPackDialog = new DelegateCommand(OpenNewPackDialog);

            PackOptionsDialog = new DelegateCommand(OpenPackOptionsDialog);

            EditCategoryDialog = new DelegateCommand(OpenCategoryDialog);

            SetActivePackCommand = new DelegateCommand(SetActivePack);

            ShowConfigurationViewCommand = new DelegateCommand(ShowConfigurationView, CanConfigurationView);

            ShowPlayerViewCommand = new DelegateCommand(ShowPlayerView, CanShowPlayerView);

            FullScreenCommand = new DelegateCommand(SetFullScreen);

            _questionPackRepository = new QuestionPackRepository();

        }
        public void LoadQuestionPacks()
        {
            var questionPacks = _questionPackRepository.GetAllQuestionPacks();

            if (questionPacks == null || !questionPacks.Any()) //Kollar om det finns något i databasen, om tom så går den in och gör default
            {
                var defaultPack = DataBaseInitializer.SetDefaultQuestionPack(); 
                ActivePack = new QuestionPackViewModel(defaultPack);;
                Packs.Add(ActivePack);
            }
            else
            {
                var loadedPacks= new ObservableCollection<QuestionPackViewModel>(
                    questionPacks.Select(pack => new QuestionPackViewModel(pack))); //TODO: tanke, så länge det sparas ner som questionpacks så kanske det går bra att hämta dom som Questionpackviewmodel?

                _questionPackRepository.GetAllQuestions();//TODO: kolla om den här fungerar

                foreach (var pack in loadedPacks)
                {
                    Packs.Add(pack);
                }

                ActivePack = Packs.First();

            }

        }

        private void OpenNewPackDialog(object obj)
        {
            ConfigurationViewModel.NewQuestionPack = new QuestionPack();

            CreateNewPackDialog createNewPackDialog = new CreateNewPackDialog();
            createNewPackDialog.ShowDialog();

        }

        private void OpenPackOptionsDialog(object? obj)
        {
            //ConfigurationViewModel.NewQuestionPack = new QuestionPack(" ");
            PackOptionsDialog newPackOptionsDialog = new PackOptionsDialog();

            newPackOptionsDialog.ShowDialog();
        }

        private void OpenCategoryDialog(object? obj)
        {
            ConfigurationViewModel.NewCategory = new Category();
            EditCategoryDialog editCategoryDialog = new EditCategoryDialog();
            editCategoryDialog.ShowDialog();
        }
        private void SetActivePack(object? obj)
        {
            ActivePack = (QuestionPackViewModel)obj;

            RaisePropertyChanged(nameof(ActivePack));
        }
        private bool CanConfigurationView(object? arg)
        {
            return _isPlayerMode;
        }
        private void ShowConfigurationView(object? obj)
        {
            IsConfigurationMode = true;
            IsPlayerMode = false;

            PlayerViewModel.GameReset();
            ShowConfigurationViewCommand.RaiseCanExecuteChanged();
            ShowPlayerViewCommand.RaiseCanExecuteChanged();
        }
      
        private void ShowPlayerView(object? obj)
        {
            IsConfigurationMode = false;
            IsPlayerMode = true;

            PlayerViewModel.StartGame();
            ShowConfigurationViewCommand.RaiseCanExecuteChanged();
            ShowPlayerViewCommand.RaiseCanExecuteChanged();
        }
        private bool CanShowPlayerView(object? arg)
        {
            return !_isPlayerMode && ActivePack.Questions.Any();
        }

        private void SetFullScreen(object? obj)
        {
            var window = App.Current.MainWindow;

            if (window.WindowState == WindowState.Normal)
            {
                window.WindowStyle = WindowStyle.None;
                window.ResizeMode = ResizeMode.NoResize;
                window.WindowState = WindowState.Maximized;
            }
            else
            {
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.ResizeMode = ResizeMode.CanResize;
                window.WindowState = WindowState.Normal;
            }
        }

        public void ShowResultView()
        {
            PlayerViewModel._timer.Stop();
            ResultDialog createResultDialog = new ResultDialog();
            createResultDialog.ShowDialog();
        }



        
    }
}

