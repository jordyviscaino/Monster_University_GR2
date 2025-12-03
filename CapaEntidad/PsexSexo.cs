using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad utilizada para representar el sexo o genero de una empresa
/// </summary>
[Table("PSEX_SEXO")]
public partial class PsexSexo
{
    [Key]
    [Column("PSEX_CODIGO")]
    [StringLength(1)]
    [Unicode(false)]
    public string PsexCodigo { get; set; } = null!;

    [Column("PSEX_DESCRI")]
    [StringLength(50)]
    [Unicode(false)]
    public string PsexDescri { get; set; } = null!;

    [InverseProperty("PsexCodigoNavigation")]
    public virtual ICollection<PeperPer> PeperPers { get; set; } = new List<PeperPer>();
}
