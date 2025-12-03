using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar los empleados
/// </summary>
[Table("PEEMP_EMPLE")]
[Index("PeperCodigo", Name = "PR_PEPER_PEEMP_FK")]
[Index("PedepCodigo", "PerolCodigo", Name = "PR_PEROL_PEEMP_FK")]
public partial class PeempEmple
{
    [Key]
    [Column("PEEMP_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string PeempCodigo { get; set; } = null!;

    [Column("PEDEP_CODIGO")]
    [StringLength(3)]
    [Unicode(false)]
    public string PedepCodigo { get; set; } = null!;

    [Column("PEROL_CODIGO")]
    [StringLength(3)]
    [Unicode(false)]
    public string PerolCodigo { get; set; } = null!;

    [Column("PEPER_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string PeperCodigo { get; set; } = null!;

    [InverseProperty("PeempCodigoNavigation")]
    public virtual ICollection<AenotNotum> AenotNota { get; set; } = new List<AenotNotum>();

    [InverseProperty("PeempCodigoNavigation")]
    public virtual ICollection<AeperPeri> AeperPeris { get; set; } = new List<AeperPeri>();

    [InverseProperty("PeempCodigoNavigation")]
    public virtual ICollection<FecafCabfac> FecafCabfacs { get; set; } = new List<FecafCabfac>();

    [ForeignKey("PeperCodigo")]
    [InverseProperty("PeempEmples")]
    public virtual PeperPer PeperCodigoNavigation { get; set; } = null!;

    [ForeignKey("PedepCodigo, PerolCodigo")]
    [InverseProperty("PeempEmples")]
    public virtual PerolRole PerolRole { get; set; } = null!;
}
