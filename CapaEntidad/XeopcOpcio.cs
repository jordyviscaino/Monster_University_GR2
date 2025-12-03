using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad utilizada para realizar el registro de las diferentes opciones de un sistema
/// </summary>
[Table("XEOPC_OPCIO")]
[Index("XesisCodigo", Name = "XR_XESIS_XEOPC_FK")]
public partial class XeopcOpcio
{
    [Key]
    [Column("XEOPC_CODIGO")]
    [StringLength(3)]
    [Unicode(false)]
    public string XeopcCodigo { get; set; } = null!;

    [Column("XESIS_CODIGO")]
    [StringLength(1)]
    [Unicode(false)]
    public string XesisCodigo { get; set; } = null!;

    [Column("XEOPC_DESCRI")]
    [StringLength(100)]
    [Unicode(false)]
    public string XeopcDescri { get; set; } = null!;

    [InverseProperty("XeopcCodigoNavigation")]
    public virtual ICollection<XeoxpOpcpe> XeoxpOpcpes { get; set; } = new List<XeoxpOpcpe>();

    [ForeignKey("XesisCodigo")]
    [InverseProperty("XeopcOpcios")]
    public virtual XesisSiste XesisCodigoNavigation { get; set; } = null!;
}
