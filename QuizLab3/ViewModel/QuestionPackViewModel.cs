using QuizLab3.Command;
using QuizLab3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuizLab3.ViewModel
{
    class QuestionPackViewModel: ViewModelBase
        
    {
        private readonly QuestionPack model; 
        public QuestionPackViewModel(QuestionPack model)
        {
            this.model = model;
            this.Questions = new ObservableCollection<Question>(model.Questions);

        }
        public string Name 
        {
            get => model.Name;
            set 
            {  
                model.Name = value;
                RaisePropertyChanged();
            }
        }
        public Difficulty Difficulty
        {
            get => model.Difficulty;
            set
            {
                model.Difficulty = value;
                RaisePropertyChanged();
            }
        }
        public int TimeLimitInSeconds
        { 
            get => model.TimeLimitInSeconds;
            set 
            {  
                model.TimeLimitInSeconds = value;
                RaisePropertyChanged();
            }
        }   
        public ObservableCollection<Question> Questions { get; set; }

       
    }

}
