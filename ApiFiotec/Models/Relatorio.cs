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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Column("data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

        [Required]
        [Column("arbovirose")]
        [StringLength(100)]
        public string Arbovirose { get; set; }

        [Required]
        [Column("solicitante_id")]
        public Solicitante Solicitante { get; set; }

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
        public string Municipio { get; set; }

        [Required]
        [Column("dados_relatorio")]
        public string DadosRelatorio { get; set; }

    }
}
