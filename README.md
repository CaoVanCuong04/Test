# DATN Shoes - E-commerce Shoe Store

A full-stack e-commerce web application for selling shoes, built with ASP.NET Core, Blazor WebAssembly, and Entity Framework Core.

## 🏗️ Architecture

This project follows a clean architecture pattern with the following layers:

- **WebUI** - Customer-facing Blazor WebAssembly application
- **AdminWeb** - Admin dashboard Blazor WebAssembly application (using Argon Dashboard template)
- **API** - RESTful API built with ASP.NET Core
- **BUS** - Business logic layer
- **DAL** - Data Access Layer with Entity Framework Core
- **Helper** - Utility classes and helpers (caching, VNPay integration, etc.)

## 📋 Features

### Customer Features
- Browse products with filters (category, brand, size, color, gender)
- Product search and detailed product views
- Shopping cart functionality
- User authentication (Email/Password, Google OAuth)
- Order management and tracking
- Multiple payment methods (VNPay, Stripe, PayPal)
- Shipping integration with GHN (Giao Hàng Nhanh)
- Wishlist/Favorites
- Recently viewed products
- Apply vouchers/discount codes
- Product reviews and ratings

### Admin Features
- Dashboard with statistics and charts
- Product management (CRUD operations)
- Category and brand management
- Color and size management
- Order management and status updates
- User management
- Voucher management
- Order statistics and reports
- Revenue tracking

## 🛠️ Technologies Used

### Backend
- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger/OpenAPI

### Frontend
- Blazor WebAssembly
- Argon Dashboard (Admin UI)
- Tailwind CSS
- Chart.js
- Font Awesome Icons

### Third-Party Integrations
- **VNPay** - Vietnamese payment gateway
- **Stripe** - International payment processing
- **PayPal** - Payment processing
- **Google OAuth** - Social authentication
- **GHN (Giao Hàng Nhanh)** - Shipping service

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) or SQL Server Express
- Visual Studio 2022 or VS Code
- Node.js (optional, for npm packages)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd DATN_Shoes
   ```

2. **Configure Database Connection**
   
   Copy the example configuration file:
   ```bash
   cd API
   cp appsettings.example.json appsettings.json
   ```

   Update `appsettings.json` with your settings:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;TrustServerCertificate=True;"
     },
     "Jwt": {
       "Secret": "YOUR_JWT_SECRET_KEY_HERE_MINIMUM_32_CHARACTERS"
     }
   }
   ```

3. **Apply Database Migrations**
   ```bash
   cd DAL
   dotnet ef database update --startup-project ../API
   ```

4. **Configure API URLs**
   
   Update the API base URLs in:
   - `AdminWeb/Program.cs` (line 21, 26, 31, etc.)
   - `WebUI/Services/*` files
   
   Default API URL: `https://localhost:7134/`

5. **Build the Solution**
   ```bash
   dotnet build
   ```

### Running the Application

You need to run all three projects simultaneously:

#### Option 1: Using Visual Studio
1. Set multiple startup projects:
   - Right-click on solution → Properties
   - Select "Multiple startup projects"
   - Set `API`, `AdminWeb`, and `WebUI` to "Start"
2. Press F5 to run

#### Option 2: Using Command Line

**Terminal 1 - API:**
```bash
cd API
dotnet run
```

**Terminal 2 - AdminWeb:**
```bash
cd AdminWeb
dotnet run
```

**Terminal 3 - WebUI:**
```bash
cd WebUI
dotnet run
```

### Default URLs
- **API**: https://localhost:7134
- **AdminWeb**: https://localhost:7221
- **WebUI**: https://localhost:7041

### Default Admin Credentials
Check `DAL/AppDbContext.cs` for seeded admin credentials.

## 📁 Project Structure

```
DATN_Shoes/
├── API/                          # Web API Project
│   ├── Controllers/              # API Controllers
│   │   ├── AuthController.cs
│   │   ├── ProductAdminController.cs
│   │   ├── OrdersController.cs
│   │   ├── PaymentController.cs
│   │   └── ...
│   ├── Services/                 # API-specific services
│   ├── Extensions/               # Extension methods
│   └── Program.cs               # API configuration
│
├── AdminWeb/                    # Admin Blazor WebAssembly
│   ├── Components/              # Reusable Blazor components
│   │   ├── Sidebar.razor
│   │   ├── Navbar.razor
│   │   └── ...
│   ├── Pages/                   # Admin pages
│   │   ├── Home.razor          # Dashboard
│   │   ├── Products.razor
│   │   ├── Orders.razor
│   │   └── ...
│   ├── Services/                # Admin services
│   │   ├── AuthService.cs
│   │   ├── ProductService.cs
│   │   └── ...
│   ├── Models/                  # DTOs
│   └── wwwroot/                 # Static assets
│
├── WebUI/                       # Customer Blazor WebAssembly
│   ├── Components/              # UI components
│   ├── Pages/                   # Customer pages
│   │   ├── Index.razor         # Home page
│   │   ├── Shop.razor
│   │   ├── ProductDetail.razor
│   │   ├── Cart.razor
│   │   └── ...
│   ├── Services/                # Client services
│   └── wwwroot/                 # Static assets
│
├── BUS/                         # Business Logic Layer
│   └── Services/                # Business services
│       └── Interfaces/          # Service interfaces
│
├── DAL/                         # Data Access Layer
│   ├── Entities/                # Entity models
│   │   ├── User.cs
│   │   ├── Product.cs
│   │   ├── Order.cs
│   │   └── ...
│   ├── DTOs/                    # Data Transfer Objects
│   ├── Repositories/            # Repository pattern
│   ├── Migrations/              # EF Core migrations
│   ├── AppDbContext.cs         # Database context
│   └── UnitOfWork/              # Unit of Work pattern
│
└── Helper/                      # Utilities
    ├── CacheCore/               # Caching utilities
    ├── VNPay/                   # VNPay integration
    ├── Utils/                   # Helper utilities
    └── ModelHelps/              # Model helpers
```

## 🔧 Configuration

### JWT Authentication
Configure in `appsettings.json`:
```json
"Jwt": {
  "Issuer": "http://localhost",
  "Secret": "YOUR_JWT_SECRET_KEY_HERE_MINIMUM_32_CHARACTERS",
  "ExpirationInDays": 8
}
```

### Payment Gateways

#### VNPay (Vietnamese Payment)
```json
"VNPay": {
  "TmnCode": "YOUR_VNPAY_TMN_CODE",
  "HashSecret": "YOUR_VNPAY_HASH_SECRET",
  "PaymentUrl": "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
  "ReturnUrl": "https://localhost:7134/api/Payment/vnpay-return"
}
```

#### Stripe
```json
"Stripe": {
  "SecretKey": "YOUR_STRIPE_SECRET_KEY",
  "PublishableKey": "YOUR_STRIPE_PUBLISHABLE_KEY"
}
```

#### PayPal
```json
"PayPal": {
  "ClientId": "YOUR_PAYPAL_CLIENT_ID",
  "ClientSecret": "YOUR_PAYPAL_CLIENT_SECRET"
}
```

### Shipping Integration (GHN)
```json
"GHN": {
  "BaseUrl": "https://dev-online-gateway.ghn.vn/shiip/public-api",
  "Token": "YOUR_GHN_TOKEN",
  "ShopId": "YOUR_SHOP_ID"
}
```

### Google OAuth
```json
"Google": {
  "ClientId": "YOUR_GOOGLE_CLIENT_ID",
  "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
}
```

## 📊 Database Schema

The database includes the following main entities:
- **Users** - User accounts and authentication
- **Roles** - User roles (Admin, Employee, User)
- **Products** - Product information
- **ProductVariants** - Size and color variants
- **Categories** - Product categories
- **Brands** - Shoe brands
- **Orders** - Customer orders
- **OrderDetails** - Order line items
- **Payments** - Payment transactions
- **Shipments** - Shipping information
- **Vouchers** - Discount codes
- **Carts** - Shopping carts
- **Addresses** - User addresses
- **Reviews** - Product reviews
- **FavoriteProducts** - User wishlists

## 🔒 Security Features

- JWT-based authentication
- Password hashing using ASP.NET Core Identity
- Role-based authorization
- Secure payment processing
- CORS configuration
- HTTPS enforcement
- Input validation and sanitization

## 📱 Responsive Design

Both WebUI and AdminWeb are fully responsive and work on:
- Desktop browsers
- Tablets
- Mobile devices

## 🧪 Testing

To run tests:
```bash
dotnet test
```

## 📝 API Documentation

When running the API project, access Swagger UI at:
```
https://localhost:7134/swagger
```

This provides interactive API documentation with the ability to test endpoints.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👥 Authors

DATN Team - Final Year Project

## 🙏 Acknowledgments

- Argon Dashboard template for the admin UI
- VNPay, Stripe, PayPal for payment processing
- GHN for shipping services
- All open-source libraries used in this project

## 📞 Support

For support and questions, please contact the development team.

---

**Note**: This is a student project (DATN - Đồ Án Tốt Nghiệp) for educational purposes.
