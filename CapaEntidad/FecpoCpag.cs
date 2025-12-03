using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar las facturas
/// </summary>
[Table("FECPO_CPAG")]
public partial class FecpoCpag
{
    [Key]
    [Column("FECPO_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string FecpoCodigo { get; set; } = null!;

    [Column("FECPO_NOMBRE")]
    [StringLength(50)]
    [Unicode(false)]
    public string FecpoNombre { get; set; } = null!;

    [Column("FECPO_DESCRIPCION")]
    [StringLength(100)]
    [Unicode(false)]
    public string FecpoDescripcion { get; set; } = null!;

    [Column("FECPO_VALOR", TypeName = "decimal(10, 2)")]
    public decimal FecpoValor { get; set; }

    [InverseProperty("FecpoCodigoNavigation")]
    public virtual ICollection<FedetDetum> FedetDeta { get; set; } = new List<FedetDetum>();
}
