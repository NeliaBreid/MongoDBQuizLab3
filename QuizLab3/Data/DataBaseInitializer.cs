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
                var animalCategory = new Category
                {
                    Name = "Animals"
                };

                // Insert a new category
                context.Categories.InsertOne(animalCategory);
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
                    Category = "Äventyr",
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Query = "En fråga",
                            CorrectAnswer = "Korrekt svar",
                            IncorrectAnswers = new [] { "fel1", "fel2", "fel3" }
                        },
                        new Question
                        {
                            Query = "En till fråga",
                            CorrectAnswer = "Korrekt svar andra gången",
                            IncorrectAnswers = new [] { "fel1", "fel2", "fel3" }
                        }
                    }
                };

                // Insert a new question pack
                context.QuestionPacks.InsertOne(defaultQuestionPack);
            }
        }
    }
}
