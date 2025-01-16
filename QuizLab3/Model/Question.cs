using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuizLab3.Model
{
    public class Question 
    {[BsonIgnore]
        public string Id { get; set; }
        [BsonElement("query")]
        public string Query { get; set; }

        [BsonElement("correctAnswer")]
        public string CorrectAnswer { get; set; }

        [BsonElement("incorrectAnswers")]
        public string[] IncorrectAnswers { get; set; }
        
        public Question() //
        {

        }
        public Question(string query, string correctAnswer, string[] incorrectAnswers) 
        {
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = incorrectAnswers;
        }
        public Question(string query, string correctAnswer, string incorrectAnswer1, string incorrectAnswer2, string incorrectAnswer3)
        {
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = new string[3] { incorrectAnswer1, incorrectAnswer2, incorrectAnswer3 };
        }


    }
}
