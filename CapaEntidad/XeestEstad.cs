using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad utilizada para gestionar el estado de las difetrentes tablas
/// </summary>
[Table("XEEST_ESTAD")]
public partial class XeestEstad
{
    [Key]
    [Column("XEEST_CODIGO")]
    [StringLength(1)]
    [Unicode(false)]
    public string XeestCodigo { get; set; } = null!;

    [Column("XEEST_DESCRI")]
    [StringLength(50)]
    [Unicode(false)]
    public string XeestDescri { get; set; } = null!;

    [InverseProperty("XeestCodigoNavigation")]
    public virtual ICollection<XeusuUsuar> XeusuUsuars { get; set; } = new List<XeusuUsuar>();
}
