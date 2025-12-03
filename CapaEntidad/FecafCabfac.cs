using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad utilizada para la gesti?n de la cabecera de la FACTURA
/// </summary>
[Table("FECAF_CABFAC")]
[Index("AeestCodigo", Name = "FR_AEEST_FECAF_FK")]
[Index("FebecCodigobeca", Name = "FR_FEBEC_FECAF_FK")]
[Index("PeempCodigo", Name = "FR_PEEMP_FECAF_FK")]
public partial class FecafCabfac
{
    [Key]
    [Column("FECAF_NUMFAC")]
    [StringLength(6)]
    [Unicode(false)]
    public string FecafNumfac { get; set; } = null!;

    [Column("FEBEC_CODIGOBECA")]
    [StringLength(10)]
    [Unicode(false)]
    public string? FebecCodigobeca { get; set; }

    [Column("PEEMP_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string PeempCodigo { get; set; } = null!;

    [Column("AEEST_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AeestCodigo { get; set; } = null!;

    [Column("FECAF_FECEMI", TypeName = "datetime")]
    public DateTime FecafFecemi { get; set; }

    [Column("FECAF_AUTSRI")]
    [StringLength(50)]
    [Unicode(false)]
    public string? FecafAutsri { get; set; }

    [Column("FECAF_FEASRI", TypeName = "datetime")]
    public DateTime? FecafFeasri { get; set; }

    [Column("FECAF_IVA", TypeName = "numeric(10, 2)")]
    public decimal FecafIva { get; set; }

    [Column("FECAF_DESCUES", TypeName = "numeric(10, 2)")]
    public decimal? FecafDescues { get; set; }

    [Column("FECAF_TOTPAG", TypeName = "numeric(10, 2)")]
    public decimal FecafTotpag { get; set; }

    [Column("FECAF_SUBTOT", TypeName = "numeric(10, 2)")]
    public decimal FecafSubtot { get; set; }

    [Column("FECAF_ESTADO")]
    [StringLength(25)]
    [Unicode(false)]
    public string FecafEstado { get; set; } = null!;

    [ForeignKey("AeestCodigo")]
    [InverseProperty("FecafCabfacs")]
    public virtual AeestEstu AeestCodigoNavigation { get; set; } = null!;

    [ForeignKey("FebecCodigobeca")]
    [InverseProperty("FecafCabfacs")]
    public virtual FebecBeca? FebecCodigobecaNavigation { get; set; }

    [InverseProperty("FecafNumfacNavigation")]
    public virtual ICollection<FedetDetum> FedetDeta { get; set; } = new List<FedetDetum>();

    [InverseProperty("FecafNumfacNavigation")]
    public virtual ICollection<FepagPago> FepagPagos { get; set; } = new List<FepagPago>();

    [ForeignKey("PeempCodigo")]
    [InverseProperty("FecafCabfacs")]
    public virtual PeempEmple PeempCodigoNavigation { get; set; } = null!;
}
