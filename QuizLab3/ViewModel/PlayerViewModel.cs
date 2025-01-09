﻿using ListShuffle;
using QuizLab3.Command;
using QuizLab3.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Threading;

namespace QuizLab3.ViewModel
{
    class PlayerViewModel : ViewModelBase
    {
        public QuestionPackViewModel? ActivePack { get => _mainWindowViewModel?.ActivePack; }
        private List<Question> _shuffledQuestions { get; set; }
        public List<String> ShuffledAnswers { get; set; }
        public string AnswerContent1 { get; set; }
        public string AnswerContent2 { get; set; }
        public string AnswerContent3 { get; set; }
        public string AnswerContent4 { get; set; }
        public int CountCorrectAnswers { get; set; }

        public DispatcherTimer _timer;

        private int _timeRemaining;

        private int _totalQuestions;

        private int _currentQuestionIndex; 

        private Question _currentQuestion; 

        private readonly MainWindowViewModel? _mainWindowViewModel;

        public string TimeRemainingDisplay
        {
            get
            {
                return TimeSpan.FromSeconds(TimeRemaining).ToString("c");
            }
        }
        public int TotalQuestions
        {
            get
            {
                return ShuffledQuestions != null ? ShuffledQuestions.Count : 0;
            }
        }
        public int CurrentQuestionIndex
        {
            get => _currentQuestionIndex + 1;
            private set
            {
                _currentQuestionIndex = value;
                RaisePropertyChanged(nameof(TotalQuestions));
                RaisePropertyChanged(nameof(CurrentQuestionIndex));
            }
        }
        public Question CurrentQuestion
        {
            get => _currentQuestion;
            private set
            {
                _currentQuestion = value;
                RaisePropertyChanged();
            }
        }

        public List<Question> ShuffledQuestions
        {
            get => _shuffledQuestions;
            private set
            {
                _shuffledQuestions = value;
                RaisePropertyChanged(); 
                RaisePropertyChanged(nameof(TotalQuestions)); 
            }
        }

        public int TimeRemaining
        {
            get => _timeRemaining;
            set
            {
                _timeRemaining = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(TimeRemainingDisplay));
            }
        }

        public DelegateCommand AnswerButtonCommand { get; }

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
            ShuffledQuestions = new List<Question>();

            ShuffleQuestions();

            this._timer = new DispatcherTimer(); 
            _timer.Interval = TimeSpan.FromSeconds(1); 
            _timer.Tick += Timer_Tick;               
            CurrentQuestionIndex = 0; 
            AnswerButtonCommand = new DelegateCommand(SetAnswerButton);
            CountCorrectAnswers = 0;
        }


        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (TimeRemaining > 0)
            {
                TimeRemaining--;
            }
            else if(TimeRemaining == 0 && CurrentQuestionIndex == TotalQuestions && CurrentQuestionIndex != 0)
            {
                _mainWindowViewModel?.ShowResultView();
            }
            else if (CurrentQuestionIndex != TotalQuestions)
            {
                NextQuestion();
                TimeRemaining = ActivePack?.TimeLimitInSeconds ?? 0;
            }
        }

        public void ShuffleQuestions()
        {
            if (_mainWindowViewModel?.ActivePack?.Questions != null)
            {
                ShuffledQuestions = _mainWindowViewModel.ActivePack.Questions.ToList(); 
                ShuffledQuestions.Shuffle();

                RaisePropertyChanged(nameof(ShuffledQuestions));
                CurrentQuestion = ShuffledQuestions.ElementAtOrDefault(_currentQuestionIndex);

            }
        }
        public void ShuffleAnswers()
        {
            if (CurrentQuestion != null)
            {

                ShuffledAnswers = new List<string>
            {
                CurrentQuestion.CorrectAnswer,
                CurrentQuestion.IncorrectAnswers[0],
                CurrentQuestion.IncorrectAnswers[1],
                CurrentQuestion.IncorrectAnswers[2]
            };
           
            }
            ShuffledAnswers?.Shuffle();
            SetAnswers();
            RaisePropertyChanged(nameof(ShuffledAnswers));
        }
        public void NextQuestion()
        {
            if (_currentQuestionIndex < TotalQuestions - 1)
            {
                _currentQuestionIndex++;
                RaisePropertyChanged(nameof(CurrentQuestionIndex));

                CurrentQuestion = ShuffledQuestions.ElementAtOrDefault(_currentQuestionIndex);
                ShuffleAnswers();

                TimeRemaining = ActivePack?.TimeLimitInSeconds ?? 0;
            }
            else if(CurrentQuestionIndex == TotalQuestions)
            {
                _mainWindowViewModel.ShowResultView();
                GameReset();
            }
            else
            {
                _timer.Stop();
            }
        }
        private async void SetAnswerButton(object? obj)
        {
            UpdateButtonContent(CurrentQuestion.CorrectAnswer, "Correct!");

            if (obj is not string selectedAnswer)
                return;

        
            if (selectedAnswer == CurrentQuestion.CorrectAnswer)
            {
                UpdateButtonContent(selectedAnswer, "Correct!");
                CountCorrectAnswers++;
                await Task.Delay(2000);
                RaisePropertyChanged(nameof(CountCorrectAnswers));
            }
            else
            {
                UpdateButtonContent(selectedAnswer, "Incorrect!");
                await Task.Delay(2000);
            }
            NextQuestion();
        }
       

            public void StartGame()
        {
            _currentQuestionIndex = 0;

            ShuffleQuestions();
            ShuffleAnswers();

            TimeRemaining = ActivePack?.TimeLimitInSeconds ?? 0;
            _timer.Start();
        }

        public void GameReset()
        {
            CountCorrectAnswers = 0;

            _currentQuestionIndex = 0;

            TimeRemaining = ActivePack?.TimeLimitInSeconds ?? 0;

            CurrentQuestion = ShuffledQuestions.ElementAtOrDefault(_currentQuestionIndex);

            RaisePropertyChanged(nameof(CurrentQuestionIndex));
            RaisePropertyChanged(nameof(CountCorrectAnswers));
        }

        public void SetAnswers()
        {
            AnswerContent1 = ShuffledAnswers[0] ?? string.Empty;
            AnswerContent2 = ShuffledAnswers[1] ?? string.Empty;
            AnswerContent3 = ShuffledAnswers[2] ?? string.Empty;
            AnswerContent4 = ShuffledAnswers[3] ?? string.Empty;

            RaisePropertyChanged(nameof(AnswerContent1));
            RaisePropertyChanged(nameof(AnswerContent2));
            RaisePropertyChanged(nameof(AnswerContent3));
            RaisePropertyChanged(nameof(AnswerContent4));
        }
        private void UpdateButtonContent(string answer, string feedback)
        {
            if (AnswerContent1 == answer)
            {
                AnswerContent1 = feedback;
            }
            if (AnswerContent2 == answer)
            {
                AnswerContent2 = feedback;
            }
            if (AnswerContent3 == answer)
            {
                AnswerContent3 = feedback;
            }
            if (AnswerContent4 == answer)
            {
                AnswerContent4 = feedback;
            }

            RaisePropertyChanged(nameof(AnswerContent1));
            RaisePropertyChanged(nameof(AnswerContent2));
            RaisePropertyChanged(nameof(AnswerContent3));
            RaisePropertyChanged(nameof(AnswerContent4));
        }
      
    }
}
    

