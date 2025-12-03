using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad utilizada para realizar la gesti?n de los diferentes perfiles
/// </summary>
[Table("XEPER_PERFI")]
public partial class XeperPerfi
{
    [Key]
    [Column("XEPER_CODIGO")]
    [StringLength(8)]
    [Unicode(false)]
    public string XeperCodigo { get; set; } = null!;

    [Column("XEPER_DESCRI")]
    [StringLength(100)]
    [Unicode(false)]
    public string XeperDescri { get; set; } = null!;

    [Column("XEPER_OBSER", TypeName = "text")]
    public string? XeperObser { get; set; }

    [InverseProperty("XeperCodigoNavigation")]
    public virtual ICollection<XeoxpOpcpe> XeoxpOpcpes { get; set; } = new List<XeoxpOpcpe>();

    [InverseProperty("XeperCodigoNavigation")]
    public virtual ICollection<XeuxpUsupe> XeuxpUsupes { get; set; } = new List<XeuxpUsupe>();
}
