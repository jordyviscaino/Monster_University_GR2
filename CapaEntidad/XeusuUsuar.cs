using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad relacionada para gestionar los usuarios que ingresan al sistema
/// </summary>
[Table("XEUSU_USUAR")]
[Index("PeperCodigo", Name = "XR_PEPER_XEUSU_FK")]
[Index("XeestCodigo", Name = "XR_XEEST_XEUSU_FK")]
public partial class XeusuUsuar
{
    [Key]
    [Column("XEUSU_LOGIN")]
    [StringLength(50)]
    [Unicode(false)]
    public string XeusuLogin { get; set; } = null!;

    [Column("XEUSU_PASWD")]
    [StringLength(255)]
    [Unicode(false)]
    public string XeusuPaswd { get; set; } = null!;

    [Column("XEEST_CODIGO")]
    [StringLength(1)]
    [Unicode(false)]
    public string XeestCodigo { get; set; } = null!;

    [Column("PEPER_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string PeperCodigo { get; set; } = null!;

    [Column("XEUSU_FECCRE", TypeName = "datetime")]
    public DateTime XeusuFeccre { get; set; }

    [Column("XEUSU_FECMOD", TypeName = "datetime")]
    public DateTime XeusuFecmod { get; set; }

    [Column("XEUSU_PIEFIR")]
    [StringLength(100)]
    [Unicode(false)]
    public string XeusuPiefir { get; set; } = null!;

    // Veo que ya tenías los tokens listos, excelente.
    [Column("XEUSU_TOKEN_REC")]
    [StringLength(100)]
    [Unicode(false)]
    public string? XeusuTokenRec { get; set; }

    [Column("XEUSU_FEC_EXP_TOK", TypeName = "datetime")]
    public DateTime? XeusuFecExpTok { get; set; }

    [Column("XEUSU_CAMBIAR_PWD")]
    [StringLength(1)]
    [Unicode(false)]
    // Inicializar con "N" para que nunca sea null por defecto
    public string XeusuCambiarPwd { get; set; } = "N";

    [ForeignKey("PeperCodigo")]
    [InverseProperty("XeusuUsuars")]
    public virtual PeperPer PeperCodigoNavigation { get; set; } = null!;

    [ForeignKey("XeestCodigo")]
    [InverseProperty("XeusuUsuars")]
    public virtual XeestEstad XeestCodigoNavigation { get; set; } = null!;

    [InverseProperty("XeusuLoginNavigation")]
    public virtual ICollection<XeuxpUsupe> XeuxpUsupes { get; set; } = new List<XeuxpUsupe>();
}