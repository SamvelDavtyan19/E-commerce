using ORMExplained.Shared.DTO; // Import shared DTOs for data transfer
using ORMExplained.Shared.Models; // Import shared models
using System.Net.Http.Json; // Import for JSON serialization and deserialization

namespace ORMExplained.Client.Services.CategoryServices
{
    // Service class for category-related operations
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient httpClient; // HTTP client for making API requests

        // Constructor injection of HttpClient
        public CategoryService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        // Method to add a new category
        public async Task<ServiceModel<Category>> AddCategory(Category newCategory)
        {
            var response = await httpClient.PostAsJsonAsync("api/Category", newCategory); // POST request to API
            var result = await response.Content.ReadFromJsonAsync<ServiceModel<Category>>(); // Deserialize response
            return result!; // Return deserialized result
        }

        // Method to delete a category by ID
        public async Task<ServiceModel<Category>> DeleteCategory(int id)
        {
            var response = await httpClient.DeleteFromJsonAsync<ServiceModel<Category>>($"api/Category/{id}"); // DELETE request to API
            return response!; // Return deserialized response
        }

        // Method to retrieve all categories
        public async Task<ServiceModel<Category>> GetCategories()
        {
            var response = await httpClient.GetAsync("api/Category"); // GET request to API
            var result = await response.Content.ReadFromJsonAsync<ServiceModel<Category>>(); // Deserialize response
            return result!; // Return deserialized result
        }

        // Method to retrieve a category by ID
        public async Task<ServiceModel<Category>> GetCategory(int id)
        {
            var response = await httpClient.GetAsync($"api/Category/{id}"); // GET request to API with ID parameter
            var result = await response.Content.ReadFromJsonAsync<ServiceModel<Category>>(); // Deserialize response
            return result!; // Return deserialized result
        }

        // Method to update a category
        public async Task<ServiceModel<Category>> UpdateCategory(Category newCategory)
        {
            var response = await httpClient.PutAsJsonAsync("api/Category", newCategory); // PUT request to API
            var result = await response.Content.ReadFromJsonAsync<ServiceModel<Category>>(); // Deserialize response
            return result!; // Return deserialized result
        }
    }
}
