using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad utilizada para llevar el registro de las opciones que pertenecen a un perfil
/// </summary>
[PrimaryKey("XeopcCodigo", "XeperCodigo", "XeoxpFecasi")]
[Table("XEOXP_OPCPE")]
[Index("XeopcCodigo", Name = "XR_XEOPC_XEOXP_FK")]
[Index("XeperCodigo", Name = "XR_XEPER_XEOXP_FK")]
public partial class XeoxpOpcpe
{
    [Key]
    [Column("XEOPC_CODIGO")]
    [StringLength(3)]
    [Unicode(false)]
    public string XeopcCodigo { get; set; } = null!;

    [Key]
    [Column("XEPER_CODIGO")]
    [StringLength(8)]
    [Unicode(false)]
    public string XeperCodigo { get; set; } = null!;

    [Key]
    [Column("XEOXP_FECASI", TypeName = "datetime")]
    public DateTime XeoxpFecasi { get; set; }

    [Column("XEOXP_FECRET", TypeName = "datetime")]
    public DateTime? XeoxpFecret { get; set; }

    [ForeignKey("XeopcCodigo")]
    [InverseProperty("XeoxpOpcpes")]
    public virtual XeopcOpcio XeopcCodigoNavigation { get; set; } = null!;

    [ForeignKey("XeperCodigo")]
    [InverseProperty("XeoxpOpcpes")]
    public virtual XeperPerfi XeperCodigoNavigation { get; set; } = null!;
}
