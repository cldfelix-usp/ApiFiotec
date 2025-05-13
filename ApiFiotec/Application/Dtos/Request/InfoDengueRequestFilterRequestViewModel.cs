using ApiFiotec.Enums;

namespace ApiFiotec.Application.Dtos.Request;

public class InfoDengueRequestFilterRequestViewModel
{
    public Guid SolicitanteId { get; set; }
    
    public string GeoCode { get; set; } = null!;

    public DiseaseType Arbovirose { get; set; }

    public int EpidemiologicalWeekStart { get; set; }

    public int EpidemiologicalWeekEnd { get; set; }

    public int EpidemiologicalYearStart { get; set; }
    
    public int EpidemiologicalYearEnd { get; set; }
    
    public void Validate()
    {
        if (string.IsNullOrEmpty(GeoCode))
            throw new ArgumentException("O código IBGE da cidade é obrigatório", nameof(GeoCode));

        if (EpidemiologicalWeekStart < 1 || EpidemiologicalWeekStart > 53)
            throw new ArgumentOutOfRangeException(nameof(EpidemiologicalWeekStart), "A semana epidemiológica de início deve estar entre 1 e 53");

        if (EpidemiologicalWeekEnd < 1 || EpidemiologicalWeekEnd > 53)
            throw new ArgumentOutOfRangeException(nameof(EpidemiologicalWeekEnd), "A semana epidemiológica de término deve estar entre 1 e 53");

        if (EpidemiologicalWeekStart > EpidemiologicalWeekEnd)
            throw new ArgumentException("A semana epidemiológica de início não pode ser maior que a de término");

        if (EpidemiologicalYearStart < 0 || EpidemiologicalYearStart> 9999)
            throw new ArgumentOutOfRangeException(nameof(EpidemiologicalYearStart), "O ano de início deve ser positivo entre 1 e 9999");

        if (EpidemiologicalYearEnd < 0 || EpidemiologicalYearEnd > 9999)
            throw new ArgumentOutOfRangeException(nameof(EpidemiologicalYearEnd), "O ano de término deve ser positivo entre 1 e 9999");

        if (EpidemiologicalYearStart > EpidemiologicalYearEnd)
            throw new ArgumentException("O ano de início não pode ser maior que o de término");
    }
    
    public string ToQueryString()
    {
        Validate();

        var diseaseStr = Arbovirose.ToString().ToLower();

        return $"geocode={GeoCode}" +
               $"&disease={diseaseStr}" +
               $"&format=json" +
               $"&ew_start={EpidemiologicalWeekStart}" +
               $"&ew_end={EpidemiologicalWeekEnd}" +
               $"&ey_start={EpidemiologicalYearStart}" +
               $"&ey_end={EpidemiologicalYearEnd}";
    }
}
