using A1.Models;

namespace A1.Data
{
    public interface IA1Repo
    {
        Task<IEnumerable<Product>> GetAllProducts();
    }
}