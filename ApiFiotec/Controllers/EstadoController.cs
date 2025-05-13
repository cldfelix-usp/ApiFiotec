using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Contracts;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace ApiFiotec.Controllers;
    [ApiController]
    [Route("api/v1/estados")]
    public class EstadosController : ControllerBase
    {
        private readonly IEstadosService _estadosService;

        public EstadosController(IEstadosService estadosService)
        {
            _estadosService = estadosService;
        }

        [HttpGet(Name = "pegarTodosEstados")]
        public async Task<IActionResult> PegarTodosEstados(CancellationToken cancellationToken = default)
        {
            IEnumerable<EstadoResponseViewModel> estados = await _estadosService.PegarTodosEstados(cancellationToken);
            return Ok(new BaseResponseViewModel<IEnumerable<EstadoResponseViewModel>>(estados));
        }

}
