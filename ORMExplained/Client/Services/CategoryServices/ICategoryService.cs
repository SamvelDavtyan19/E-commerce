using ORMExplained.Shared.DTO; // Import shared DTOs for data transfer
using ORMExplained.Shared.Models; // Import shared models

namespace ORMExplained.Client.Services.CategoryServices
{
    // Interface for category-related service operations
    public interface ICategoryService
    {
        // Method to add a new category
        Task<ServiceModel<Category>> AddCategory(Category newCategory);

        // Method to delete a category by ID
        Task<ServiceModel<Category>> DeleteCategory(int id);

        // Method to update a category
        Task<ServiceModel<Category>> UpdateCategory(Category newCategory);

        // Method to retrieve a category by ID
        Task<ServiceModel<Category>> GetCategory(int id);

        // Method to retrieve all categories
        Task<ServiceModel<Category>> GetCategories();
    }
}
