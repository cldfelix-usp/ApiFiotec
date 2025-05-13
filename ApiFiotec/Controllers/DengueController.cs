using ApiFiotec.Application.Dtos.Request;
using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Contracts;
using ApiFiotec.Enums;
using Microsoft.AspNetCore.Mvc;


namespace ApiFiotec.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DengueController : ControllerBase
    {
        private readonly IInfoDengueService _infoDengueService;
        private readonly IMunicipiosService _municipiosService;
        private readonly ISolicitanteService _solicitanteService;
        private readonly ILogger<DengueController> _logger;

        public DengueController(
            IInfoDengueService infoDengueService
            ,IMunicipiosService municipiosService
            ,ILogger<DengueController> logger, ISolicitanteService solicitanteService)
        {
            _infoDengueService = infoDengueService ;
            _municipiosService = municipiosService ;
            _solicitanteService = solicitanteService;
            _logger = logger;
        }

        private void SolicitanteJaCadastrado(string cpf, string nome)
        {
            var solicitanteJaCadastrado = _solicitanteService.SolicitanteJaCadastrado(cpf);
            if (!solicitanteJaCadastrado.Result)
                _ = _solicitanteService.CadastrarSolicitante(cpf, nome);
   
        }
        /*
         * Listar todos os dados epidemiológicos do município do Rio de Janeiro e
           São Paulo;
         */


        
        
        [HttpPost]
        [Route("GetDiseaseDataRjeSp")]
        public async Task<IActionResult> GetDiseaseDataRjeSp([FromBody] DadosRjeSpRequestModel request)
        {
            var result = new List<AlertCityResponseViewModel>();
            try
            {

                request.Validade();

                foreach (var i in request.CodigosIbge)
                {
                    var filter = new InfoDengueRequestFilterRequestViewModel()
                    {
                        GeoCode = i.ToString(),
                        Arbovirose = DiseaseType.Dengue,
                        EpidemiologicalWeekStart = request.SemanaInicio,
                        EpidemiologicalWeekEnd = request.SemanaTermino,
                        EpidemiologicalYearStart = DateTime.Now.Year,
                        EpidemiologicalYearEnd = DateTime.Now.Year
                    };
                    var dados = await _infoDengueService.GetAlertCityAsync(filter);
                    result.AddRange(dados);

                    var filter2 = new InfoDengueRequestFilterRequestViewModel()
                    {
                        GeoCode = i.ToString(),
                        Arbovirose = DiseaseType.Chikungunya,
                        EpidemiologicalWeekStart = request.SemanaInicio,
                        EpidemiologicalWeekEnd = request.SemanaTermino,
                        EpidemiologicalYearStart = DateTime.Now.Year,
                        EpidemiologicalYearEnd = DateTime.Now.Year
                    };
                    var dados2 = await _infoDengueService.GetAlertCityAsync(filter2);
                    result.AddRange(dados2);

                    var filter3 = new InfoDengueRequestFilterRequestViewModel()
                    {
                        GeoCode = i.ToString(),
                        Arbovirose = DiseaseType.Zika,
                        EpidemiologicalWeekStart = request.SemanaInicio,
                        EpidemiologicalWeekEnd = request.SemanaTermino,
                        EpidemiologicalYearStart = DateTime.Now.Year,
                        EpidemiologicalYearEnd = DateTime.Now.Year
                    };
                    var dados3 = await _infoDengueService.GetAlertCityAsync(filter3);
                    result.AddRange(dados3);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado ao processar a requisição");
                return BadRequest(new BaseResponseViewModel<IEnumerable<AlertCityResponseViewModel>>(ex.Message));
            }

            return Ok(new BaseResponseViewModel<IEnumerable<AlertCityResponseViewModel>>(result));

        }


        /*Listar os dados epidemiológicos dos municípios pelo código IBGE;*/
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDiseaseData(
            [FromBody] ListarDadosPorCodigoRquestViewModel request)
        {
            var result = new List<AlertCityResponseViewModel>();
            try
            {
                try
                {
                    request.Validate();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Erro ao validar a requisição");
                    return BadRequest(new BaseResponseViewModel<List<AlertCityResponseViewModel>>(e.Message));
                }

                foreach (var i in request.Diseases)
                {
                    var filter = new InfoDengueRequestFilterRequestViewModel()
                    {
                        GeoCode = request.CodigosIbge.ToString(),
                        Arbovirose = i switch
                        {
                            "dengue" => DiseaseType.Dengue,
                            "chikungunya" => DiseaseType.Chikungunya,
                            "zika" => DiseaseType.Zika,
                            _ => throw new ArgumentException("Tipo de doença inválido")
                        },
                        EpidemiologicalWeekStart = request.SemanaInicio,
                        EpidemiologicalWeekEnd = request.SemanaTermino,
                        EpidemiologicalYearStart = DateTime.Now.Year,
                        EpidemiologicalYearEnd = DateTime.Now.Year
                    };

                    var dados = await _infoDengueService.GetAlertCityAsync(filter);
                    result.AddRange(dados);
                }

                if (result.Count == 0)
                {
                    _logger.LogInformation("Nenhum dado encontrado para o código IBGE {geoCode}", request.CodigosIbge);
                    return NotFound(
                        new BaseResponseViewModel<List<AlertCityResponseViewModel>>(
                            $"Nenhum dado encontrado para o código IBGE {request.CodigosIbge}."));
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Erro ao acessar o serviço externo InfoDengue");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new BaseResponseViewModel<List<AlertCityResponseViewModel>>(
                    "Erro ao acessar o serviço InfoDengue. Tente novamente mais tarde."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado ao processar a requisição");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new BaseResponseViewModel<List<AlertCityResponseViewModel>>("Ocorreu um erro ao processar a solicitação."));
            }

            return Ok(new BaseResponseViewModel<IEnumerable<AlertCityResponseViewModel>>(result));
        }


        /// <summary>
        /// Obtém dados consolidados sobre uma doença para uma cidade em um período específico
        /// </summary>
        /// <param name="geoCode">Código IBGE da cidade</param>
        /// <param name="disease">Tipo de doença (dengue, zika, chikungunya)</param>
        /// <param name="year">Ano para consulta</param>
        /// <returns>Dados consolidados sobre a doença</returns>
        [HttpGet("summary/{disease}/{geoCode}/{year}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDiseaseSummary(
            [FromRoute] string geoCode,
            [FromRoute] string disease,
            [FromRoute] int year)
        {
            try
            {
                _logger.LogInformation("Consultando resumo para {disease} na cidade {geoCode} em {year}", disease, geoCode, year);

                // Valida e converte o tipo de doença
                if (!Enum.TryParse<DiseaseType>(disease, true, out var diseaseType))
                {
                    _logger.LogWarning("Tipo de doença inválido: {disease}", disease);
                    return BadRequest($"Tipo de doença inválido: {disease}. Use 'dengue', 'zika' ou 'chikungunya'.");
                }

                var filter = new InfoDengueRequestFilterRequestViewModel()
                {
                    GeoCode = geoCode,
                    Arbovirose = diseaseType,
                    EpidemiologicalWeekStart = 1,
                    EpidemiologicalWeekEnd = 53,  // Consulta o ano completo
                    EpidemiologicalYearStart = year,
                    EpidemiologicalYearEnd = year
                };

                var result = await _infoDengueService.GetAlertCityAsync(filter);

                if (!result.Any())
                {
                    _logger.LogInformation("Nenhum dado encontrado para {disease} na cidade {geoCode} em {year}", disease, geoCode, year);
                    return NotFound($"Nenhum dado encontrado para {disease} na cidade {geoCode} no ano {year}.");
                }

                // Calcula estatísticas resumidas
                var summary = new
                {
                    Disease = disease,
                    GeoCode = geoCode,
                    Year = year,
                    TotalCases = result.Sum(r => r.Casos),
                    AverageCasesPerWeek = Math.Round(result.Average(r => r.Casos), 2),
                    MaxCasesInWeek = result.Max(r => r.Casos),
                    WeeksWithData = result.Count(),
                    WeeksWithAlerts = result.Count(r => r.Nivel >= 2),  // Considerando nível 2+ como alerta
                    HighestAlertLevel = result.Max(r => r.Nivel),
                    //FirstCaseDate = result.Min(r => r.Date),
                    //LastCaseDate = result.Max(r => r.Date)
                };

                _logger.LogInformation("Retornando resumo para {disease} na cidade {geoCode} em {year}", disease, geoCode, year);
                return Ok(summary);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Erro ao acessar o serviço externo InfoDengue");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Erro ao acessar o serviço InfoDengue. Tente novamente mais tarde.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado ao processar a requisição");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar a solicitação.");
            }
        }

        /*Listar o total de casos epidemiológicos dos municípios do Rio de Janeiro
e São Paulo;*/
        [HttpPost("GetTotalCasos")]
        public async Task<IActionResult> GetTotalCasos([FromBody] DadosRjeSpRequestModel request)
        {
            var response2 = new Dictionary<string, object>();
            try
            {
                try
                {
                    request.Validade();
                    SolicitanteJaCadastrado(request.Cpf, request.Nome);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Erro ao validar a requisição");
                    return BadRequest(new BaseResponseViewModel<Dictionary<string, object>>(e.Message));
                }

                foreach (var i in request.CodigosIbge)
                {
                    var filter = new InfoDengueRequestFilterRequestViewModel()
                    {
                        GeoCode = i.ToString(),
                        Arbovirose = DiseaseType.Dengue,
                        EpidemiologicalWeekStart = request.SemanaInicio,
                        EpidemiologicalWeekEnd = request.SemanaTermino,
                        EpidemiologicalYearStart = DateTime.Now.Year,
                        EpidemiologicalYearEnd = DateTime.Now.Year
                    };
                    var result = await _infoDengueService.GetAlertCityAsync(filter);

                    var summary = new
                    {
                        City = i.ToString(),
                        Disease = DiseaseType.Dengue,
                        Period = $"{DateTime.Now.Year}-{DateTime.Now.Year}, Weeks {request.SemanaInicio}-{request.SemanaTermino}",
                        TotalCases = result.Sum(r => r.Casos),
                        AverageCases = result.Any() ? Math.Round(result.Average(r => r.Casos), 2) : 0,
                        MaxCases = result.Any() ? result.Max(r => r.Casos) : 0,
                        MaxCasesWeek = result.Any() ? result.FirstOrDefault(r => r.Casos == result.Max(r => r.Casos))?.Se : 0,
                        MaxCasesDate = result.Any() ? result.FirstOrDefault(r => r.Casos == result.Max(r => r.Casos))?.DataIniSe : 0,
                        AlertLevels = result.GroupBy(r => r.Nivel).Select(g => new
                        {
                            Level = g.Key,
                            Count = g.Count(),
                            Percentage = result.Any() ? Math.Round((double)g.Count() / result.Count() * 100, 2) : 0
                        })
                    };

                    response2.Add($"Dengue:{i}", summary);

                    var filter1 = new InfoDengueRequestFilterRequestViewModel()
                    {
                        GeoCode = i.ToString(),
                        Arbovirose = DiseaseType.Chikungunya,
                        EpidemiologicalWeekStart = request.SemanaInicio,
                        EpidemiologicalWeekEnd = request.SemanaTermino,
                        EpidemiologicalYearStart = DateTime.Now.Year,
                        EpidemiologicalYearEnd = DateTime.Now.Year
                    };
                    var result1 = await _infoDengueService.GetAlertCityAsync(filter);

                    var summary1 = new
                    {
                        City = i.ToString(),
                        Disease = DiseaseType.Chikungunya,
                        Period = $"{DateTime.Now.Year}-{DateTime.Now.Year}, Weeks {request.SemanaInicio}-{request.SemanaTermino}",
                        TotalCases = result1.Sum(r => r.Casos),
                        AverageCases = result1.Any() ? Math.Round(result1.Average(r => r.Casos), 2) : 0,
                        MaxCases = result1.Any() ? result1.Max(r => r.Casos) : 0,
                        MaxCasesWeek = result1.Any() ? result1.FirstOrDefault(r => r.Casos == result1.Max(r => r.Casos))?.Se : 0,
                        MaxCasesDate = result1.Any() ? result1.FirstOrDefault(r => r.Casos == result1.Max(r => r.Casos))?.DataIniSe : 0,
                        AlertLevels = result1.GroupBy(r => r.Nivel).Select(g => new
                        {
                            Level = g.Key,
                            Count = g.Count(),
                            Percentage = result1.Any() ? Math.Round((double)g.Count() / result1.Count() * 100, 2) : 0
                        })
                    };
                    response2.Add($"Chikungunya:{i}", summary1);
                    var filter2 = new InfoDengueRequestFilterRequestViewModel()
                    {
                        GeoCode = i.ToString(),
                        Arbovirose = DiseaseType.Zika,
                        EpidemiologicalWeekStart = request.SemanaInicio,
                        EpidemiologicalWeekEnd = request.SemanaTermino,
                        EpidemiologicalYearStart = DateTime.Now.Year,
                        EpidemiologicalYearEnd = DateTime.Now.Year
                    };
                    var result2 = await _infoDengueService.GetAlertCityAsync(filter);
                    var summary2 = new
                    {
                        City = i.ToString(),
                        Disease = DiseaseType.Zika,
                        Period = $"{DateTime.Now.Year}-{DateTime.Now.Year}, Weeks {request.SemanaInicio}-{request.SemanaTermino}",
                        TotalCases = result2.Sum(r => r.Casos),
                        AverageCases = result2.Any() ? Math.Round(result2.Average(r => r.Casos), 2) : 0,
                        MaxCases = result2.Any() ? result2.Max(r => r.Casos) : 0,
                        MaxCasesWeek = result2.Any() ? result2.FirstOrDefault(r => r.Casos == result2.Max(r => r.Casos))?.Se : 0,
                        MaxCasesDate = result2.Any() ? result2.FirstOrDefault(r => r.Casos == result2.Max(r => r.Casos))?.DataIniSe : 0,
                        AlertLevels = result2.GroupBy(r => r.Nivel).Select(g => new
                        {
                            Level = g.Key,
                            Count = g.Count(),
                            Percentage = result2.Any() ? Math.Round((double)g.Count() / result2.Count() * 100, 2) : 0
                        })
                    };
                    response2.Add($"Zika:{i}", summary1);
             
                }

                if (response2.Count == 0)
                {
                    _logger.LogInformation("Nenhum dado encontrado para o código IBGE {geoCode}", request.CodigosIbge);
                    return NotFound(
                        new BaseResponseViewModel<Dictionary<string, object>>(
                            $"Nenhum dado encontrado para o código IBGE {request.CodigosIbge}."));
                }



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado ao processar a requisição");
                return BadRequest(new BaseResponseViewModel<Dictionary<string, object>>(ex.Message));
            }

            return Ok(new BaseResponseViewModel<Dictionary<string, object>>(response2));
        }


        //Listar o total de casos epidemiológicos dos municípios por arbovirose;
        [HttpPost("GetTotalCasosArbovirose")]
        public async Task<IActionResult> GetTotalCasosArbovirose([FromBody] DadosRjeSpRequestModel request)
        {
            var response2 = new Dictionary<string, object>();
            try
            {
                try
                {
                    request.Validade();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Erro ao validar a requisição");
                    return BadRequest(new BaseResponseViewModel<Dictionary<string, object>>(e.Message));
                }

                var municipios = await _municipiosService.PegarTodosMunicipios();
             

                foreach (var i in municipios)
                {
                    var filter = new InfoDengueRequestFilterRequestViewModel()
                    {
                        GeoCode = i.Id.ToString(),
                        Arbovirose = DiseaseType.Dengue,
                        EpidemiologicalWeekStart = request.SemanaInicio,
                        EpidemiologicalWeekEnd = request.SemanaTermino,
                        EpidemiologicalYearStart = DateTime.Now.Year,
                        EpidemiologicalYearEnd = DateTime.Now.Year
                    };
                    var result = await _infoDengueService.GetAlertCityAsync(filter);

                    var summary = new
                    {
                        City = i.NomeMunicipio,
                        Disease = DiseaseType.Dengue,
                        TotalCases = result.Sum(r => r.Casos),
                    };

                    response2.Add($"Dengue:{i.NomeMunicipio}-{i.Id}", summary);

                    var filter1 = new InfoDengueRequestFilterRequestViewModel()
                    {
                        GeoCode = i.Id.ToString(),
                        Arbovirose = DiseaseType.Chikungunya,
                        EpidemiologicalWeekStart = request.SemanaInicio,
                        EpidemiologicalWeekEnd = request.SemanaTermino,
                        EpidemiologicalYearStart = DateTime.Now.Year,
                        EpidemiologicalYearEnd = DateTime.Now.Year
                    };
                    var result1 = await _infoDengueService.GetAlertCityAsync(filter1);
                    var summary1 = new
                    {
                        City = i.NomeMunicipio,
                        Disease = DiseaseType.Chikungunya,
                        TotalCases = result1.Sum(r => r.Casos),
                    };
                    response2.Add($"Chikungunya:{i.NomeMunicipio}-{i.Id}", summary1);
                    var filter2 = new InfoDengueRequestFilterRequestViewModel()
                    {
                        GeoCode = i.Id.ToString(),
                        Arbovirose = DiseaseType.Zika,
                        EpidemiologicalWeekStart = request.SemanaInicio,
                        EpidemiologicalWeekEnd = request.SemanaTermino,
                        EpidemiologicalYearStart = DateTime.Now.Year,
                        EpidemiologicalYearEnd = DateTime.Now.Year
                    };
                    var result2 = await _infoDengueService.GetAlertCityAsync(filter2);      
                    var summary2 = new
                    {
                        City = i.NomeMunicipio,
                        Disease = DiseaseType.Zika,
                        TotalCases = result2.Sum(r => r.Casos),
                    };
                    response2.Add($"Zika:{i.NomeMunicipio}-{i.Id}", summary2);
                }
                if (response2.Count == 0)
                {
                    _logger.LogInformation("Nenhum dado encontrado para o código IBGE {geoCode}", request.CodigosIbge);
                    return NotFound(
                        new BaseResponseViewModel<Dictionary<string, object>>(
                            $"Nenhum dado encontrado para o código IBGE {request.CodigosIbge}."));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro não tratado ao processar a requisição");
                return BadRequest(new BaseResponseViewModel<Dictionary<string, object>>(ex.Message));
            }
            return Ok(new BaseResponseViewModel<Dictionary<string, object>>(response2));
        }



                
    }

}