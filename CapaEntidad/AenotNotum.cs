using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar las notas de los estudianrtes
/// </summary>
[PrimaryKey("AeestCodigo", "PeempCodigo", "AenotCodigo")]
[Table("AENOT_NOTA")]
[Index("AeestCodigo", Name = "AR_AEEST_AENOT_FK")]
[Index("AeasiCodigo", "AegruCodigo", Name = "AR_AEGRU_AENOT_FK")]
[Index("PeempCodigo", Name = "AR_PEEMP_AENOT_FK")]
public partial class AenotNotum
{
    [Key]
    [Column("AEEST_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AeestCodigo { get; set; } = null!;

    [Key]
    [Column("PEEMP_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string PeempCodigo { get; set; } = null!;

    [Key]
    [Column("AENOT_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AenotCodigo { get; set; } = null!;

    [Column("AEASI_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string? AeasiCodigo { get; set; }

    [Column("AEGRU_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string? AegruCodigo { get; set; }

    [Column("AENOT_NOTA", TypeName = "decimal(4, 2)")]
    public decimal AenotNota { get; set; }

    [Column("AENOT_PARCIAL")]
    public short AenotParcial { get; set; }

    [ForeignKey("AeestCodigo")]
    [InverseProperty("AenotNota")]
    public virtual AeestEstu AeestCodigoNavigation { get; set; } = null!;

    [ForeignKey("AeasiCodigo, AegruCodigo")]
    [InverseProperty("AenotNota")]
    public virtual AegruGrup? AegruGrup { get; set; }

    [ForeignKey("PeempCodigo")]
    [InverseProperty("AenotNota")]
    public virtual PeempEmple PeempCodigoNavigation { get; set; } = null!;
}
