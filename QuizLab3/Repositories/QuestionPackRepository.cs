﻿using MongoDB.Bson;
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

        public async Task UpdateQuestionPackInDbAsync(QuestionPack updatedPack)
        {
            var filter = Builders<QuestionPack>.Filter.Eq(p => p.Id, updatedPack.Id);
            var existingPack = await _packsCollection.Find(p => p.Id == updatedPack.Id).FirstOrDefaultAsync();

            var update = Builders<QuestionPack>.Update
                .Set(p => p.Name, updatedPack.Name)
                .Set(p => p.Category, updatedPack.Category)
                .Set(p => p.Difficulty, updatedPack.Difficulty)
                .Set(p => p.TimeLimitInSeconds, updatedPack.TimeLimitInSeconds);

            var result = await _packsCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteQuestionPackAsync(ObjectId packIdToRemove)
        {
            try
            {

            var filter = Builders<QuestionPack>.Filter.Eq(p => p.Id, packIdToRemove);

            var result = await _packsCollection.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                throw new Exception("Inget frågepaket hittades att ta bort.");
            }
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while deleting the question pack.", ex);
            }
        }
    } 
}
