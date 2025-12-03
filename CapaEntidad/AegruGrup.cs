using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar los grupos
/// </summary>
[PrimaryKey("AeasiCodigo", "AegruCodigo")]
[Table("AEGRU_GRUP")]
[Index("AeasiCodigo", Name = "AR_AEASI_AEGRU_FK")]
[Index("AeaulCodigo", Name = "AR_AEAUL_AEGRU_FK")]
[Index("AehorCodigo", Name = "AR_AEHOR_AEGRU_FK")]
[Index("AeperCodigoperi", Name = "AR_AEPER_AEGRU_FK")]
public partial class AegruGrup
{
    [Key]
    [Column("AEASI_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AeasiCodigo { get; set; } = null!;

    [Key]
    [Column("AEGRU_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AegruCodigo { get; set; } = null!;

    [Column("AEAUL_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AeaulCodigo { get; set; } = null!;

    [Column("AEHOR_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AehorCodigo { get; set; } = null!;

    [Column("AEPER_CODIGOPERI")]
    [StringLength(10)]
    [Unicode(false)]
    public string AeperCodigoperi { get; set; } = null!;

    [Column("AEGRU_CUPOMAX")]
    public int AegruCupomax { get; set; }

    [InverseProperty("AegruGrup")]
    public virtual ICollection<AamatMatr> AamatMatrs { get; set; } = new List<AamatMatr>();

    [ForeignKey("AeasiCodigo")]
    [InverseProperty("AegruGrups")]
    public virtual AeasiAsig AeasiCodigoNavigation { get; set; } = null!;

    [ForeignKey("AeaulCodigo")]
    [InverseProperty("AegruGrups")]
    public virtual AeaulAula AeaulCodigoNavigation { get; set; } = null!;

    [ForeignKey("AehorCodigo")]
    [InverseProperty("AegruGrups")]
    public virtual AehorHora AehorCodigoNavigation { get; set; } = null!;

    [InverseProperty("AegruGrup")]
    public virtual ICollection<AenotNotum> AenotNota { get; set; } = new List<AenotNotum>();

    [ForeignKey("AeperCodigoperi")]
    [InverseProperty("AegruGrups")]
    public virtual AeperPeri AeperCodigoperiNavigation { get; set; } = null!;
}
