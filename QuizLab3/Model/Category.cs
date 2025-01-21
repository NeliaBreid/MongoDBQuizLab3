using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuizLab3.Model
{
    public class Category
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("categoryName")]
        public string Name { get; set; }

        public Category(string name)
        {
            Name = name;

        }
        public Category()
        {

        }

    }

}
