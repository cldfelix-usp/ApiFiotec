using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ApiFiotec.Controllers;

[ApiController]
[Route("api/v1/relatorios")]
public class RelatorioController : ControllerBase
{
    private readonly IRelatorioService _relatorioService;

    public RelatorioController(IRelatorioService relatorioService)
    {
        _relatorioService = relatorioService;
    }

    [HttpGet(Name = "pegarTodosRelatorios")]
    public async Task<IActionResult> PegarTodosRelatorios(CancellationToken cancellationToken = default)
    {
        var relatorios = await _relatorioService.GetRelatoriosAsync(cancellationToken);
        return Ok(new BaseResponseViewModel<IEnumerable<RelatorioResponseViewModel>>(relatorios));
    }
}
