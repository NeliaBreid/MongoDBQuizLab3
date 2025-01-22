using MongoDB.Bson;
using MongoDB.Driver;
using QuizLab3.Data;
using QuizLab3.Model;

namespace QuizLab3.Repositories
{
    public class QuestionRepository
    {
        public readonly IMongoCollection<QuestionPack> _packsCollection;

        public QuestionRepository() 
        { 
            var connection = new QuizDbContext();
            _packsCollection = connection.QuestionPacks;
                
        }
        public async Task<List<Question>> GetAllQuestionsInPackAsync(ObjectId packId)
        {
            var pack = await _packsCollection.Find(p => p.Id == packId).FirstOrDefaultAsync();
            var result = pack?.Questions ?? new List<Question>();
            return result;
        }
        public async Task RemoveQuestionFromDbAsync(ObjectId packId, Question questionToRemove) 
        {
            var filterPack = Builders<QuestionPack>.Filter.Eq(p => p.Id, packId);

            var update = Builders<QuestionPack>.Update.PullFilter(
                p => p.Questions, // Frågepaketet (arrayen)
                q => q.QuestionId == questionToRemove.QuestionId
            );

            await _packsCollection.UpdateOneAsync(filterPack, update);

        }
        public async Task UpdateQuestionInDbAsync(ObjectId packId, Question questionToUpdate)
        {
            var filterPack = Builders<QuestionPack>.Filter.Eq(p => p.Id, packId);

            var filterQuestion = Builders<QuestionPack>.Filter.ElemMatch(
                p => p.Questions,
                q => q.QuestionId == questionToUpdate.QuestionId 
            );

            var combinedFilter = Builders<QuestionPack>.Filter.And(filterPack, filterQuestion);

            var updateExistingQuestion = Builders<QuestionPack>.Update
                .Set("Questions.$.Query", questionToUpdate.Query)
                .Set("Questions.$.CorrectAnswer", questionToUpdate.CorrectAnswer)
                .Set("Questions.$.IncorrectAnswers", questionToUpdate.IncorrectAnswers);

            var result = await _packsCollection.UpdateOneAsync(combinedFilter, updateExistingQuestion);

            if (result.MatchedCount == 0)
            {
                var addNewQuestion = Builders<QuestionPack>.Update.Push(p => p.Questions, questionToUpdate);
                await _packsCollection.UpdateOneAsync(filterPack, addNewQuestion);
            }
        }

    }
}
