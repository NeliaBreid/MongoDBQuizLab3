using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuizLab3.Model
{
   public class Category
    {
        [BsonId]
        public ObjectId Id { get; set; }//???

        [BsonElement("categoryName")]
        public string Name { get; set; }

        public Category(string name)
        {
            Name = name;
            //Lägg en metod som skapar upp tre stycken kategorier som defaultkategorier
        }
        public Category()
        {

        }
    }
}
