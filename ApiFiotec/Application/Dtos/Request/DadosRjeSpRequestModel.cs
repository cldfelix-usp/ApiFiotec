using ApiFiotec.Contracts;

namespace ApiFiotec.Application.Dtos.Request;

public class DadosRjeSpRequestModel: SolicitanteRequestViewModel
{


    public DateTime DataSolicitacao { get; private set; } = DateTime.Now;
    public ushort SemanaInicio { get; set; } = 1;
    public ushort SemanaTermino { get; set; } = 1;
    public int[] CodigosIbge { get; private set; } = new[] { 3550308, 3304557 };
    public string[] Diseases { get; private set; } = new[] {"dengue","chikungunya","zika"};
    public string[] Minicipios { get; private set; } = new[] { "São Paulo", "Rio de Janeiro" };

    public void Validade()
    {
        if (SemanaInicio < 1 || SemanaInicio > 53)
            throw new ArgumentOutOfRangeException(nameof(SemanaInicio), "A semana epidemiológica de início deve estar entre 1 e 53");

        if (SemanaTermino < 1 || SemanaTermino > 53)
            throw new ArgumentOutOfRangeException(nameof(SemanaTermino), "A semana epidemiológica de término deve estar entre 1 e 53");

        if (SemanaInicio > SemanaTermino)
            throw new ArgumentException("A semana epidemiológica de início não pode ser maior que a de término");

    }

}