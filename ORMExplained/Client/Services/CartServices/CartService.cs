using Blazored.LocalStorage; // Import Blazored LocalStorage library for managing local storage in Blazor
using Microsoft.JSInterop; // Import Microsoft.JSInterop library for interacting with JavaScript
using ORMExplained.Shared.DTO; // Import DTOs (Data Transfer Objects) shared between client and server

namespace ORMExplained.Client.Services.CartServices
{
    // CartService class implementing ICartService interface
    public class CartService : ICartService
    {
        // Dependency injection of services needed by CartService
        private readonly ILocalStorageService localStorageService; // LocalStorageService for managing browser local storage
        private readonly IJSRuntime jSRuntime; // JavaScript Runtime for invoking JavaScript functions
        private readonly IProductServices productService; // ProductServices for interacting with product-related functionalities

        // Constructor to initialize CartService with required services
        public CartService(ILocalStorageService localStorageService, IJSRuntime jSRuntime, IProductServices productService)
        {
            this.localStorageService = localStorageService; // Initialize LocalStorageService
            this.jSRuntime = jSRuntime; // Initialize JavaScript Runtime
            this.productService = productService; // Initialize ProductServices
        }

        // Property to hold the count of items in the cart
        public int Count { get; set; }

        // Event to trigger when there's a change in the cart
        public event Action OnChange;

        // Method to add a product to the cart
        public async Task AddToCart(MyCartModel myCartModel)
        {
            var MyCart = await localStorageService.GetItemAsync<List<MyCartModel>>("MyCart"); // Retrieve cart items from local storage
            if (MyCart == null)
            {
                MyCart = new List<MyCartModel>(); // If cart is empty, initialize a new list
            }

            if (myCartModel != null)
            {
                var item = MyCart.FirstOrDefault(p => p.ProductId == myCartModel.ProductId); // Check if the product already exists in the cart
                if (item != null)
                {
                    MyCart.Remove(item); // If the product exists, remove it from the cart
                    MyCart.Add(myCartModel); // Add the updated product to the cart
                    await localStorageService.SetItemAsync("MyCart", MyCart); // Save the updated cart in local storage
                    await jSRuntime.InvokeVoidAsync("alert", "Product updated done!"); // Show a notification indicating that the product was updated
                }
                else
                {
                    MyCart.Add(myCartModel); // If the product doesn't exist in the cart, add it
                    await localStorageService.SetItemAsync("MyCart", MyCart); // Save the updated cart in local storage
                    await jSRuntime.InvokeVoidAsync("alert", "Product added to cart!"); // Show a notification indicating that the product was added
                }
            }
            else
            {
                await jSRuntime.InvokeVoidAsync("alert", "Product cart cannot be empty"); // Show a notification if the cart is empty
            }

            Count = (await localStorageService.GetItemAsync<List<MyCartModel>>("MyCart")).Count(); // Update the count of items in the cart
            OnChange?.Invoke(); // Trigger the OnChange event
        }

        // Method to get the items in the cart
        public async Task<List<CartModel>> GetCart()
        {
            var CartModel = new List<CartModel>(); // Initialize a list to hold cart items
            var MyCart = await localStorageService.GetItemAsync<List<MyCartModel>>("MyCart"); // Retrieve cart items from local storage
            if (MyCart != null)
            {
                foreach (var item in MyCart)
                {
                    var product = await productService.GetProduct(item.ProductId); // Get product details for each item in the cart
                    if (product != null)
                    {
                        var model = new CartModel // Create a CartModel object for the item
                        {
                            ProductId = item.ProductId,
                            Quantity = item.UserQuantity,
                            Name = product.Single!.Name,
                            Image = product.Single.Image,
                            Price = product.Single.NewPrice != 0 && product.Single.NewPrice < product.Single.OriginalPrice ?
                            product.Single.NewPrice : product.Single.OriginalPrice,
                            SubTotal = (product.Single.NewPrice != 0 && product.Single.NewPrice < product.Single.OriginalPrice ?
                            product.Single.NewPrice : product.Single.OriginalPrice) * item.UserQuantity
                        };
                        CartModel.Add(model); // Add the CartModel object to the list
                    }
                }
            }
            return CartModel; // Return the list of cart items
        }

        // Method to remove an item from the cart
        public async Task RemoveCartItem(int productId)
        {
            var MyCart = await localStorageService.GetItemAsync<List<MyCartModel>>("MyCart"); // Retrieve cart items from local storage
            if (MyCart != null)
            {
                var product = MyCart.FirstOrDefault(p => p.ProductId == productId); // Find the item to be removed from the cart
                if (product != null)
                {
                    MyCart.Remove(product); // Remove the item from the cart
                    await localStorageService.SetItemAsync("MyCart", MyCart); // Save the updated cart in local storage
                }
                Count = (await localStorageService.GetItemAsync<List<MyCartModel>>("MyCart")).Count(); // Update the count of items in the cart
                OnChange?.Invoke(); // Trigger the OnChange event
            }
        }
    }
}
