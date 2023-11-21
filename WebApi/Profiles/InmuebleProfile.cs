
using AutoMapper;
using NetKubernetes.Dtos.InmuebleDTO;
using NetKubernetes.Models;

namespace NetKubernetes.Profiles;
public class InmuebleProfile : Profile
{
    public InmuebleProfile()
    {
        CreateMap<Inmueble, InmuebleResponseDTO>();
        CreateMap<InmuebleRequestDTO, Inmueble>();

    }
}