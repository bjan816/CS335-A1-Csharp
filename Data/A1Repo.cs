using A1.Models;
using Microsoft.EntityFrameworkCore;

namespace A1.Data
{
    public class A1Repo : IA1Repo
    {
        private readonly A1DbContext _dbContext;

        public A1Repo(A1DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            IEnumerable<Product> allProducts = await _dbContext.Products.ToListAsync<Product>();
            return allProducts;
        }

        public async Task<Comment?> GetCommentById(int commentId)
        {
            return await _dbContext.Comments
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }
    }
}