using ApiFiotec.Enums;
using ApiFiotec.Models;

namespace ApiFiotec.Application.Dtos.Request;

public class RelatorioRequestViewModel
{
        public DateTime Data { get; private set; } = DateTime.Now;

        public DiseaseType Arbovirose { get; set; }

        public Guid SolicitanteId { get; set; }

        public int SemanaInicio { get; set; }

        public int SemanaTermino { get; set; }

        public int CodigoIbge { get; set; }

        public required string Municipio { get; set; }

        public required string DadosRelatorio { get; set; }
}
