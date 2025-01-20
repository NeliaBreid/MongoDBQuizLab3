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

        }
        public void UpdateCategory(Category category) //Uppdatera kategorin, Om den finns, matchat på Id så updaterar den existerande
        {
            if (category == null) return;

            var existingCategory = _categoriesCollection.Find(c => c.Id == category.Id).FirstOrDefault();

            if (existingCategory != null)
            {
                // Om vi har hittat en kategori men det är en annan (inte samma kategori) uppdaterar vi den
                    var filter = Builders<Category>.Filter.Eq(c => c.Id, existingCategory.Id);
                    var update = Builders<Category>.Update.Set(c => c.Name, category.Name);
                    var result =_categoriesCollection.UpdateOne(filter, update);
            }
  
        }
        public void AddCategory(Category newCategory)
        {

            if (newCategory == null) return;

            // Lägg till den nya kategorin i databasen
            _categoriesCollection.InsertOne(newCategory);
        }

    }
}
