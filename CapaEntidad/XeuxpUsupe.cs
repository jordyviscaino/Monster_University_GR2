using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad utilizada para realizar el registro de los diferentes usuarios que pertenecen a un perfil
/// </summary>
[PrimaryKey("XeusuLogin", "XeperCodigo", "XeuxpFecasi")]
[Table("XEUXP_USUPE")]
[Index("XeperCodigo", Name = "XR_XEPER_XEUXP_FK")]
public partial class XeuxpUsupe
{
    [Key]
    [Column("XEUSU_LOGIN")]
    [StringLength(50)]
    [Unicode(false)]
    public string XeusuLogin { get; set; } = null!;

    [Key]
    [Column("XEPER_CODIGO")]
    [StringLength(8)]
    [Unicode(false)]
    public string XeperCodigo { get; set; } = null!;

    [Key]
    [Column("XEUXP_FECASI", TypeName = "datetime")]
    public DateTime XeuxpFecasi { get; set; }

    [Column("XEUXP_FECRET", TypeName = "datetime")]
    public DateTime? XeuxpFecret { get; set; }

    [ForeignKey("XeperCodigo")]
    [InverseProperty("XeuxpUsupes")]
    public virtual XeperPerfi XeperCodigoNavigation { get; set; } = null!;

    [ForeignKey("XeusuLogin")]
    [InverseProperty("XeuxpUsupes")]
    public virtual XeusuUsuar XeusuLoginNavigation { get; set; } = null!;
}
