using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Contracts;
using ApiFiotec.Models;
using AutoMapper;

namespace ApiFiotec.Services;

public class MunicipiosService : IMunicipiosService
{
   
    private readonly IMapper _mapper;
    private readonly IMunicipiosRepository _municipiosRepository;

    public MunicipiosService(IMapper mapper, IMunicipiosRepository municipiosRepository)
    {
        _mapper = mapper;
        _municipiosRepository = municipiosRepository;
    }



    public async Task<List<MunicipioResponseViewModel>> PegarTodosMunicipios()
    {
        var municipios = await _municipiosRepository.PegarTodosMunicipios();
        var mapped = _mapper.Map<List<MunicipioResponseViewModel>>(municipios);
        return mapped;

    }

    public async Task<List<MunicipioResponseViewModel>> PegarTodosMunicipiosPorEstadoAsync(uint estadoId, bool cancelationToken)
    {
      List<Municipio> municipios  = await _municipiosRepository.PegarTodosMunicipiosPorEstadoAsync(estadoId, cancelationToken);
      var mapped = _mapper.Map<List<MunicipioResponseViewModel>>(municipios);
      return mapped;
      
    }
}

