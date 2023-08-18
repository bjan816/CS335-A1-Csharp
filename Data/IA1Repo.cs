using A1.Models;

namespace A1.Data
{
    public interface IA1Repo
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Comment>> GetAllComments();
        Task<Comment> GetCommentById(int commentId);
        Task<Comment> AddComment(Comment comment);
    }
}