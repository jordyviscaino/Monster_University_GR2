using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad utilizada para representar el CARGO dentro de un DEPARTAMENTO
/// </summary>
[PrimaryKey("PedepCodigo", "PerolCodigo")]
[Table("PEROL_ROLES")]
[Index("PedepCodigo", Name = "PR_PEDEP_PEROL_FK")]
public partial class PerolRole
{
    [Key]
    [Column("PEDEP_CODIGO")]
    [StringLength(3)]
    [Unicode(false)]
    public string PedepCodigo { get; set; } = null!;

    [Key]
    [Column("PEROL_CODIGO")]
    [StringLength(3)]
    [Unicode(false)]
    public string PerolCodigo { get; set; } = null!;

    [Column("PEROL_DESCRI")]
    [StringLength(50)]
    [Unicode(false)]
    public string PerolDescri { get; set; } = null!;

    [ForeignKey("PedepCodigo")]
    [InverseProperty("PerolRoles")]
    public virtual PedepDepar PedepCodigoNavigation { get; set; } = null!;

    [InverseProperty("PerolRole")]
    public virtual ICollection<PeempEmple> PeempEmples { get; set; } = new List<PeempEmple>();
}
