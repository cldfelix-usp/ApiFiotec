using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Contracts;
using ApiFiotec.Models;
using AutoMapper;

namespace ApiFiotec.Services;

public class EstadosService : IEstadosService
{
    private readonly IEstadosRepository _estadosRepository;
    private readonly IMapper _mapper;

    public EstadosService(IEstadosRepository estadosRepository, IMapper mapper)
    {
        _estadosRepository = estadosRepository;
        _mapper = mapper;
    }

    public async Task<List<EstadoResponseViewModel>> PegarTodosEstados(CancellationToken cancellationToken)
    {
        IEnumerable<Estado> estados = await _estadosRepository.PegarTodosEstados(cancellationToken);

        var mapped = _mapper.Map<List<EstadoResponseViewModel>>(estados);

        return mapped;
    }
}