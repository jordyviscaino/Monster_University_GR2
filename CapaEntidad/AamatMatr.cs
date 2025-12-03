using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

[PrimaryKey("AeasiCodigo", "AegruCodigo", "AeestCodigo")]
[Table("AAMAT_MATR")]
[Index("AeestCodigo", Name = "ESTU_MATR_FK")]
[Index("AeasiCodigo", "AegruCodigo", Name = "GRUP_MATR_FK")]
public partial class AamatMatr
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

    [Key]
    [Column("AEEST_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AeestCodigo { get; set; } = null!;

    [Column("AAMAT_FECHA", TypeName = "datetime")]
    public DateTime AamatFecha { get; set; }

    [Column("AAMAT_ESTADO")]
    public bool AamatEstado { get; set; }

    [ForeignKey("AeestCodigo")]
    [InverseProperty("AamatMatrs")]
    public virtual AeestEstu AeestCodigoNavigation { get; set; } = null!;

    [ForeignKey("AeasiCodigo, AegruCodigo")]
    [InverseProperty("AamatMatrs")]
    public virtual AegruGrup AegruGrup { get; set; } = null!;
}
