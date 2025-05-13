using ApiFiotec.Application.Dtos.Request;
using ApiFiotec.Application.Dtos.Response;

namespace ApiFiotec.Contracts;

public interface IInfoDengueService
{
    Task<IEnumerable<AlertCityResponseViewModel>> GetAlertCityAsync(InfoDengueRequestFilterRequestViewModel filter);

    Task<string> GetAlertCityRawAsync(InfoDengueRequestFilterRequestViewModel filter);
}