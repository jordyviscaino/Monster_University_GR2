using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar los departamentos
/// </summary>
[Table("PEDEP_DEPAR")]
public partial class PedepDepar
{
    [Key]
    [Column("PEDEP_CODIGO")]
    [StringLength(3)]
    [Unicode(false)]
    public string PedepCodigo { get; set; } = null!;

    [Column("PEDEP_DESCRI")]
    [StringLength(50)]
    [Unicode(false)]
    public string PedepDescri { get; set; } = null!;

    [InverseProperty("PedepCodigoNavigation")]
    public virtual ICollection<PerolRole> PerolRoles { get; set; } = new List<PerolRole>();
}
