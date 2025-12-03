using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar los periodos academicos
/// </summary>
[Table("AEPER_PERI")]
[Index("PeempCodigo", Name = "AR_PEEMP_AEPER_FK")]
public partial class AeperPeri
{
    [Key]
    [Column("AEPER_CODIGOPERI")]
    [StringLength(10)]
    [Unicode(false)]
    public string AeperCodigoperi { get; set; } = null!;

    [Column("PEEMP_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string? PeempCodigo { get; set; }

    [Column("AEPER_NOMBREPERI")]
    [StringLength(20)]
    [Unicode(false)]
    public string AeperNombreperi { get; set; } = null!;

    [Column("AEPER_FECHAINICIO", TypeName = "datetime")]
    public DateTime AeperFechainicio { get; set; }

    [Column("AEPER_FECHAFIN", TypeName = "datetime")]
    public DateTime AeperFechafin { get; set; }

    [Column("AEPER_ESTADO")]
    public bool AeperEstado { get; set; }

    [InverseProperty("AeperCodigoperiNavigation")]
    public virtual ICollection<AegruGrup> AegruGrups { get; set; } = new List<AegruGrup>();

    [ForeignKey("PeempCodigo")]
    [InverseProperty("AeperPeris")]
    public virtual PeempEmple? PeempCodigoNavigation { get; set; }
}
