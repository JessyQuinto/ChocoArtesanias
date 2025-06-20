using ChocoArtesanias.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChocoArtesanias.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProducersController : ControllerBase
{
    private readonly ProducerService _producerService;

    public ProducersController(ProducerService producerService)
    {
        _producerService = producerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducers()
    {
        var producers = await _producerService.GetAllProducersAsync();
        return Ok(new { Success = true, Data = producers });
    }    [HttpGet("{id}")]
    public async Task<IActionResult> GetProducerById(Guid id)
    {
        var producer = await _producerService.GetProducerByIdAsync(id);
        
        if (producer == null)
            return NotFound(new { Message = "Productor no encontrado" });

        return Ok(new { Success = true, Data = producer });
    }
}
