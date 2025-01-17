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
        
        public List<QuestionPack> GetAllQuestionPacks()
        {
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

        public void UpdateQuestionInDb(ObjectId packId, Question newQuestion)
        {
            var filterPack = Builders<QuestionPack>.Filter.Eq(p => p.Id, packId);

            var filterQuestion = Builders<QuestionPack>.Filter.ElemMatch(
                p => p.Questions,
                q => q.QuestionId == newQuestion.QuestionId // Här matchar vi baserat på Query och korrektanswer
            );

            var combinedFilter = Builders<QuestionPack>.Filter.And(filterPack, filterQuestion);

            var updateExistingQuestion = Builders<QuestionPack>.Update
                .Set("Questions.$.Query", newQuestion.Query)
                .Set("Questions.$.CorrectAnswer", newQuestion.CorrectAnswer)
                .Set("Questions.$.IncorrectAnswers", newQuestion.IncorrectAnswers);

            var result = _packsCollection.UpdateOne(combinedFilter, updateExistingQuestion);

            if (result.MatchedCount == 0)
            {
                var addNewQuestion = Builders<QuestionPack>.Update.Push(p => p.Questions, newQuestion);
                _packsCollection.UpdateOne(filterPack, addNewQuestion);
            }
        }

        public void RemoveQuestionFromDb(ObjectId packId, Question questionToRemove) //TODO: den här fungerar inte för tillfället
        {
            var filterPack = Builders<QuestionPack>.Filter.Eq(p => p.Id, packId);

            // Filtrera för att hitta den specifika frågan i arrayen som ska tas bort
            var update = Builders<QuestionPack>.Update.PullFilter(
                p => p.Questions, // Frågepaketet (arrayen)
                q => q.QuestionId == questionToRemove.QuestionId // Här matchar vi på QuestionId
            );

            // Uppdatera QuestionPack och ta bort den matchande frågan
            var result = _packsCollection.UpdateOne(filterPack, update);

        }
        public void UpdateQuestionPackInDb(QuestionPack updatedPack)
        {
            var filter = Builders<QuestionPack>.Filter.Eq(p => p.Id, updatedPack.Id);
            var existingPack = _packsCollection.Find(p => p.Id == updatedPack.Id).FirstOrDefault();
            // Uppdatera specifika fält istället för att ersätta hela dokumentet
            var update = Builders<QuestionPack>.Update
                .Set(p => p.Name, updatedPack.Name)
                .Set(p => p.Category, updatedPack.Category)
                .Set(p => p.Difficulty, updatedPack.Difficulty)
                .Set(p => p.TimeLimitInSeconds, updatedPack.TimeLimitInSeconds)
                .Set(p => p.Questions, updatedPack.Questions);

            var result = _packsCollection.UpdateOne(filter, update);
            if (result.MatchedCount > 0)
            {
            }
            }




    } 
}
