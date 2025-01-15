using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void RemoveCategory()
        {
            //Om kategorin inte finns, händer inget

            //Om kategorin finns, ta bort från databasen
        }
        public void UppdateCategories()
        {
            //Om kategori med samma namn finns så spara inte

            //Om kategori med samma namn inte finns så spara i databasen
        }

    }
}
