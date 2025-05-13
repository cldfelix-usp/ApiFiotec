using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiFiotec.Models
{
    [Table("tbl_estados")]
    public class Estado
    {

        [Key]
        [Column("id")]
        public ushort Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("uf")]
        public required string NomeUf { get; set; }
        public required IEnumerable<Municipio> Municipios { get; set; }
    }
}
