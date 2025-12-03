using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar las carreras
/// </summary>
[Table("AECAR_CARR")]
public partial class AecarCarr
{
    [Key]
    [Column("AECAR_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AecarCodigo { get; set; } = null!;

    [Column("AECAR_NOMBRE")]
    [StringLength(50)]
    [Unicode(false)]
    public string AecarNombre { get; set; } = null!;

    [Column("AECAR_DESCRIPCION")]
    [StringLength(200)]
    [Unicode(false)]
    public string? AecarDescripcion { get; set; }

    [InverseProperty("AecarCodigoNavigation")]
    public virtual ICollection<AanivNvel> AanivNvels { get; set; } = new List<AanivNvel>();

    [InverseProperty("AecarCodigoNavigation")]
    public virtual ICollection<AeestEstu> AeestEstus { get; set; } = new List<AeestEstu>();
}
