using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using QuizLab3.Data;
using QuizLab3.Model;
using QuizLab3.ViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace QuizLab3.Repositories
{
    public class QuestionPackRepository
    {
        private readonly IMongoCollection<QuestionPack> _packsCollection;
        public QuestionPackRepository()
        {
            var connection = new QuizDbContext();
            _packsCollection = connection.QuestionPacks;
        }
        
        public List<QuestionPack> GetAllQuestionPacks()
        {
            //Hämtar alla Questionspacks från databasen
            var packs = _packsCollection.Find(_ => true).ToList();

            return packs;
        }

        // Metod för att spara frågor

    } 
}
