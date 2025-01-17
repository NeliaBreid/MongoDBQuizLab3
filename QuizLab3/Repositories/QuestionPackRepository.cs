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

        public void UpdateQuestionInDb(ObjectId packId, Question newQuestion) //Man kan inte lägga till frågor med samma Query
        {
            var filterPack = Builders<QuestionPack>.Filter.Eq(p => p.Id, packId);

            var filterQuestion = Builders<QuestionPack>.Filter.ElemMatch(
                p => p.Questions,
                q => q.Query == newQuestion.Query || q.CorrectAnswer == newQuestion.CorrectAnswer // Här matchar vi baserat på Query
            );

            var combinedFilter = Builders<QuestionPack>.Filter.And(filterPack, filterQuestion);

            var updateExistingQuestion = Builders<QuestionPack>.Update
                .Set("Questions.$.Query", newQuestion.Query)
                .Set("Questions.$.CorrectAnswer", newQuestion.CorrectAnswer)
                .Set("Questions.$.IncorrectAnswers", newQuestion.IncorrectAnswers);

            var result = _packsCollection.UpdateMany(combinedFilter, updateExistingQuestion);

            if (result.MatchedCount == 0)
            {
                var addNewQuestion = Builders<QuestionPack>.Update.Push(p => p.Questions, newQuestion);
                _packsCollection.UpdateOne(filterPack, addNewQuestion);
            }
        }

        public void RemoveQuestionFromDb(ObjectId packId, Question questionToRemove)
        {

        }
        public void UpdateQuestionPack(QuestionPack updatedPack)
        {
            var filter = Builders<QuestionPack>.Filter.Eq(p => p.Id, updatedPack.Id);
            _packsCollection.ReplaceOne(filter, updatedPack);
        }


        // Metod för att spara frågor

    } 
}
