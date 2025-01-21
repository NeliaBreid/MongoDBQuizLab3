using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuizLab3.Model
{
    public class Question 
    {

        [BsonElement("questionId")]
        public string QuestionId { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("query")]
        public string Query { get; set; }

        [BsonElement("correctAnswer")]
        public string CorrectAnswer { get; set; }

        [BsonElement("incorrectAnswers")]
        public string[] IncorrectAnswers { get; set; }
        
        public Question() //Måste ha när man skapar ny fråga
        {

        }
        //public Question(string query, string correctAnswer, string[] incorrectAnswers) 
        //{
        //    QuestionId = ObjectId.GenerateNewId().ToString();
        //    Query = query;
        //    CorrectAnswer = correctAnswer;
        //    IncorrectAnswers = incorrectAnswers;
        //}
        public Question(string query, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2, string incorrectAnswer3)
        {
            QuestionId = ObjectId.GenerateNewId().ToString();
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = new string[3] { incorrectAnswer1, incorrectAnswer2, incorrectAnswer3 };
        }

        public Question(string questionId, string query, string correctAnswer, string[] incorrectAnswers)
        {
            QuestionId = questionId;
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = incorrectAnswers;
        }
    }
}
