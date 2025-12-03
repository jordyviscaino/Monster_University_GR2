using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar las aulas
/// </summary>
[Table("AEAUL_AULA")]
public partial class AeaulAula
{
    [Key]
    [Column("AEAUL_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AeaulCodigo { get; set; } = null!;

    [Column("AEAUL_NOMBRE")]
    [StringLength(50)]
    [Unicode(false)]
    public string AeaulNombre { get; set; } = null!;

    [Column("AEAUL_CAPACIDAD")]
    public int AeaulCapacidad { get; set; }

    [Column("AEAUL_UBICACION")]
    [StringLength(100)]
    [Unicode(false)]
    public string AeaulUbicacion { get; set; } = null!;

    [InverseProperty("AeaulCodigoNavigation")]
    public virtual ICollection<AegruGrup> AegruGrups { get; set; } = new List<AegruGrup>();
}
