API Endpoints
├── /api/MenuItem
│   ├── GET /                 → MenuItemController.GetAll()
│   ├── GET /{id}             → MenuItemController.GetById(id)
│   ├── POST /                → MenuItemController.Create()
│   ├── PUT /{id}             → MenuItemController.Update(id)
│   ├── DELETE /{id}          → MenuItemController.Delete(id)
│   ├── POST /{id}/review     → MenuItemController.AddReview(id, review)
│   └── GET /{id}/reviews     → MenuItemController.GetReviews(id)
│
├── /api/User
│   ├── GET /                 → UserController.GetAllUsers()
│   ├── GET /{id}             → UserController.GetUserById(id)
│   ├── POST /                → UserController.CreateUser()
│   ├── PUT /{id}             → UserController.UpdateUser(id)
│   └── DELETE /{id}          → UserController.DeleteUser(id)
│
├── /api/Order
│   ├── GET /                 → OrderController.GetAllOrders()
│   ├── GET /{id}             → OrderController.GetOrderById(id)
│   ├── POST /                → OrderController.CreateOrder()
│   ├── PUT /{id}             → OrderController.UpdateOrder(id)
│   └── DELETE /{id}          → OrderController.DeleteOrder(id)
│
├── /api/Cart
│   ├── GET /{userId}          → CartController.GetCart(userId)
│   ├── POST /{userId}/add     → CartController.AddItem(userId, CartItemRequest)
│   ├── PUT /{userId}/update   → CartController.UpdateQuantity(userId, CartItemRequest)
│   └── DELETE /{userId}/clear → CartController.ClearCart(userId)

FOLDER STRUCTURE; not complete but this is how it's structured:

RestaurantMS/
│
├── Controllers/
│   ├── CartController.cs
│   ├── MenuItemController.cs
│   ├── OrderController.cs
│   └── UserController.cs
│
├── Models/
│   ├── Cart.cs
│   ├── CustomerInfo.cs
│   ├── MenuItem.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   └── Review.cs
│
├── Services/
│   ├── CartService.cs
│   ├── MenuItemService.cs
|   |---MenuService.cs
│   ├── OrderService.cs
│   ├── UserService.cs
│   └── MongoDBService.cs
│
├── Pages/
│   └── Checkout.razor
│
├── Program.cs
│
├── RestaurantMS.csproj
│
└── appsettings.json
