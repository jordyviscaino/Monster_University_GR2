using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar el pago
/// </summary>
[Table("FEPAG_PAGO")]
[Index("FecafNumfac", Name = "FR_FECAF_FEPAG_FK")]
public partial class FepagPago
{
    [Key]
    [Column("FEPAG_COIDGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string FepagCoidgo { get; set; } = null!;

    [Column("FECAF_NUMFAC")]
    [StringLength(6)]
    [Unicode(false)]
    public string FecafNumfac { get; set; } = null!;

    [Column("FEPAG_MONTO", TypeName = "decimal(10, 2)")]
    public decimal FepagMonto { get; set; }

    [Column("FEPAG_FECHAPAGO", TypeName = "datetime")]
    public DateTime FepagFechapago { get; set; }

    [ForeignKey("FecafNumfac")]
    [InverseProperty("FepagPagos")]
    public virtual FecafCabfac FecafNumfacNavigation { get; set; } = null!;

    [InverseProperty("FepagCoidgoNavigation")]
    public virtual ICollection<FefpgFpag> FefpgFpags { get; set; } = new List<FefpgFpag>();
}
