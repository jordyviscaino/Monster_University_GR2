using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Monster_University_GR2.CapaEntidad;

/// <summary>
/// Entidad que se utiliza para almacenar los grupos
/// </summary>
[Table("AEHOR_HORA")]
public partial class AehorHora
{
    [Key]
    [Column("AEHOR_CODIGO")]
    [StringLength(10)]
    [Unicode(false)]
    public string AehorCodigo { get; set; } = null!;

    [Column("AEHOR_DIASEMANA")]
    [StringLength(25)]
    [Unicode(false)]
    public string AehorDiasemana { get; set; } = null!;

    [Column("AEHOR_HORAINICIO", TypeName = "datetime")]
    public DateTime AehorHorainicio { get; set; }

    [Column("AEHOR_HORAFIN", TypeName = "datetime")]
    public DateTime AehorHorafin { get; set; }

    [InverseProperty("AehorCodigoNavigation")]
    public virtual ICollection<AegruGrup> AegruGrups { get; set; } = new List<AegruGrup>();
}
