using System.ComponentModel.DataAnnotations;
using ApiFiotec.Contracts;

namespace ApiFiotec.Application.Dtos.Request;

public class SolicitanteRequestViewModel
{
  


    public required string Cpf { get; set; }
    public required string Nome { get; set; }

    public void Validade()
    {
        if (string.IsNullOrWhiteSpace(Cpf))
            throw new ArgumentException("Cpf invalido, formeca um numero valido", nameof(Cpf));

        Cpf = new string(Cpf.Where(char.IsDigit).ToArray());

        if (Cpf.Length != 11 || Cpf.Distinct().Count() == 1)
            throw new ArgumentException("Cpf invalido, formeca um numero valido", nameof(Cpf));

        int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var digito1 = CalcularDigito(Cpf.Substring(0, 9), multiplicadores1);
        var digito2 = CalcularDigito(Cpf.Substring(0, 10), multiplicadores2);

        if(!Cpf.EndsWith($"{digito1}{digito2}"))
            throw new ArgumentException("Cpf invalido, formeca um numero valido", nameof(Cpf));
        
        if(Nome.Length  < 3)
            throw new ArgumentException("Nome deve ter no minimo 3 caracteres", nameof(Nome));
        
        if(Nome.Length  > 100)
            throw new ArgumentException("Nome deve ter no maximo 100 caracteres", nameof(Nome));

        //SolicitanteJaCadastrado();


    }

    private static int CalcularDigito(string cpfParcial, int[] multiplicadores)
    {
        var soma = cpfParcial.Select((c, i) => (c - '0') * multiplicadores[i]).Sum();
        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }



}