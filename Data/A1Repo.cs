using A1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public async Task<IEnumerable<Comment>> GetAllComments()
        {
            IEnumerable<Comment> allComments = await _dbContext.Comments.ToListAsync<Comment>();
            return allComments;
        }

        public IQueryable<Comment> GetMostRecentComments(int count)
        {
            return _dbContext.Comments
                .OrderByDescending(c => c.Time)
                .Take(count);
        }

        public async Task<Comment?> GetCommentById(int commentId)
        {
            return await _dbContext.Comments
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<Comment> AddComment(Comment comment)
        {
            EntityEntry<Comment> entityEntry = await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
            return entityEntry.Entity;
        }
    }
}