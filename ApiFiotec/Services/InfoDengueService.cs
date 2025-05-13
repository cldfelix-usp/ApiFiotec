using System.Text.Json;
using ApiFiotec.Application.Dtos.Request;
using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Contracts;
using ApiFiotec.Models;
using AutoMapper;

namespace ApiFiotec.Services;

public class InfoDengueService : IInfoDengueService
{
    private readonly HttpClient _httpClient;
    private readonly IRelatorioService _relatoriosService;
    private readonly ILogger<InfoDengueService> _logger;
    private readonly string _baseApiUrl;

    public InfoDengueService( HttpClient httpClient, IRelatorioService relatoriosService, ILogger<InfoDengueService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _relatoriosService = relatoriosService ?? throw new ArgumentNullException(nameof(relatoriosService));
        _baseApiUrl = "https://info.dengue.mat.br/api/alertcity";
    }

    private async Task GravarRelatorioAsync(AlertCityResponseViewModel content,
        InfoDengueRequestFilterRequestViewModel request, CancellationToken cancellationToken = default)
    {
        try
        {
            
            var relatorioRequestViewModel = new RelatorioRequestViewModel
            {
                DadosRelatorio = JsonSerializer.Serialize(content),
                Municipio = request.GeoCode
            };
            relatorioRequestViewModel.Arbovirose = request.Arbovirose;
            relatorioRequestViewModel.CodigoIbge = int.Parse(request.GeoCode);
            relatorioRequestViewModel.SemanaInicio = request.EpidemiologicalWeekStart;
            relatorioRequestViewModel.SemanaTermino = request.EpidemiologicalWeekEnd;
            relatorioRequestViewModel.SolicitanteId = request.SolicitanteId;
            await _relatoriosService.CriarRelatorioAsync(relatorioRequestViewModel, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erro ao gravar relatório");
            throw new Exception("Erro ao gravar relatório", e);
        }


    }

    public async Task<IEnumerable<AlertCityResponseViewModel>> GetAlertCityAsync(InfoDengueRequestFilterRequestViewModel filter)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(filter);

            filter.Validate();

            var url = $"{_baseApiUrl}?{filter.ToQueryString()}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<AlertCityResponseViewModel>>(content, options);

            if (result != null)
                foreach (var item in result)
                    await GravarRelatorioAsync(item, filter);

            return result ?? new List<AlertCityResponseViewModel>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erro ao buscar dados da API InfoDengue");
            throw new Exception("Erro ao buscar dados da API InfoDengue", e);
        }
  
    }

    public async Task<string> GetAlertCityRawAsync(InfoDengueRequestFilterRequestViewModel filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        filter.Validate();

        var url = $"{_baseApiUrl}?{filter.ToQueryString()}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }


}