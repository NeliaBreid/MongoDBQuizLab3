using System.Security.Policy;
using System;
using MongoDB.Driver;
using QuizLab3.Model;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Printing;

namespace QuizLab3.Data
{
    public static class DataBaseInitializer
    {
        public static void SetDefaultCategory() 
        {
            var context = new QuizDbContext();

            var categoryCount = context.Categories.CountDocuments(_ => true);
            if (categoryCount == 0)
            {
                var animalCategory = new Category("Animals");

                var musicCategory = new Category("Music");

                var sportCategory = new Category("Sport");

                context?.Categories?.InsertOne(animalCategory); 
                context?.Categories?.InsertOne(musicCategory);
                context?.Categories?.InsertOne(sportCategory);
            }
        }

        public static QuestionPack SetDefaultQuestionPack()
        {
            var context = new QuizDbContext();

            var packCount = context.QuestionPacks.CountDocuments(_ => true);
            if (packCount == 0)
            {

                var defaultQuestionPack = new QuestionPack
                {
                    Name = "Default QuestionPack",
                    Difficulty = Difficulty.Medium,
                    TimeLimitInSeconds = 30,
                    Category = "Animals",
                    Questions = new List<Question>
                    {
                         new Question
                        (
                            "Which animal is known for changing colors?",
                            "Chameleon",
                            "Octopus","Lizard","Frog"
                        )
                        ,
                        new Question
                        (
                           "Which animal is the fastest?",
                            "Cheetah",
                            "Leopard","Antelope","Ostrich"
                        ),
                        new Question
                        (
                            "Which animal can fly the farthest without stopping?",
                            "Turnstone",
                            "Albatross","Eagle","Hawk"
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
                
        }
            
    }
}

