﻿using QuizLab3.Command;
using QuizLab3.Dialogs;
using QuizLab3.Model;
using System.Collections.ObjectModel;
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

        }


        private void OpenNewPackDialog(object obj)
        {
            ConfigurationViewModel.NewQuestionPack = new QuestionPack(" ");

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

