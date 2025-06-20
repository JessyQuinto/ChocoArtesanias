# ChocoArtesanias - Artisan Chocolate E-commerce Platform

## Overview
ChocoArtesanias is a modern e-commerce platform built with .NET 9, designed specifically for artisan chocolate producers and customers. The platform features a clean architecture with separate layers for Domain, Application, Infrastructure, and API.

## 🏗️ Architecture

### Project Structure
```
src/
├── ChocoArtesanias.Api/           # Web API layer
├── ChocoArtesanias.Application/   # Application services and DTOs
├── ChocoArtesanias.Domain/        # Domain entities and business logic
└── ChocoArtesanias.Infrastructure/ # Data access and external services
```

### Key Features
- **Clean Architecture**: Separation of concerns with dependency inversion
- **Entity Framework Core**: Code-first approach with SQL Server
- **JWT Authentication**: Secure token-based authentication
- **AutoMapper**: Automated object-to-object mapping
- **FluentValidation**: Robust input validation
- **Performance Optimized**: Database indexes and query optimization
- **CORS Support**: Flexible cross-origin resource sharing

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- SQL Server (LocalDB is sufficient for development)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd ChocoArtesanias
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Update database**
   ```bash
   cd src/ChocoArtesanias.Api
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

### Configuration

#### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ChocoArtesaniasDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "TokenKey": "your-secret-key-for-jwt-tokens"
}
```

#### appsettings.Development.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ChocoArtesaniasDb_Dev;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "TokenKey": "development-secret-key"
}
```

## 📊 Database Schema

### Core Entities
- **User**: Customer and admin accounts
- **Product**: Chocolate products with details
- **Category**: Product categorization
- **Producer**: Artisan producers/brands
- **Order**: Customer orders and order items
- **CartItem**: Shopping cart functionality
- **Review**: Product reviews and ratings

### Performance Features
- Optimized indexes for common queries
- Compound indexes for category + featured products
- Indexes on order status, creation date, and user relationships

## 🔐 Security

### Authentication
- JWT Bearer token authentication
- Refresh token support for seamless user experience
- Password hashing with secure algorithms

### Authorization
- Role-based access control
- User and Admin roles
- Protected endpoints for sensitive operations

### Configuration Security
- Token keys stored in configuration (move to Azure Key Vault for production)
- CORS properly configured for development and production
- Input validation using FluentValidation

## 🔧 Development

### Adding New Features
1. **Domain First**: Add entities to the Domain layer
2. **Application Layer**: Create DTOs, services, and validators
3. **Infrastructure**: Implement repositories and data access
4. **API**: Create controllers and configure routing

### Database Migrations
```bash
# Create new migration
cd src/ChocoArtesanias.Infrastructure
dotnet ef migrations add <MigrationName> --startup-project ../ChocoArtesanias.Api

# Update database
cd ../ChocoArtesanias.Api
dotnet ef database update
```

### Testing
```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📚 API Documentation

### Authentication Endpoints
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/refresh` - Refresh access token
- `POST /api/auth/logout` - User logout

### Product Endpoints
- `GET /api/products` - List products with pagination
- `GET /api/products/{id}` - Get product details
- `GET /api/products/search` - Search products
- `GET /api/products/featured` - Get featured products

### Category Endpoints
- `GET /api/categories` - List all categories
- `GET /api/categories/{id}` - Get category with products

### Cart Endpoints
- `GET /api/cart` - Get user's cart
- `POST /api/cart/items` - Add item to cart
- `PUT /api/cart/items/{id}` - Update cart item
- `DELETE /api/cart/items/{id}` - Remove cart item

### Order Endpoints
- `GET /api/orders` - Get user's orders
- `POST /api/orders` - Create new order
- `GET /api/orders/{id}` - Get order details

## 🔄 CORS Configuration

The application supports flexible CORS configuration:

### Development
- Multiple localhost ports supported (3000, 5000, 5001, 5173)
- Credentials allowed for authentication

### Production
- Configured for specific domains
- Security-focused settings

## 🏁 Deployment

### Production Checklist
- [ ] Move secrets to Azure Key Vault or environment variables
- [ ] Configure production CORS origins
- [ ] Set up production database
- [ ] Configure logging and monitoring
- [ ] Set up CI/CD pipeline
- [ ] Enable HTTPS
- [ ] Configure rate limiting

### Environment Variables
```bash
ASPNETCORE_ENVIRONMENT=Production
TokenKey=your-production-secret-key
ConnectionStrings__DefaultConnection=your-production-connection-string
```

## 🐛 Troubleshooting

### Common Issues

1. **Database Connection Issues**
   - Ensure SQL Server/LocalDB is running
   - Check connection string format
   - Verify database exists

2. **Build Errors**
   - Clean and rebuild: `dotnet clean && dotnet build`
   - Check package compatibility
   - Ensure all projects reference correct versions

3. **Authentication Issues**
   - Verify TokenKey is configured
   - Check JWT token expiration
   - Ensure CORS is properly configured

## 📈 Performance Monitoring

### Database Performance
- Monitor query execution plans
- Check index usage statistics
- Review slow query logs

### Application Performance
- Use Application Insights (in production)
- Monitor memory usage
- Track response times

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests for new features
5. Submit a pull request

## 📝 Recent Improvements

### Critical Fixes Applied
- ✅ Fixed syntax error in Program.cs
- ✅ Resolved DLL locking issues during build
- ✅ Added missing TokenKey in development configuration

### Architecture Improvements
- ✅ Implemented dependency injection extensions
- ✅ Added AutoMapper for object mapping
- ✅ Integrated FluentValidation for input validation
- ✅ Added performance database indexes

### Configuration Enhancements
- ✅ Improved CORS configuration for development
- ✅ Enhanced JWT settings with reasonable clock skew
- ✅ Organized service registration by layer

## 📋 Next Steps

### Recommended Improvements
1. **Testing**: Add unit and integration tests
2. **Documentation**: Expand API documentation with Swagger
3. **Monitoring**: Implement comprehensive logging
4. **Caching**: Add Redis for improved performance
5. **File Upload**: Implement image upload for products

## 📞 Support

For issues and questions:
- Create an issue in the repository
- Check the troubleshooting section
- Review the architecture documentation

---

**License**: MIT License
**Version**: 1.0.0
**Last Updated**: June 20, 2025
