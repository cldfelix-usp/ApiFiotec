using ApiFiotec.Models;

namespace ApiFiotec.Application.Dtos.Request;

public class RelatorioRequestViewModel
{
        public DateTime Data { get; private set; } = DateTime.Now;

        public string Arbovirose { get; set; }

        public Solicitante Solicitante { get; set; }

        public int SemanaInicio { get; set; }

        public int SemanaTermino { get; set; }

        public int CodigoIbge { get; set; }

        public string Municipio { get; set; }

        public string DadosRelatorio { get; set; }
}
