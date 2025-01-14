using MongoDB.Driver;
using QuizLab3.Model;

namespace QuizLab3.Data
{
    public class QuizDbContext
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<Category> Categories { get; set; }
        public IMongoCollection<QuestionPack> QuestionPacks { get; set; }

        public QuizDbContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            _database = client.GetDatabase("CorneliaBreid");

            Categories = _database.GetCollection<Category>("Categories");
            QuestionPacks = _database.GetCollection<QuestionPack>("QuestionPacks");

        }
    }
}
