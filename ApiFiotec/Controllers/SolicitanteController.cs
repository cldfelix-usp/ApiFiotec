using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ApiFiotec.Controllers;

[ApiController]
[Route("api/v1/solicitantes")]
public class SolicitanteController : ControllerBase
{
    private readonly ISolicitanteService _solicitanteService;

    public SolicitanteController(ISolicitanteService solicitanteService)
    {
        _solicitanteService = solicitanteService;
    }

    [HttpGet(Name = "pegarTodosSolicitantes")]
    public async Task<IActionResult> PegarTodosSolicitantes(CancellationToken cancellationToken = default)
    {
        IEnumerable<SolicitanteResponseViewModel> solicitantes = await _solicitanteService.PegarTodosSolicitantes(cancellationToken);
        return Ok(new BaseResponseViewModel<IEnumerable<SolicitanteResponseViewModel>>(solicitantes));
    }

}