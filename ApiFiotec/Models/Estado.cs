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
            public string NomeUf { get; set; }
            public IEnumerable<Municipio> Municipios { get; set; }
    }
}
