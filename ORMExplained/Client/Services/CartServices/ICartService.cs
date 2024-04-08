using ORMExplained.Shared.DTO; // Import shared DTOs for data transfer

namespace ORMExplained.Client.Services.CartServices
{
    // Interface for defining cart service methods
    public interface ICartService
    {
        // Event to trigger when the cart changes
        event Action OnChange;

        // Property to hold the count of items in the cart
        int Count { get; set; }

        // Method to add a product to the cart
        Task AddToCart(MyCartModel myCartModel);

        // Method to retrieve the items in the cart
        Task<List<CartModel>> GetCart();

        // Method to remove an item from the cart
        Task RemoveCartItem(int productId);
    }
}
