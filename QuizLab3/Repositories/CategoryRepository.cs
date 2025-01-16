using MongoDB.Driver;
using QuizLab3.Data;
using QuizLab3.Model;

namespace QuizLab3.Repositories
{
    internal class CategoryRepository
    {
        private readonly IMongoCollection<Category> _categoriesCollection;

        public CategoryRepository()
        {
            var connection = new QuizDbContext();
            _categoriesCollection = connection.Categories;
        }

        public List<Category> GetAllCategories()
        {
            var projection = Builders<Category>.Projection.Include(c => c.Name);

            // Hämta alla dokument men bara Name
            var categories = _categoriesCollection.Find(FilterDefinition<Category>.Empty)
                                                   .Project<Category>(projection)
                                                   .ToList();
            return categories;
        }

        public void RemoveCategory(Category category)
        {

            if (category != null)
            {
                var filter = Builders<Category>.Filter.Eq(c => c.Id, category.Id);
                _categoriesCollection.DeleteOne(filter);
            }

            //Om kategorin inte finns, händer inget

            //Om kategorin finns, ta bort från databasen
        }
        public void UppdateCategories(Category category)
        {
            //TODO: Dela upp i två metoder? Ha två olika commands att skifta emellan?

            if (category == null) return;

            
            var existingCategory = _categoriesCollection.Find(c => c.Name == category.Name).FirstOrDefault();

            if (existingCategory != null)
            {
                
                if (existingCategory.Id != category.Id)
                {
                    var filter = Builders<Category>.Filter.Eq(c => c.Id, existingCategory.Id);
                    var update = Builders<Category>.Update.Set(c => c.Name, category.Name);
                    _categoriesCollection.UpdateOne(filter, update);
                }
               
            }
            else
            {
                _categoriesCollection.InsertOne(category);
            }
        }

    }
    }

//var existingCategory = _categoriesCollection.Find(c => c.Id == category.Id).FirstOrDefault();

//if (newCategory != null)
//{
//    _categoriesCollection.InsertOne(newCategory);
//}

//else if (category != null) //det finns en sådan kategori med samma namn
//{

//    var filter = Builders<Category>.Filter.Eq(c => c.Id, existingCategory.Id);
//    var update = Builders<Category>.Update.Set(c => c.Name, category.Name);
//    _categoriesCollection.UpdateOne(filter, update);
//}

//else
//{
//    return;
//}