using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar las formas de pago
/// </summary>
[Table("FEFPG_FPAG")]
[Index("FepagCoidgo", Name = "FR_FEPAG_FEFPG_FK")]
public partial class FefpgFpag
{
    [Key]
    [Column("FEFPG_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string FefpgCodigo { get; set; } = null!;

    [Column("FEPAG_COIDGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string? FepagCoidgo { get; set; }

    [Column("FEFPG_NOMBRE")]
    [StringLength(50)]
    [Unicode(false)]
    public string FefpgNombre { get; set; } = null!;

    [ForeignKey("FepagCoidgo")]
    [InverseProperty("FefpgFpags")]
    public virtual FepagPago? FepagCoidgoNavigation { get; set; }
}
