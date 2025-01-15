using MongoDB.Driver;
using QuizLab3.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuizLab3.Data
{
    public static class DataBaseInitializer
    {
        public static void SetDefaultCategory()
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
                context.Categories.InsertOne(animalCategory); //TODO: insertmany istället?
                context.Categories.InsertOne(musicCategory);
                context.Categories.InsertOne(sportCategory);
            }
        }

        public static void SetDefaultQuestionPack()
        {
            var context = new QuizDbContext();

            // Check if the collection contains any question packs
            var questionPackCount = context.QuestionPacks.CountDocuments(_ => true);
            if (questionPackCount == 0)
            {
                var defaultQuestionPack = new QuestionPack
                {
                    Name = "Blabla",
                    Difficulty = Difficulty.Medium,
                    TimeLimitInSeconds = 30,
                    Category = "Sport",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Query = "En fråga",
                            CorrectAnswer = "Korrekt svar1",
                            IncorrectAnswers = new [] { "fel1", "fel2", "fel3" }
                        },
                        new Question
                        {
                            Query = "En till fråga",
                            CorrectAnswer = "Korrekt svar2",
                            IncorrectAnswers = new [] { "fell1", "fell2", "fell3" }
                        },
                        new Question
                        {
                            Query = "En tredje fråga",
                            CorrectAnswer = "Korrekt svar3",
                            IncorrectAnswers = new [] { "felll1", "felll2", "fel3" }
                        }
                    }
                };

                // Insert a new question pack
                context.QuestionPacks.InsertOne(defaultQuestionPack);
            }
        }
    }
}
