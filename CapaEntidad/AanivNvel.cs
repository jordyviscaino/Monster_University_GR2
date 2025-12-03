using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

[PrimaryKey("AecarCodigo", "AeasiCodigo")]
[Table("AANIV_NVEL")]
[Index("AeasiCodigo", Name = "ASIG_NVEL_FK")]
[Index("AecarCodigo", Name = "CARR_NVEL_FK")]
public partial class AanivNvel
{
    [Key]
    [Column("AECAR_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AecarCodigo { get; set; } = null!;

    [Key]
    [Column("AEASI_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AeasiCodigo { get; set; } = null!;

    [Column("AANIV_NUMERO")]
    public int AanivNumero { get; set; }

    [Column("AANIV_CREDITOS")]
    public int AanivCreditos { get; set; }

    [ForeignKey("AeasiCodigo")]
    [InverseProperty("AanivNvels")]
    public virtual AeasiAsig AeasiCodigoNavigation { get; set; } = null!;

    [ForeignKey("AecarCodigo")]
    [InverseProperty("AanivNvels")]
    public virtual AecarCarr AecarCodigoNavigation { get; set; } = null!;
}
