using MongoDB.Bson;
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
        
        public async Task<List<QuestionPack>> GetAllQuestionPacksAsync()
        {
            var packs = await _packsCollection.Find(_ => true).ToListAsync();
            return packs;
        }

        public async Task AddQuestionPackAsync(QuestionPack newQuestionPack)
        {
             await _packsCollection.InsertOneAsync(newQuestionPack);
        }

        public async Task UpdateQuestionPackInDbAsync(QuestionPack updatedPack) //Den här fungerar inte eftersom jag skapar som ett nytt pack kan
            // jag inte jämföra id för det nya packet har noll i id. Försök istället ta det befintliga packet och bara uppdatera det.
        {
            var filter = Builders<QuestionPack>.Filter.Eq(p => p.Id, updatedPack.Id);
            var existingPack = await _packsCollection.Find(p => p.Id == updatedPack.Id).FirstOrDefaultAsync();
            // Uppdatera specifika fält istället för att ersätta hela dokumentet
            var update = Builders<QuestionPack>.Update
                .Set(p => p.Name, updatedPack.Name)
                .Set(p => p.Category, updatedPack.Category)
                .Set(p => p.Difficulty, updatedPack.Difficulty)
                .Set(p => p.TimeLimitInSeconds, updatedPack.TimeLimitInSeconds);

            var result = await _packsCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteQuestionPackAsync(ObjectId packIdToRemove)
        {
            var filter = Builders<QuestionPack>.Filter.Eq(p => p.Id, packIdToRemove);

            var result = await _packsCollection.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                throw new Exception("Inget frågepaket hittades att ta bort."); //TODO: Lägga sånt här överallt
            }
        }
    } 
}
