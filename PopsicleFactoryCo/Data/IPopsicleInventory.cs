using PopsicleFactoryCo.Models;

//Implement a repository pattern with Dependency Injection (with Data Access Layer for PopsicleModel)
// Implementing the Repository Pattern with Dependency Injection (DI) in .NET Core involves creating an abstraction layer for data access
// and then registering these abstractions with the built-in DI container.
//This interface defines the contract for managing popsicle inventory data.
//It includes methods for searching, retrieving, creating, updating, and deleting popsicle records.

namespace PopsicleFactoryCo.Data
{
    public interface IPopsicleInventory
    {
        public Task<IEnumerable<PopsicleModel>> SearchForPopsiclesAsync(string searchItem);
        public Task<IEnumerable<PopsicleModel>> GetListOfAllPopsicles();
        public Task<PopsicleModel> GetPopsicleByIdAsync(int id);
        public Task<PopsicleModel> CreatePopsicleAsync(PopsicleModel popsicle);
        public Task<PopsicleModel> UpdatePopsicleAsync(PopsicleModel popsicle);
        public Task<bool> DeletePopsicleAsync(int id);
    }
}