using ListShuffle;
using QuizLab3.Command;
using QuizLab3.Dialogs;
using QuizLab3.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

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
        public DelegateCommand SetActivePackCommand { get; }
        public DelegateCommand ShowConfigurationViewCommand { get; }
        public DelegateCommand ShowPlayerViewCommand { get; }
        public DelegateCommand FullScreenCommand { get; }

        public MainWindowViewModel()
        {
            Packs = new ObservableCollection<QuestionPackViewModel>();

            PlayerViewModel = new PlayerViewModel(this);

            ConfigurationViewModel = new ConfigurationViewModel(this);

            NewPackDialog = new DelegateCommand(OpenNewPackDialog);

            PackOptionsDialog = new DelegateCommand(OpenPackOptionsDialog);

            SetActivePackCommand = new DelegateCommand(SetActivePack);

            ShowConfigurationViewCommand = new DelegateCommand(ShowConfigurationView);

            ShowPlayerViewCommand = new DelegateCommand(ShowPlayerView, CanShowPlayerView);

            FullScreenCommand = new DelegateCommand(SetFullScreen);
        }

        private void OpenNewPackDialog(object obj)
        {
            ConfigurationViewModel.NewQuestionPack = new QuestionPack(" ");

            CreateNewPackDialog createNewPackDialog = new CreateNewPackDialog();
            createNewPackDialog.ShowDialog();

        }

        private void OpenPackOptionsDialog(object? obj)
        {
            ConfigurationViewModel.NewQuestionPack = new QuestionPack(" ");
            PackOptionsDialog newPackOptionsDialog = new PackOptionsDialog();

            newPackOptionsDialog.ShowDialog();
        }
        private void SetActivePack(object? obj)
        {
            ActivePack = (QuestionPackViewModel)obj;

            RaisePropertyChanged(nameof(ActivePack));
        }
        private void ShowConfigurationView(object? obj)
        {
            IsConfigurationMode = true;
            IsPlayerMode = false;

            PlayerViewModel.GameReset();
        }
      
        private void ShowPlayerView(object? obj)
        {
            IsConfigurationMode = false;
            IsPlayerMode = true;

            PlayerViewModel.StartGame();
        }
        private bool CanShowPlayerView(object? arg)
        {
            return ActivePack.Questions.Any();
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

        public async Task LoadDataAsync() 
        {
              
            var JsonHandler = new QuizLab3.Json.Json();

            List<QuestionPack> loadedPacks = await JsonHandler.LoadJson();

            foreach (var pack in loadedPacks)
            {
                Packs.Add(new QuestionPackViewModel(pack));
            }

            if (Packs.Any())
            {
                ActivePack = Packs.First();
            }

            if (ActivePack == null)
            {
                ActivePack = new QuestionPackViewModel(new QuestionPack("My Default QuestionPack"));
                Packs.Add(ActivePack);
            };
            ConfigurationViewModel.AddQuestionsCommand.RaiseCanExecuteChanged();
            ConfigurationViewModel.RemoveQuestionsCommand.RaiseCanExecuteChanged();
            ConfigurationViewModel.CreateQuestionPacksCommand.RaiseCanExecuteChanged();
            ConfigurationViewModel.DeleteQuestionPacksCommand.RaiseCanExecuteChanged();
        }

        public async Task SaveDataAsync()
        {
            var JsonHandler = new QuizLab3.Json.Json();

            List<QuestionPack> packsToSave = Packs.Select(viewModel => new QuestionPack(

                viewModel.Name,
                viewModel.Difficulty,
                viewModel.TimeLimitInSeconds)
            {
                Questions = viewModel.Questions.ToList()
            }
            ).ToList();

            await JsonHandler.SaveJson(packsToSave);
            
        }
    }
}

