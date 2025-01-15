using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuizLab3.Model
{
    public enum Difficulty { Easy, Medium, Hard }

    public class QuestionPack
    {

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [BsonElement("difficulty")]
        public Difficulty Difficulty { get; set; }

        [BsonElement("timeLimitInSeconds")]
        public int TimeLimitInSeconds { get; set; }

        [BsonElement("questions")]
        public List<Question> Questions { get; set; }
        
        [BsonElement("category")]
        public string Category { get; set; }

        public QuestionPack(string name, string category ="hejhej", Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30)
        {
            Name = name;
            Difficulty = difficulty;
            TimeLimitInSeconds = timeLimitInSeconds;
            Questions = new List<Question>() ;
            Category = category;
        }
        public QuestionPack() //kan det här hjölpa något?
        {

        }


    }
}
