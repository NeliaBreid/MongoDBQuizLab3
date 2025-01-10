using QuizLab3.Model;
using System.Collections.ObjectModel;

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
