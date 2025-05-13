using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ApiFiotec.Controllers;

[ApiController]
[Route("api/v1/municipios")]
public class MunicipiosController : ControllerBase
{
    private readonly IMunicipiosService _municipiosService;

    public MunicipiosController(IMunicipiosService municipiosService)
    {
        _municipiosService = municipiosService;
    }

    [HttpGet]
    [Route("pegarTodosMunicipios")]
    public async Task<IActionResult> PegarTodosMunicipios(bool cancelationToken = false)
    {
        var municipios = await _municipiosService.PegarTodosMunicipios();
        return Ok(new  BaseResponseViewModel<List<MunicipioResponseViewModel>>(municipios));
    }
        
    [HttpGet]
    [Route( "pegarTodosMunicipiosPorEstadoAsync/{estadoId}")]
    public async Task<IActionResult> PegarTodosMunicipiosPorEstadoAsync([FromRoute] uint estadoId)
    {
        var municipios = await _municipiosService.PegarTodosMunicipiosPorEstadoAsync(estadoId, false);
        return Ok(new  BaseResponseViewModel<List<MunicipioResponseViewModel>>(municipios));
    }

}