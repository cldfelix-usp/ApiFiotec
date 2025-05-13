using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiFiotec.Models;

[Table("tbl_solicitantes")]
[Index(nameof(Cpf), IsUnique = true, Name = "idx_cpf")]
public class Solicitante
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(100)]
    [Column("nome")]
    public string Nome { get; set; }
    
    [Required]
    [StringLength(11)]
    [Column("cpf")]
    public string Cpf { get; set; }
}