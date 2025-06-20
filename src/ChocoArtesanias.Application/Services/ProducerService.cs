using ChocoArtesanias.Application.DTOs;
using ChocoArtesanias.Application.Interfaces;

namespace ChocoArtesanias.Application.Services;

public class ProducerService
{
    private readonly IProducerRepository _producerRepository;

    public ProducerService(IProducerRepository producerRepository)
    {
        _producerRepository = producerRepository;
    }

    public async Task<IEnumerable<ProducerResponse>> GetAllProducersAsync()
    {
        var producers = await _producerRepository.GetAllAsync();
        
        return producers.Select(p => new ProducerResponse(
            p.Id,
            p.Name,
            p.Description,
            p.Location,
            p.ImageUrl,
            p.Featured,
            p.FoundationYear,
            p.Products.Count
        ));
    }

    public async Task<ProducerDetailResponse?> GetProducerByIdAsync(int id)
    {
        var producer = await _producerRepository.GetByIdWithProductsAsync(id);
        if (producer == null) return null;

        var products = producer.Products.Select(p => new ProductResponse(
            p.Id,
            p.Name,
            p.Slug,
            p.Description,
            p.Price,
            p.DiscountedPrice,
            p.ImageUrl,
            p.Images,
            p.CategoryId,
            p.Category?.Name ?? "",
            p.ProducerId,
            producer.Name,
            p.Stock,
            p.Featured,
            p.Rating,
            p.Artisan,
            p.Origin,
            p.CreatedAt
        ));

        var contact = new ContactInfo(
            producer.ContactPhone,
            producer.ContactEmail,
            producer.ContactAddress
        );

        return new ProducerDetailResponse(
            producer.Id,
            producer.Name,
            producer.Description,
            producer.Location,
            producer.ImageUrl,
            producer.Featured,
            producer.FoundationYear,
            products,
            contact
        );
    }
}
