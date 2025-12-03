using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Contiene la informacion Personal de las personas
/// </summary>
[Table("PEPER_PERS")]
[Index("PeescCodigo", Name = "PR_PEESC_PEPER_FK")]
[Index("PsexCodigo", Name = "PR_PESEX_PEPER_FK")]
public partial class PeperPer
{
    [Key]
    [Column("PEPER_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string PeperCodigo { get; set; } = null!;

    [Column("PSEX_CODIGO")]
    [StringLength(1)]
    [Unicode(false)]
    public string PsexCodigo { get; set; } = null!;

    [Column("PEESC_CODIGO")]
    [StringLength(3)]
    [Unicode(false)]
    public string? PeescCodigo { get; set; }

    [Column("PEPER_NOMBRE")]
    [StringLength(15)]
    [Unicode(false)]
    public string PeperNombre { get; set; } = null!;

    [Column("PEPER_APELLIDO")]
    [StringLength(15)]
    [Unicode(false)]
    public string PeperApellido { get; set; } = null!;

    [Column("PEPER_CEDULA")]
    [StringLength(10)]
    [Unicode(false)]
    public string? PeperCedula { get; set; }

    [Column("PEPER_FECHANACI", TypeName = "datetime")]
    public DateTime PeperFechanaci { get; set; }

    [Column("PEPER_CARGAS", TypeName = "numeric(2, 0)")]
    public decimal PeperCargas { get; set; }

    [Column("PEPER_DIRECCION")]
    [StringLength(100)]
    [Unicode(false)]
    public string PeperDireccion { get; set; } = null!;

    [Column("PEPER_CELULAR")]
    [StringLength(10)]
    [Unicode(false)]
    public string? PeperCelular { get; set; }

    [Column("PEPER_TELDOM")]
    [StringLength(10)]
    [Unicode(false)]
    public string? PeperTeldom { get; set; }

    [Column("PEPER_EMAIL")]
    [StringLength(100)]
    [Unicode(false)]
    public string PeperEmail { get; set; } = null!;

    [InverseProperty("PeperCodigoNavigation")]
    public virtual ICollection<AeestEstu> AeestEstus { get; set; } = new List<AeestEstu>();

    [InverseProperty("PeperCodigoNavigation")]
    public virtual ICollection<PeempEmple> PeempEmples { get; set; } = new List<PeempEmple>();

    [ForeignKey("PeescCodigo")]
    [InverseProperty("PeperPers")]
    public virtual PeescEstciv? PeescCodigoNavigation { get; set; }

    [ForeignKey("PsexCodigo")]
    [InverseProperty("PeperPers")]
    public virtual PsexSexo PsexCodigoNavigation { get; set; } = null!;

    [InverseProperty("PeperCodigoNavigation")]
    public virtual ICollection<XeusuUsuar> XeusuUsuars { get; set; } = new List<XeusuUsuar>();
}
