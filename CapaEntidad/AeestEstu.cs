using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar los estudiantes
/// </summary>
[Table("AEEST_ESTU")]
[Index("AecarCodigo", Name = "AR_AECAR_AEEST_FK")]
[Index("PeperCodigo", Name = "PR_PEPER_AEEST_FK")]
public partial class AeestEstu
{
    [Key]
    [Column("AEEST_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AeestCodigo { get; set; } = null!;

    [Column("AECAR_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string? AecarCodigo { get; set; }

    [Column("PEPER_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string PeperCodigo { get; set; } = null!;

    [InverseProperty("AeestCodigoNavigation")]
    public virtual ICollection<AamatMatr> AamatMatrs { get; set; } = new List<AamatMatr>();

    [ForeignKey("AecarCodigo")]
    [InverseProperty("AeestEstus")]
    public virtual AecarCarr? AecarCodigoNavigation { get; set; }

    [InverseProperty("AeestCodigoNavigation")]
    public virtual ICollection<AenotNotum> AenotNota { get; set; } = new List<AenotNotum>();

    [InverseProperty("AeestCodigoNavigation")]
    public virtual ICollection<FecafCabfac> FecafCabfacs { get; set; } = new List<FecafCabfac>();

    [ForeignKey("PeperCodigo")]
    [InverseProperty("AeestEstus")]
    public virtual PeperPer PeperCodigoNavigation { get; set; } = null!;
}
