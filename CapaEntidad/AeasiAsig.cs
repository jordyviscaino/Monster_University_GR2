using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar las asignaturas
/// </summary>
[Table("AEASI_ASIG")]
[Index("AeareCodigo", Name = "AR_AEARE_AEGRU_FK")]
public partial class AeasiAsig
{
    [Key]
    [Column("AEASI_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AeasiCodigo { get; set; } = null!;

    [Column("AEARE_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string? AeareCodigo { get; set; }

    [Column("AEASI_NOMBREASIG")]
    [StringLength(50)]
    [Unicode(false)]
    public string AeasiNombreasig { get; set; } = null!;

    [InverseProperty("AeasiCodigoNavigation")]
    public virtual ICollection<AanivNvel> AanivNvels { get; set; } = new List<AanivNvel>();

    [ForeignKey("AeareCodigo")]
    [InverseProperty("AeasiAsigs")]
    public virtual AeareArea? AeareCodigoNavigation { get; set; }

    [InverseProperty("AeasiCodigoNavigation")]
    public virtual ICollection<AegruGrup> AegruGrups { get; set; } = new List<AegruGrup>();

    [ForeignKey("AeasiCodigo")]
    [InverseProperty("AeasiCodigos")]
    public virtual ICollection<AeasiAsig> AeaAeasiCodigos { get; set; } = new List<AeasiAsig>();

    [ForeignKey("AeaAeasiCodigo")]
    [InverseProperty("AeaAeasiCodigos")]
    public virtual ICollection<AeasiAsig> AeasiCodigos { get; set; } = new List<AeasiAsig>();
}
