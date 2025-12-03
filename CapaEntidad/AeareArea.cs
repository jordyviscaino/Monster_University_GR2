using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar las Areas
/// </summary>
[Table("AEARE_AREA")]
public partial class AeareArea
{
    [Key]
    [Column("AEARE_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AeareCodigo { get; set; } = null!;

    [Column("AEARE_NOMBRE")]
    [StringLength(50)]
    [Unicode(false)]
    public string AeareNombre { get; set; } = null!;

    [InverseProperty("AeareCodigoNavigation")]
    public virtual ICollection<AeasiAsig> AeasiAsigs { get; set; } = new List<AeasiAsig>();
}
