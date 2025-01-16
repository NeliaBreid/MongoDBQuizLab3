using MongoDB.Driver;
using QuizLab3.Data;
using QuizLab3.Model;

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

        public void AddQuestionPack(QuestionPack newQuestionPack)
        {
             _packsCollection.InsertOne(newQuestionPack);
        }
        public List<Question> GetAllQuestions()
        {
            var packs = GetAllQuestionPacks();  // Hämtar alla packs
            var allQuestions = packs.SelectMany(pack => pack.Questions).ToList(); // Samlar alla frågor

            return allQuestions;
        }

        // Metod för att spara frågor

    } 
}
