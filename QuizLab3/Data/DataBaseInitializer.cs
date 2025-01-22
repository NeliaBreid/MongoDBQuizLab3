using MongoDB.Driver;
using QuizLab3.Model;

namespace QuizLab3.Data
{
    public static class DataBaseInitializer
    {
        public static void SetDefaultCategory() //TODO:Gör säkerhetsnät ifall det redan finns
        {
            
            var context = new QuizDbContext();

            // Check if the collection contains any categories
            var categoryCount = context.Categories.CountDocuments(_ => true);
            if (categoryCount == 0)
            {
                var animalCategory = new Category("Animals");

                var musicCategory = new Category("Music");

                var sportCategory = new Category("Sport");

                // Insert a new category
                context?.Categories.InsertOne(animalCategory); //TODO: insertmany istället?
                context?.Categories.InsertOne(musicCategory);
                context?.Categories.InsertOne(sportCategory);
            }
        }

        public static QuestionPack SetDefaultQuestionPack()//TODO: Gör säkerhetsnät ifall det redan finns
        {
            var context = new QuizDbContext();

            // Check if the collection contains any question packs
            var packCount = context.QuestionPacks.CountDocuments(_ => true);
            if (packCount == 0)
            {

                var defaultQuestionPack = new QuestionPack
                {
                    Name = "Default QuestionPack",
                    Difficulty = Difficulty.Medium,
                    TimeLimitInSeconds = 30,
                    Category = "Sport",
                    Questions = new List<Question>
                    {
                         new Question
                        (
                             "Vilket djur är störst?",
                            "Blåval",
                             "Elefant", "Giraff", "Noshörning" 
                        )
                        ,
                        new Question
                        (
                            "Vilket instrument har sex strängar?",
                           "Gitarr",
                             "Fiol", "Bas", "Piano" 
                        ),
                        new Question
                        (
                            "Hur många spelare finns i ett fotbollslag?",
                            "11",
                             "9", "10", "12" 
                        )
                    }                
                
                };
                context.QuestionPacks.InsertOne(defaultQuestionPack);
                return defaultQuestionPack;
            }

            else
            {
                return context.QuestionPacks.Find(_ => true).FirstOrDefault();
            }
                // Insert a new question pack
                
            }
            
            
        }
   }

