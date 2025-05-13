using System.Text.Json;
using ApiFiotec.Application.Dtos.Request;
using ApiFiotec.Application.Dtos.Response;
using ApiFiotec.Contracts;
using AutoMapper;

namespace ApiFiotec.Services;

public class InfoDengueService : IInfoDengueService
{
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    private readonly string _baseApiUrl;

    public InfoDengueService(IMapper mapper, HttpClient httpClient)
    {
        _mapper = mapper;
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _baseApiUrl = "https://info.dengue.mat.br/api/alertcity";
    }

    public async Task<IEnumerable<AlertCityResponseViewModel>> GetAlertCityAsync(InfoDengueRequestFilterRequestViewModel filter)
    {
        try
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            filter.Validate();

            string url = $"{_baseApiUrl}?{filter.ToQueryString()}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<AlertCityResponseViewModel>>(content, options);

            return result ?? new List<AlertCityResponseViewModel>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
  
    }

    public async Task<string> GetAlertCityRawAsync(InfoDengueRequestFilterRequestViewModel filter)
    {
        if (filter == null)
            throw new ArgumentNullException(nameof(filter));

        filter.Validate();

        string url = $"{_baseApiUrl}?{filter.ToQueryString()}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<IEnumerable<AlertCityResponseViewModel>> GetDadosPorCodigoIbge(ListarDadosPorCodigoRquestViewModel filter)
    {
        throw new NotImplementedException();
    }
}