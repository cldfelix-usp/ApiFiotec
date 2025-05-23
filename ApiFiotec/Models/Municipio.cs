﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiFiotec.Application.Dtos.Response;

namespace ApiFiotec.Models;

[Table("tbl_municipios")]
public class Municipio
{
    [Key]
    [Column("id")]
    public uint Id { get; set; }
    
    [Required]
    [StringLength(100)]
    [Column("municipio")]
    public required string NomeMunicipio { get; set; }
    public required Estado Estado { get; set; }
}