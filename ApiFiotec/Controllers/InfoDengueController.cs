using ApiFiotec.Application.Dtos.Request;
using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Contracts;
using ApiFiotec.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ApiFiotec.Controllers
{
    [ApiController]
    [Route("api/v1/infodengue")]
    public class InfoDengueController : ControllerBase
    {
        private readonly IInfoDengueService _infoDengueService;
        private readonly IMunicipiosService _municipiosService;
        private readonly ISolicitanteService _solicitanteService;
        private readonly ILogger<InfoDengueController> _logger;

        public InfoDengueController(
            IInfoDengueService infoDengueService,
            IMunicipiosService municipiosService,
            ISolicitanteService solicitanteService,
            ILogger<InfoDengueController> logger)
        {
            _infoDengueService = infoDengueService;
            _municipiosService = municipiosService;
            _solicitanteService = solicitanteService;
            _logger = logger;
        }

        /// <summary>
        /// Gets or creates the solicitante record based on CPF
        /// </summary>
        private async Task<SolicitanteResponseViewModel> GetOrCreateSolicitante(string cpf, string nome, CancellationToken cancellationToken = default)
        {
            var solicitanteExists = await _solicitanteService.SolicitanteJaCadastrado(cpf, cancellationToken);
            
            return solicitanteExists
                ? await _solicitanteService.PegarSolicitantePorCpf(cpf, cancellationToken)
                : await _solicitanteService.CadastrarSolicitante(cpf, nome, cancellationToken);
        }

        /// <summary>
        /// Creates a standard filter model for InfoDengue requests
        /// </summary>
        private InfoDengueRequestFilterRequestViewModel CreateDiseaseFilter(
            string geoCode, 
            DiseaseType diseaseType, 
            int weekStart, 
            int weekEnd, 
            int yearStart, 
            int yearEnd,
            Guid solicitanteId)
        {
            var model = new InfoDengueRequestFilterRequestViewModel();
            model.GeoCode = geoCode;
            model.Arbovirose = diseaseType;
            model.EpidemiologicalWeekStart = weekStart;
            model.EpidemiologicalWeekEnd = weekEnd;
            model.EpidemiologicalYearStart = yearStart;
            model.EpidemiologicalYearEnd = yearEnd;
            model.SolicitanteId = solicitanteId;
            return model;
        }

        /// <summary>
        /// Helper method to get disease data for multiple disease types
        /// </summary>
        private async Task<List<AlertCityResponseViewModel>> GetDiseaseDataForTypes(
            string geoCode,
            IEnumerable<DiseaseType> diseaseTypes,
            int weekStart,
            int weekEnd,
            int yearStart,
            int yearEnd,
            Guid solicitanteId)
        {
            var result = new List<AlertCityResponseViewModel>();

            foreach (var diseaseType in diseaseTypes)
            {
                var filter = CreateDiseaseFilter(geoCode, diseaseType, weekStart, weekEnd, yearStart, yearEnd, solicitanteId);
                var data = await _infoDengueService.GetAlertCityAsync(filter);
                result.AddRange(data);
            }

            return result;
        }

        /// <summary>
        /// Creates a disease summary object from alert city data
        /// </summary>
        private object CreateDiseaseSummary(
            string cityCode, 
            string cityName, 
            DiseaseType diseaseType, 
            IEnumerable<AlertCityResponseViewModel> data,
            int weekStart,
            int weekEnd,
            int year)
        {
            return new
            {
                Cidade = string.IsNullOrEmpty(cityName) ? cityCode : cityName,
                Doenca = diseaseType,
                Periodo = $"{year}, Weeks {weekStart}-{weekEnd}",
                TotalDeCasos = data.Sum(r => r.Casos),
                MediaDeCasos = data.Any() ? Math.Round(data.Average(r => r.Casos), 2) : 0,
                MaximoCasos = data.Any() ? data.Max(r => r.Casos) : 0,
                MaximoCasosNaSemana = data.Any() ? data.FirstOrDefault(r => r.Casos == data.Max(r => r.Casos))?.Se : 0,
                MaxCasesDate = data.Any() ? data.FirstOrDefault(r => r.Casos == data.Max(r => r.Casos))?.DataIniSe : 0,
                NiveisDeAlertas = data.GroupBy(r => r.Nivel).Select(g => new
                {
                    Nivel = g.Key,
                    Quantidade = g.Count(),
                    Porcentagem = data.Any() ? Math.Round((double)g.Count() / data.Count() * 100, 2) : 0
                })
            };
        }

        /// <summary>
        /// Helper method to handle common response errors
        /// </summary>
        private IActionResult HandleError<T>(Exception ex, string customMessage = null)
        {
            if (ex is HttpRequestException)
            {
                _logger.LogError(ex, "Erro ao acessar o serviço externo InfoDengue");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, 
                    new BaseResponseViewModel<T>("Erro ao acessar o serviço InfoDengue. Tente novamente mais tarde."));
            }
            
            _logger.LogError(ex, "Erro não tratado ao processar a requisição");
            
            return ex is ArgumentException
                ? BadRequest(new BaseResponseViewModel<T>(customMessage ?? ex.Message))
                : StatusCode(StatusCodes.Status500InternalServerError, 
                    new BaseResponseViewModel<T>(customMessage ?? "Ocorreu um erro ao processar a solicitação."));
        }

        /// <summary>
        /// Lists epidemiological data for Rio de Janeiro and São Paulo
        /// </summary>
        [HttpPost]
        [Route("getDadosRjeSp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDiseaseDataRjeSp([FromBody] DadosRjeSpRequestModel request)
        {
            try
            {
                request.Validade();
                var solicitante = await GetOrCreateSolicitante(request.Cpf, request.Nome);
                var result = new List<AlertCityResponseViewModel>();
                var currentYear = DateTime.Now.Year;
                var allDiseaseTypes = new[] { DiseaseType.Dengue, DiseaseType.Chikungunya, DiseaseType.Zika };

                foreach (var geoCode in request.CodigosIbge)
                {
                    var data = await GetDiseaseDataForTypes(
                        geoCode.ToString(),
                        allDiseaseTypes,
                        request.SemanaInicio,
                        request.SemanaTermino,
                        currentYear,
                        currentYear,
                        solicitante.Id);
                    
                    result.AddRange(data);
                }

                return Ok(new BaseResponseViewModel<IEnumerable<AlertCityResponseViewModel>>(result));
            }
            catch (Exception ex)
            {
                return HandleError<IEnumerable<AlertCityResponseViewModel>>(ex);
            }
        }

        /// <summary>
        /// Lists epidemiological data for municipalities by IBGE code
        /// </summary>
        [HttpPost]
        [Route("pegarDadosPorCodigoIbge")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDiseaseData([FromBody] ListarDadosPorCodigoRquestViewModel request)
        {
            try
            {
                request.Validate();
                var solicitante = await GetOrCreateSolicitante(request.Cpf, request.Nome);
                var result = new List<AlertCityResponseViewModel>();
                var currentYear = DateTime.Now.Year;

                foreach (var diseaseName in request.Diseases)
                {
                    var diseaseType = diseaseName.ToLower() switch
                    {
                        "dengue" => DiseaseType.Dengue,
                        "chikungunya" => DiseaseType.Chikungunya,
                        "zika" => DiseaseType.Zika,
                        _ => throw new ArgumentException($"Tipo de doença inválido: {diseaseName}")
                    };

                    var filter = CreateDiseaseFilter(
                        request.CodigosIbge.ToString(),
                        diseaseType,
                        request.SemanaInicio,
                        request.SemanaTermino,
                        currentYear,
                        currentYear,
                        solicitante.Id);

                    var data = await _infoDengueService.GetAlertCityAsync(filter);
                    result.AddRange(data);
                }

                if (result.Count == 0)
                {
                    _logger.LogInformation("Nenhum dado encontrado para o código IBGE {geoCode}", request.CodigosIbge);
                    return NotFound(
                        new BaseResponseViewModel<List<AlertCityResponseViewModel>>(
                            $"Nenhum dado encontrado para o código IBGE {request.CodigosIbge}."));
                }

                return Ok(new BaseResponseViewModel<IEnumerable<AlertCityResponseViewModel>>(result));
            }
            catch (Exception ex)
            {
                return HandleError<List<AlertCityResponseViewModel>>(ex);
            }
        }

        
        /// <summary>
        /// Lists total epidemiological cases for Rio de Janeiro and São Paulo
        /// </summary>
        [HttpPost("getDadosSumarizados")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTotalCasos([FromBody] DadosRjeSpRequestModel request)
        {
            try
            {
                request.Validade();
                var solicitante = await GetOrCreateSolicitante(request.Cpf, request.Nome);
                var response = new Dictionary<string, object>();
                var currentYear = DateTime.Now.Year;
                var allDiseaseTypes = new[] { DiseaseType.Dengue, DiseaseType.Chikungunya, DiseaseType.Zika };

                foreach (var geoCode in request.CodigosIbge)
                {
                    var geoCodeStr = geoCode.ToString();
                    
                    foreach (var diseaseType in allDiseaseTypes)
                    {
                        var filter = CreateDiseaseFilter(
                            geoCodeStr,
                            diseaseType,
                            request.SemanaInicio,
                            request.SemanaTermino,
                            currentYear,
                            currentYear,
                            solicitante.Id);
                        
                        var result = await _infoDengueService.GetAlertCityAsync(filter);
                        var summary = CreateDiseaseSummary(
                            geoCodeStr, 
                            null, 
                            diseaseType, 
                            result,
                            request.SemanaInicio,
                            request.SemanaTermino,
                            currentYear);
                        
                        response.Add($"{diseaseType}:{geoCodeStr}", summary);
                    }
                }

                if (response.Count == 0)
                {
                    _logger.LogInformation("Nenhum dado encontrado para os códigos IBGE solicitados");
                    return NotFound(
                        new BaseResponseViewModel<Dictionary<string, object>>(
                            "Nenhum dado encontrado para os códigos IBGE solicitados."));
                }

                return Ok(new BaseResponseViewModel<Dictionary<string, object>>(response));
            }
            catch (Exception ex)
            {
                return HandleError<Dictionary<string, object>>(ex);
            }
        }

        /// <summary>
        /// Lists total epidemiological cases by arbovirus for all municipalities
        /// </summary>
        [HttpPost("getDadosTotalArbovirose")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTotalCasosArbovirose(
            [FromBody] DadosRjeSpRequestModel request, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                request.Validade();
                var solicitante = await GetOrCreateSolicitante(request.Cpf, request.Nome);
                var response = new Dictionary<string, object>();
                var currentYear = DateTime.Now.Year;
                var allDiseaseTypes = new[] { DiseaseType.Dengue, DiseaseType.Chikungunya, DiseaseType.Zika };
                
                var municipios = await _municipiosService.PegarTodosMunicipios(cancellationToken);

                foreach (var municipio in municipios)
                {
                    var geoCodeStr = municipio.Id.ToString();
                    
                    foreach (var diseaseType in allDiseaseTypes)
                    {
                        var filter = CreateDiseaseFilter(
                            geoCodeStr,
                            diseaseType,
                            request.SemanaInicio,
                            request.SemanaTermino,
                            currentYear,
                            currentYear,
                            solicitante.Id);
                        
                        var result = await _infoDengueService.GetAlertCityAsync(filter);
                        
                        var summary = new
                        {
                            Cidade = municipio.NomeMunicipio,
                            Doenca = diseaseType,
                            CasosTotais = result.Sum(r => r.Casos)
                        };
                        
                        var key = $"{diseaseType}:{municipio.NomeMunicipio}-{municipio.Id}";
                        response.Add(key, summary);
                    }
                }

                if (response.Count == 0)
                {
                    _logger.LogInformation("Nenhum dado encontrado");
                    return NotFound(
                        new BaseResponseViewModel<Dictionary<string, object>>("Nenhum dado encontrado."));
                }

                return Ok(new BaseResponseViewModel<Dictionary<string, object>>(response));
            }
            catch (Exception ex)
            {
                return HandleError<Dictionary<string, object>>(ex);
            }
        }
    }
}