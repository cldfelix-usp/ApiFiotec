using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ApiFiotec.Models
{
    [Table("tbl_relatorios")]
    public class Relatorio
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

        [Required]
        [Column("arbovirose")]
        [StringLength(100)]
        public required string Arbovirose { get; set; }

        [Required]
        [Column("solicitanteId")]
        public Guid SolicitanteId { get; set; }
        public virtual required Solicitante Solicitante { get; set; }

        [Required]
        [Column("semana_inicio")]
        public int SemanaInicio { get; set; }

        [Required]
        [Column("semana_termino")]
        public int SemanaTermino { get; set; }

        [Required]
        [Column("codigo_ibge")]
        [Range(1, 9999999)]

        public int CodigoIbge { get; set; }

        [Required]
        [Column("municipio")]
        [StringLength(100)]
        public required string Municipio { get; set; }

        [Required]
        [Column("dados_relatorio")]
        public required string DadosRelatorio { get; set; }

    }
}
