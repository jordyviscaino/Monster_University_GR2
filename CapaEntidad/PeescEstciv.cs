using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar el estado civil
/// </summary>
[Table("PEESC_ESTCIV")]
public partial class PeescEstciv
{
    [Key]
    [Column("PEESC_CODIGO")]
    [StringLength(3)]
    [Unicode(false)]
    public string PeescCodigo { get; set; } = null!;

    [Column("PEESC_DESCRI")]
    [StringLength(50)]
    [Unicode(false)]
    public string PeescDescri { get; set; } = null!;

    [InverseProperty("PeescCodigoNavigation")]
    public virtual ICollection<PeperPer> PeperPers { get; set; } = new List<PeperPer>();
}
