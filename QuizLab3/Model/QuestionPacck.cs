using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuizLab3.Model
{
    public enum Difficulty { Easy, Medium, Hard }

    public class QuestionPack
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)]  
        public string Id { get; set; }

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

        public QuestionPack(string name, string category, Difficulty difficulty, int timeLimitInSeconds)
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
