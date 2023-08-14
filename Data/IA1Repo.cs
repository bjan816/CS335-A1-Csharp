using A1.Models;

namespace A1.Data
{
    public interface IA1Repo
    {
        Task<IEnumerable<Product>> GetAllProducts();
        IQueryable<Comment> GetMostRecentComments(int count);
        Task<Comment?> GetCommentById(int commentId);
        Task<Comment> AddComment(Comment comment);
    }
}