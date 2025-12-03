using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar las becas
/// </summary>
[Table("FEBEC_BECA")]
public partial class FebecBeca
{
    [Key]
    [Column("FEBEC_CODIGOBECA")]
    [StringLength(10)]
    [Unicode(false)]
    public string FebecCodigobeca { get; set; } = null!;

    [Column("FEBEC_NOMBREBECA")]
    [StringLength(50)]
    [Unicode(false)]
    public string FebecNombrebeca { get; set; } = null!;

    [Column("FEBEC_DESCRIPCIONB")]
    [StringLength(100)]
    [Unicode(false)]
    public string? FebecDescripcionb { get; set; }

    [Column("FEBEC_PORCENTAJE")]
    public int FebecPorcentaje { get; set; }

    [InverseProperty("FebecCodigobecaNavigation")]
    public virtual ICollection<FecafCabfac> FecafCabfacs { get; set; } = new List<FecafCabfac>();
}
