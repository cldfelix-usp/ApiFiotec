using ApiFiotec.Application.Dtos.Request;
using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Models;
using AutoMapper;

namespace ApiFiotec.Application.Map
{
    public class AoiMappingProfile: Profile
    {
        public AoiMappingProfile()
        {

            // Responses
            CreateMap<Estado, EstadoResponseViewModel>();
            CreateMap<Municipio, MunicipioResponseViewModel>();
            CreateMap<Solicitante, SolicitanteResponseViewModel>();
            
            // Requests
            CreateMap<SolicitanteRequestViewModel, Solicitante>();
        }
    }
}
