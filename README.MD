# ShopTemplate API

A simple e-commerce backend built with ASP.NET Core 8 and MySql, supporting products, users, and shopping cart functionality. Real-time cart updates are powered by SignalR.

## Features

- **User Authentication**: Register, login, logout.
- **Products**: Add and list products with pagination.
- **Shopping Cart**: Add/remove items, update quantity, cart total auto-calculates.
- **Real-time updates**: Cart changes broadcasted to clients via SignalR.
- **Error handling**: Consistent API responses with FluentResults.
- Unit tests are provided for `AuthService` using xUnit.


- <u> Some endpoints are not fully implemented </u> (e.g., `Logout`, `UpdateUser`, `ChangePassword`).

## Global Exception Handling

All unhandled exceptions are caught by `ExceptionHandlingMiddleware`.

- Logs errors using ASP.NET Core `ILogger`.
- Returns a standardized `ProblemDetails` JSON response with `500 Internal Server Error`.
- Ensures consistent server-side error handling across the API.


## API Endpoints

- `POST /api/auth/register` – create a new user
- `POST /api/auth/login` – login user
- `POST /api/products/add-product` – add a new product
- `GET /api/products/get-all-products` – get paged list of products
- `POST /api/cart/add-item-to-cart` – add item to cart
- `POST /api/cart/remove-item-from-cart` – remove item from cart
- `POST /api/cart/update-quantity` – update item quantity
- `GET /api/cart/{userId}` – get cart by user

## SignalR

- Hub: `/cartHub`
- Method: `ReceiveCartUpdate(cartDto)` – called when cart changes

## Notes

- Cart persists via API (server-side).
- Passwords hashed securely with PBKDF2 + SHA256.
- Error codes follow `FailureTypes` enum for consistent handling.

## Setup Database

1. Install MySQL (if not installed) and start the service.
2. Create a new database, e.g., `ShopTemplate`.
3. Update the connection string in `appsettings.json` or use existing:

```json
 "ConnectionStrings": {
"DefaultConnection": "Server=localhost;Database=ShopTemplateDB;User=sa;Password=gg232412abb22#306!843;"
}
```
## Requirements

- .NET 8 SDK
- MySQL Server


## Running project with migrations
For migrations, you should open a terminal in the project folder and run:
```bash
dotnet tool install --global dotnet-ef
dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update
```
After you can launch project:
```bash
dotnet run
