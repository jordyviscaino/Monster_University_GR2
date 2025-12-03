using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar los detalles
/// </summary>
[Table("FEDET_DETA")]
[Index("FecafNumfac", Name = "FR_FECAF_FEDET_FK")]
[Index("FecpoCodigo", Name = "FR_FECPO_FEDET_FK")]
public partial class FedetDetum
{
    [Key]
    [Column("FEDET_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string FedetCodigo { get; set; } = null!;

    [Column("FECAF_NUMFAC")]
    [StringLength(6)]
    [Unicode(false)]
    public string FecafNumfac { get; set; } = null!;

    [Column("FECPO_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string FecpoCodigo { get; set; } = null!;

    [Column("FEDET_CANTIDAD")]
    public int FedetCantidad { get; set; }

    [Column("FEDET_TOTAL", TypeName = "decimal(3, 2)")]
    public decimal FedetTotal { get; set; }

    [ForeignKey("FecafNumfac")]
    [InverseProperty("FedetDeta")]
    public virtual FecafCabfac FecafNumfacNavigation { get; set; } = null!;

    [ForeignKey("FecpoCodigo")]
    [InverseProperty("FedetDeta")]
    public virtual FecpoCpag FecpoCodigoNavigation { get; set; } = null!;
}
