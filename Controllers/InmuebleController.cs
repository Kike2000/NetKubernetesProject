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
    public ActionResult<IEnumerable<InmuebleResponseDTO>> ObtenerInmuebles()
    {
        var inmuebles = _repository.GetAllInmuebles();
        return Ok(_mapper.Map<IEnumerable<InmuebleResponseDTO>>(inmuebles));
    }

    [HttpGet("{id}", Name = "GetInmuebleById")]
    public ActionResult<InmuebleResponseDTO> GetInmuebleById(int id)
    {
        var inmueble = _repository.GetInmuebleById(id);
        if (inmueble is null)
        {
            throw new MiddlewareException(HttpStatusCode.NotFound, new { mensaje = "Inmueble no fue encontrado." });
        }
        return Ok(_mapper.Map<InmuebleResponseDTO>(inmueble));
    }
    [HttpPost]
    public ActionResult<InmuebleResponseDTO> CreateInmueble([FromBody] InmuebleRequestDTO request)
    {
        var inmuebleModel = _mapper.Map<Inmueble>(request);
        _repository.CreateInmueble(inmuebleModel);
        _repository.SaveChanges();

        var inmuebleResponse = _mapper.Map<InmuebleResponseDTO>(inmuebleModel);
        return CreatedAtRoute(nameof(GetInmuebleById), new { inmuebleResponse.Id }, inmuebleResponse);
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteInmueble(int id)
    {
        _repository.DeleteInmueble(id);
        _repository.SaveChanges();
        return Ok();
    }
}