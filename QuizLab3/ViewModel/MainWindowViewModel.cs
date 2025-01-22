using QuizLab3.Command;
using QuizLab3.Data;
using QuizLab3.Dialogs;
using QuizLab3.Model;
using QuizLab3.Repositories;
using System.Collections.ObjectModel;
using System.Windows;

namespace QuizLab3.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        private readonly QuestionPackRepository _questionPackRepository;
        private readonly QuestionRepository _questionRepository;
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public List<Question> ShuffledQuestions { get; set; }
        public List<Question> ShuffledAnswers { get; set; }
        public PlayerViewModel PlayerViewModel { get; }
        public ConfigurationViewModel ConfigurationViewModel { get; }

        private QuestionPackViewModel? _activePack;

        private bool _isPlayerMode = false;
        private bool _isConfigurationMode = true;
        private bool _isQuestionSideVisible = false;

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

        public bool IsQuestionSideVisible
        {
            get => _isQuestionSideVisible;
            set
            {
                if (_isQuestionSideVisible != value)
                {
                    _isQuestionSideVisible = value;
                    RaisePropertyChanged(nameof(IsQuestionSideVisible));
                }
            }
        }
        public DelegateCommand NewPackDialog { get; }
        public DelegateCommand PackOptionsDialog { get; }
        public DelegateCommand EditCategoryDialog { get; }
        public DelegateCommand SetActivePackCommand { get; }
        public DelegateCommand ShowConfigurationViewCommand { get; }
        public DelegateCommand ShowPlayerViewCommand { get; }
        public DelegateCommand FullScreenCommand { get; }

        public MainWindowViewModel()
        {
            _questionPackRepository = new QuestionPackRepository();
            _questionRepository = new QuestionRepository();

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
     

            LoadDefaultValues();
            LoadQuestionPacks();

        }

        public async void LoadQuestionPacks()
        {
            try
            {
                Packs.Clear();
                var questionPacks = await _questionPackRepository.GetAllQuestionPacksAsync();

                if (!questionPacks.Any())
                {
                    return;
                }

                var loadedPacks = new ObservableCollection<QuestionPackViewModel>(
                    questionPacks.Select(pack => new QuestionPackViewModel(pack)));

                foreach (var pack in loadedPacks)
                {
                    Packs.Add(pack);
                }

                ActivePack = Packs.First();
                LoadQuestionsInPack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the question packs: {ex.Message}",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public async void LoadDefaultValues()
        {
            try
            {

                var questionPacks = await _questionPackRepository.GetAllQuestionPacksAsync();

                if (!questionPacks.Any())
                {
                    var defaultPack = DataBaseInitializer.SetDefaultQuestionPack();
                    ActivePack = new QuestionPackViewModel(defaultPack);
                    Packs.Add(ActivePack);
                    ConfigurationViewModel.RemoveQuestionsCommand.RaiseCanExecuteChanged();
                    ConfigurationViewModel.AddQuestionsCommand.RaiseCanExecuteChanged();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading the default questionpacks,{ex.Message}", 
                       "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void LoadQuestionsInPack()
        {
            var questions = await _questionRepository.GetAllQuestionsInPackAsync(ActivePack.Id);
            
            ActivePack.Questions.Clear();
            foreach (var question in questions)
            {
                ActivePack.Questions.Add(question);
            }
            ConfigurationViewModel.RemoveQuestionsCommand.RaiseCanExecuteChanged();
            ConfigurationViewModel.AddQuestionsCommand.RaiseCanExecuteChanged();
        }

        private void OpenNewPackDialog(object obj)
        {
            ConfigurationViewModel.NewQuestionPack = new QuestionPack();

            CreateNewPackDialog createNewPackDialog = new CreateNewPackDialog();
            createNewPackDialog.ShowDialog();
        }

        private void OpenPackOptionsDialog(object? obj)
        {
            PackOptionsDialog newPackOptionsDialog = new PackOptionsDialog();
            newPackOptionsDialog.ShowDialog();
        }

        private void OpenCategoryDialog(object? obj)
        {
            ConfigurationViewModel.CurrentCategory = new Category();

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
            PlayerViewModel.GameReset();

            IsConfigurationMode = true;
            IsPlayerMode = false;

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
            if (ActivePack!= null)
            { 
                return !_isPlayerMode && ActivePack.Questions.Any();
            }
            return false;
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

