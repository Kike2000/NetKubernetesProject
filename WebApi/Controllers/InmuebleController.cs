using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetKubernetes.Data.Inmuebles;
using NetKubernetes.Data.Usuarios;
using NetKubernetes.Dtos.InmuebleDTO;
using NetKubernetes.Dtos.UsuarioDTO;
using NetKubernetes.Middleware;
using NetKubernetes.Models;

namespace NetKubernetes.Controllers;
[Route("api/[controller]")]
[ApiController]
public class InmuebleController : ControllerBase
{
    private readonly IInmuebleRepository _repository;
    private IMapper _mapper;
    public InmuebleController(IInmuebleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InmuebleResponseDTO>>> ObtenerInmuebles()
    {
        var inmuebles = await _repository.GetAllInmuebles();
        return Ok(_mapper.Map<IEnumerable<InmuebleResponseDTO>>(inmuebles));
    }

    [HttpGet("{id}", Name = "GetInmuebleById")]
    public async Task<ActionResult<InmuebleResponseDTO>> GetInmuebleById(int id)
    {
        var inmueble = await _repository.GetInmuebleById(id);
        if (inmueble is null)
        {
            throw new MiddlewareException(HttpStatusCode.NotFound, new { mensaje = "Inmueble no fue encontrado." });
        }
        return Ok(_mapper.Map<InmuebleResponseDTO>(inmueble));
    }

    [HttpPost]
    public async Task<ActionResult<InmuebleResponseDTO>> CreateInmueble([FromBody] InmuebleRequestDTO request)
    {
        var inmuebleModel = _mapper.Map<Inmueble>(request);
        await _repository.CreateInmueble(inmuebleModel);
        await _repository.SaveChanges();

        var inmuebleResponse = _mapper.Map<InmuebleResponseDTO>(inmuebleModel);
        return CreatedAtRoute(nameof(GetInmuebleById), new { inmuebleResponse.Id }, inmuebleResponse);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteInmueble(int id)
    {
        await _repository.DeleteInmueble(id);
        await _repository.SaveChanges();
        return Ok();
    }
}