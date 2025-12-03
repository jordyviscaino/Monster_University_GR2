using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad utilizada para realziar la gesti?n de los diferentes subsistemas
/// </summary>
[Table("XESIS_SISTE")]
public partial class XesisSiste
{
    [Key]
    [Column("XESIS_CODIGO")]
    [StringLength(1)]
    [Unicode(false)]
    public string XesisCodigo { get; set; } = null!;

    [Column("XESIS_DESCRI")]
    [StringLength(50)]
    [Unicode(false)]
    public string XesisDescri { get; set; } = null!;

    [InverseProperty("XesisCodigoNavigation")]
    public virtual ICollection<XeopcOpcio> XeopcOpcios { get; set; } = new List<XeopcOpcio>();
}
