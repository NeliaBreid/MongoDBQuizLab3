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

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var projection = Builders<Category>.Projection.Include(c => c.Name);

            // Hämta alla dokument men bara Name
            var categories = await _categoriesCollection.Find(FilterDefinition<Category>.Empty)
                                                   .Project<Category>(projection)
                                                   .ToListAsync();
            return categories;
        }

        public async Task RemoveCategory(Category category)
        {
            if (category != null)
            {
                var filter = Builders<Category>.Filter.Eq(c => c.Id, category.Id);
               await _categoriesCollection.DeleteOneAsync(filter);
            }
        }
        public async Task UpdateCategory(Category category) //Uppdatera kategorin, Om den finns, matchat på Id så updaterar den existerande
        {
            var existingCategory = await _categoriesCollection.Find(c => c.Id == category.Id).FirstOrDefaultAsync();

            var filter = Builders<Category>.Filter.Eq(c => c.Id, existingCategory.Id);
            var update = Builders<Category>.Update.Set(c => c.Name, category.Name);
            await _categoriesCollection.UpdateOneAsync(filter, update);
        }
        public async Task AddCategory(Category newCategory)
        {
           await _categoriesCollection.InsertOneAsync(newCategory);
        }
    }
}
