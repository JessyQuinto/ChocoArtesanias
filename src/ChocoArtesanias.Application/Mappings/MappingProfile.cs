using AutoMapper;
using ChocoArtesanias.Application.DTOs;
using ChocoArtesanias.Domain.Entities;

namespace ChocoArtesanias.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserResponse>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id.ToString()));
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());

        // Product mappings
        CreateMap<Product, ProductResponse>()
            .ForCtorParam("Image", opt => opt.MapFrom(src => src.ImageUrl))
            .ForCtorParam("CategoryName", opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
            .ForCtorParam("ProducerName", opt => opt.MapFrom(src => src.Producer != null ? src.Producer.Name : string.Empty));
        
        CreateMap<Product, ProductDetailResponse>()
            .ForCtorParam("Image", opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.Producer, opt => opt.MapFrom(src => src.Producer));

        // Category mappings
        CreateMap<Category, CategoryResponse>()
            .ForCtorParam("Image", opt => opt.MapFrom(src => src.ImageUrl))
            .ForCtorParam("ProductCount", opt => opt.MapFrom(src => src.Products.Count));
        CreateMap<Category, CategoryDetailResponse>()
            .ForCtorParam("Image", opt => opt.MapFrom(src => src.ImageUrl))
            .ForCtorParam("ProductCount", opt => opt.MapFrom(src => src.Products.Count))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

        // Producer mappings
        CreateMap<Producer, ProducerResponse>()
            .ForCtorParam("Image", opt => opt.MapFrom(src => src.ImageUrl))
            .ForCtorParam("ProductCount", opt => opt.MapFrom(src => src.Products.Count));
        CreateMap<Producer, ProducerDetailResponse>()
            .ForCtorParam("Image", opt => opt.MapFrom(src => src.ImageUrl))
            .ForCtorParam("Slug", opt => opt.MapFrom(src => src.Name.ToLowerInvariant().Replace(" ", "-")))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products))
            .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => new ContactInfo(
                src.ContactPhone,
                src.ContactEmail,
                src.ContactAddress)));

        // Cart mappings
        CreateMap<CartItem, CartItemResponse>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
        CreateMap<Product, ProductCartInfo>()
            .ForCtorParam("Image", opt => opt.MapFrom(src => src.ImageUrl));

        // Order mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.StatusHistory, opt => opt.Ignore())
            .ForMember(dest => dest.Tracking, opt => opt.Ignore());
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));
        CreateMap<Product, OrderProductDto>()
            .ForCtorParam("Image", opt => opt.MapFrom(src => src.ImageUrl));
        CreateMap<Order, OrderSummaryDto>()
            .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.Items.Sum(i => i.Quantity)));        // Address mappings
        CreateMap<ShippingAddressDto, Address>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.StreetAddress, opt => opt.MapFrom(src => src.StreetAddress))
            .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));
        CreateMap<Address, OrderAddressDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.StreetAddress));
    }
}
