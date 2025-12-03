using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Monster_University_GR2.CapaEntidad;

namespace Monster_University_GR2.CapaDatos;

public partial class MonsterContext : DbContext
{
    public MonsterContext()
    {
    }

    public MonsterContext(DbContextOptions<MonsterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AamatMatr> AamatMatrs { get; set; }

    public virtual DbSet<AanivNvel> AanivNvels { get; set; }

    public virtual DbSet<AeareArea> AeareAreas { get; set; }

    public virtual DbSet<AeasiAsig> AeasiAsigs { get; set; }

    public virtual DbSet<AeaulAula> AeaulAulas { get; set; }

    public virtual DbSet<AecarCarr> AecarCarrs { get; set; }

    public virtual DbSet<AeestEstu> AeestEstus { get; set; }

    public virtual DbSet<AegruGrup> AegruGrups { get; set; }

    public virtual DbSet<AehorHora> AehorHoras { get; set; }

    public virtual DbSet<AenotNotum> AenotNota { get; set; }

    public virtual DbSet<AeperPeri> AeperPeris { get; set; }

    public virtual DbSet<FebecBeca> FebecBecas { get; set; }

    public virtual DbSet<FecafCabfac> FecafCabfacs { get; set; }

    public virtual DbSet<FecpoCpag> FecpoCpags { get; set; }

    public virtual DbSet<FedetDetum> FedetDeta { get; set; }

    public virtual DbSet<FefpgFpag> FefpgFpags { get; set; }

    public virtual DbSet<FepagPago> FepagPagos { get; set; }

    public virtual DbSet<PedepDepar> PedepDepars { get; set; }

    public virtual DbSet<PeempEmple> PeempEmples { get; set; }

    public virtual DbSet<PeescEstciv> PeescEstcivs { get; set; }

    public virtual DbSet<PeperPer> PeperPers { get; set; }

    public virtual DbSet<PerolRole> PerolRoles { get; set; }

    public virtual DbSet<PsexSexo> PsexSexos { get; set; }

    public virtual DbSet<XeestEstad> XeestEstads { get; set; }

    public virtual DbSet<XeopcOpcio> XeopcOpcios { get; set; }

    public virtual DbSet<XeoxpOpcpe> XeoxpOpcpes { get; set; }

    public virtual DbSet<XeperPerfi> XeperPerfis { get; set; }

    public virtual DbSet<XesisSiste> XesisSistes { get; set; }

    public virtual DbSet<XeusuUsuar> XeusuUsuars { get; set; }

    public virtual DbSet<XeuxpUsupe> XeuxpUsupes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Usa el nombre de tu cadena en appsettings
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("CadenaSQL"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AamatMatr>(entity =>
        {
            entity.Property(e => e.AeasiCodigo).IsFixedLength();
            entity.Property(e => e.AegruCodigo).IsFixedLength();
            entity.Property(e => e.AeestCodigo).IsFixedLength();

            entity.HasOne(d => d.AeestCodigoNavigation).WithMany(p => p.AamatMatrs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AAMAT_MA_ESTU_MATR_AEEST_ES");

            entity.HasOne(d => d.AegruGrup).WithMany(p => p.AamatMatrs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AAMAT_MA_GRUP_MATR_AEGRU_GR");
        });

        modelBuilder.Entity<AanivNvel>(entity =>
        {
            entity.Property(e => e.AecarCodigo).IsFixedLength();
            entity.Property(e => e.AeasiCodigo).IsFixedLength();

            entity.HasOne(d => d.AeasiCodigoNavigation).WithMany(p => p.AanivNvels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AANIV_NV_ASIG_NVEL_AEASI_AS");

            entity.HasOne(d => d.AecarCodigoNavigation).WithMany(p => p.AanivNvels)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AANIV_NV_CARR_NVEL_AECAR_CA");
        });

        modelBuilder.Entity<AeareArea>(entity =>
        {
            entity.ToTable("AEARE_AREA", tb => tb.HasComment("Entidad que se utiliza para almacenar las Areas"));

            entity.Property(e => e.AeareCodigo).IsFixedLength();
        });

        modelBuilder.Entity<AeasiAsig>(entity =>
        {
            entity.ToTable("AEASI_ASIG", tb => tb.HasComment("Entidad que se utiliza para almacenar las asignaturas"));

            entity.Property(e => e.AeasiCodigo).IsFixedLength();
            entity.Property(e => e.AeareCodigo).IsFixedLength();

            entity.HasOne(d => d.AeareCodigoNavigation).WithMany(p => p.AeasiAsigs).HasConstraintName("FK_AEASI_AS_AR_AEARE__AEARE_AR");

            entity.HasMany(d => d.AeaAeasiCodigos).WithMany(p => p.AeasiCodigos)
                .UsingEntity<Dictionary<string, object>>(
                    "AareqReqi",
                    r => r.HasOne<AeasiAsig>().WithMany()
                        .HasForeignKey("AeaAeasiCodigo")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AAREQ_RE_ASIG_REQI_AEASI_AS"),
                    l => l.HasOne<AeasiAsig>().WithMany()
                        .HasForeignKey("AeasiCodigo")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AAREQ_RE_REQI_ASIG_AEASI_AS"),
                    j =>
                    {
                        j.HasKey("AeasiCodigo", "AeaAeasiCodigo");
                        j.ToTable("AAREQ_REQI");
                        j.HasIndex(new[] { "AeaAeasiCodigo" }, "ASIG_REQI_FK");
                        j.HasIndex(new[] { "AeasiCodigo" }, "REQI_ASIG_FK");
                        j.IndexerProperty<string>("AeasiCodigo")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("AEASI_CODIGO");
                        j.IndexerProperty<string>("AeaAeasiCodigo")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("AEA_AEASI_CODIGO");
                    });

            entity.HasMany(d => d.AeasiCodigos).WithMany(p => p.AeaAeasiCodigos)
                .UsingEntity<Dictionary<string, object>>(
                    "AareqReqi",
                    r => r.HasOne<AeasiAsig>().WithMany()
                        .HasForeignKey("AeasiCodigo")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AAREQ_RE_REQI_ASIG_AEASI_AS"),
                    l => l.HasOne<AeasiAsig>().WithMany()
                        .HasForeignKey("AeaAeasiCodigo")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AAREQ_RE_ASIG_REQI_AEASI_AS"),
                    j =>
                    {
                        j.HasKey("AeasiCodigo", "AeaAeasiCodigo");
                        j.ToTable("AAREQ_REQI");
                        j.HasIndex(new[] { "AeaAeasiCodigo" }, "ASIG_REQI_FK");
                        j.HasIndex(new[] { "AeasiCodigo" }, "REQI_ASIG_FK");
                        j.IndexerProperty<string>("AeasiCodigo")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("AEASI_CODIGO");
                        j.IndexerProperty<string>("AeaAeasiCodigo")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("AEA_AEASI_CODIGO");
                    });
        });

        modelBuilder.Entity<AeaulAula>(entity =>
        {
            entity.ToTable("AEAUL_AULA", tb => tb.HasComment("Entidad que se utiliza para almacenar las aulas"));

            entity.Property(e => e.AeaulCodigo).IsFixedLength();
        });

        modelBuilder.Entity<AecarCarr>(entity =>
        {
            entity.ToTable("AECAR_CARR", tb => tb.HasComment("Entidad que se utiliza para almacenar las carreras"));

            entity.Property(e => e.AecarCodigo).IsFixedLength();
        });

        modelBuilder.Entity<AeestEstu>(entity =>
        {
            entity.ToTable("AEEST_ESTU", tb => tb.HasComment("Entidad que se utiliza para almacenar los estudiantes"));

            entity.Property(e => e.AeestCodigo).IsFixedLength();
            entity.Property(e => e.AecarCodigo).IsFixedLength();
            entity.Property(e => e.PeperCodigo).IsFixedLength();

            entity.HasOne(d => d.AecarCodigoNavigation).WithMany(p => p.AeestEstus).HasConstraintName("FK_AEEST_ES_AR_AECAR__AECAR_CA");

            entity.HasOne(d => d.PeperCodigoNavigation).WithMany(p => p.AeestEstus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AEEST_ES_PR_PEPER__PEPER_PE");
        });

        modelBuilder.Entity<AegruGrup>(entity =>
        {
            entity.ToTable("AEGRU_GRUP", tb => tb.HasComment("Entidad que se utiliza para almacenar los grupos"));

            entity.Property(e => e.AeasiCodigo).IsFixedLength();
            entity.Property(e => e.AegruCodigo).IsFixedLength();
            entity.Property(e => e.AeaulCodigo).IsFixedLength();
            entity.Property(e => e.AehorCodigo).IsFixedLength();
            entity.Property(e => e.AeperCodigoperi).IsFixedLength();

            entity.HasOne(d => d.AeasiCodigoNavigation).WithMany(p => p.AegruGrups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AEGRU_GR_AR_AEASI__AEASI_AS");

            entity.HasOne(d => d.AeaulCodigoNavigation).WithMany(p => p.AegruGrups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AEGRU_GR_AR_AEAUL__AEAUL_AU");

            entity.HasOne(d => d.AehorCodigoNavigation).WithMany(p => p.AegruGrups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AEGRU_GR_AR_AEHOR__AEHOR_HO");

            entity.HasOne(d => d.AeperCodigoperiNavigation).WithMany(p => p.AegruGrups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AEGRU_GR_AR_AEPER__AEPER_PE");
        });

        modelBuilder.Entity<AehorHora>(entity =>
        {
            entity.ToTable("AEHOR_HORA", tb => tb.HasComment("Entidad que se utiliza para almacenar los grupos"));

            entity.Property(e => e.AehorCodigo).IsFixedLength();
        });

        modelBuilder.Entity<AenotNotum>(entity =>
        {
            entity.ToTable("AENOT_NOTA", tb => tb.HasComment("Entidad que se utiliza para almacenar las notas de los estudianrtes"));

            entity.Property(e => e.AeestCodigo).IsFixedLength();
            entity.Property(e => e.PeempCodigo).IsFixedLength();
            entity.Property(e => e.AenotCodigo).IsFixedLength();
            entity.Property(e => e.AeasiCodigo).IsFixedLength();
            entity.Property(e => e.AegruCodigo).IsFixedLength();

            entity.HasOne(d => d.AeestCodigoNavigation).WithMany(p => p.AenotNota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AENOT_NO_AR_AEEST__AEEST_ES");

            entity.HasOne(d => d.PeempCodigoNavigation).WithMany(p => p.AenotNota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AENOT_NO_AR_PEEMP__PEEMP_EM");

            entity.HasOne(d => d.AegruGrup).WithMany(p => p.AenotNota).HasConstraintName("FK_AENOT_NO_AR_AEGRU__AEGRU_GR");
        });

        modelBuilder.Entity<AeperPeri>(entity =>
        {
            entity.ToTable("AEPER_PERI", tb => tb.HasComment("Entidad que se utiliza para almacenar los periodos academicos"));

            entity.Property(e => e.AeperCodigoperi).IsFixedLength();
            entity.Property(e => e.PeempCodigo).IsFixedLength();

            entity.HasOne(d => d.PeempCodigoNavigation).WithMany(p => p.AeperPeris).HasConstraintName("FK_AEPER_PE_AR_PEEMP__PEEMP_EM");
        });

        modelBuilder.Entity<FebecBeca>(entity =>
        {
            entity.ToTable("FEBEC_BECA", tb => tb.HasComment("Entidad que se utiliza para almacenar las becas"));

            entity.Property(e => e.FebecCodigobeca).IsFixedLength();
        });

        modelBuilder.Entity<FecafCabfac>(entity =>
        {
            entity.ToTable("FECAF_CABFAC", tb => tb.HasComment("Entidad utilizada para la gesti?n de la cabecera de la FACTURA"));

            entity.Property(e => e.FecafNumfac).IsFixedLength();
            entity.Property(e => e.AeestCodigo).IsFixedLength();
            entity.Property(e => e.FebecCodigobeca).IsFixedLength();
            entity.Property(e => e.PeempCodigo).IsFixedLength();

            entity.HasOne(d => d.AeestCodigoNavigation).WithMany(p => p.FecafCabfacs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FECAF_CA_FR_AEEST__AEEST_ES");

            entity.HasOne(d => d.FebecCodigobecaNavigation).WithMany(p => p.FecafCabfacs).HasConstraintName("FK_FECAF_CA_FR_FEBEC__FEBEC_BE");

            entity.HasOne(d => d.PeempCodigoNavigation).WithMany(p => p.FecafCabfacs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FECAF_CA_FR_PEEMP__PEEMP_EM");
        });

        modelBuilder.Entity<FecpoCpag>(entity =>
        {
            entity.ToTable("FECPO_CPAG", tb => tb.HasComment("Entidad que se utiliza para almacenar las facturas"));

            entity.Property(e => e.FecpoCodigo).IsFixedLength();
        });

        modelBuilder.Entity<FedetDetum>(entity =>
        {
            entity.ToTable("FEDET_DETA", tb => tb.HasComment("Entidad que se utiliza para almacenar los detalles"));

            entity.Property(e => e.FedetCodigo).IsFixedLength();
            entity.Property(e => e.FecafNumfac).IsFixedLength();
            entity.Property(e => e.FecpoCodigo).IsFixedLength();

            entity.HasOne(d => d.FecafNumfacNavigation).WithMany(p => p.FedetDeta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FEDET_DE_FR_FECAF__FECAF_CA");

            entity.HasOne(d => d.FecpoCodigoNavigation).WithMany(p => p.FedetDeta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FEDET_DE_FR_FECPO__FECPO_CP");
        });

        modelBuilder.Entity<FefpgFpag>(entity =>
        {
            entity.ToTable("FEFPG_FPAG", tb => tb.HasComment("Entidad que se utiliza para almacenar las formas de pago"));

            entity.Property(e => e.FefpgCodigo).IsFixedLength();
            entity.Property(e => e.FepagCoidgo).IsFixedLength();

            entity.HasOne(d => d.FepagCoidgoNavigation).WithMany(p => p.FefpgFpags).HasConstraintName("FK_FEFPG_FP_FR_FEPAG__FEPAG_PA");
        });

        modelBuilder.Entity<FepagPago>(entity =>
        {
            entity.ToTable("FEPAG_PAGO", tb => tb.HasComment("Entidad que se utiliza para almacenar el pago"));

            entity.Property(e => e.FepagCoidgo).IsFixedLength();
            entity.Property(e => e.FecafNumfac).IsFixedLength();

            entity.HasOne(d => d.FecafNumfacNavigation).WithMany(p => p.FepagPagos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FEPAG_PA_FR_FECAF__FECAF_CA");
        });

        modelBuilder.Entity<PedepDepar>(entity =>
        {
            entity.ToTable("PEDEP_DEPAR", tb => tb.HasComment("Entidad que se utiliza para almacenar los departamentos"));

            entity.Property(e => e.PedepCodigo).IsFixedLength();
        });

        modelBuilder.Entity<PeempEmple>(entity =>
        {
            entity.ToTable("PEEMP_EMPLE", tb => tb.HasComment("Entidad que se utiliza para almacenar los empleados"));

            entity.Property(e => e.PeempCodigo).IsFixedLength();
            entity.Property(e => e.PedepCodigo).IsFixedLength();
            entity.Property(e => e.PeperCodigo).IsFixedLength();
            entity.Property(e => e.PerolCodigo).IsFixedLength();

            entity.HasOne(d => d.PeperCodigoNavigation).WithMany(p => p.PeempEmples)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PEEMP_EM_PR_PEPER__PEPER_PE");

            entity.HasOne(d => d.PerolRole).WithMany(p => p.PeempEmples)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PEEMP_EM_PR_PEROL__PEROL_RO");
        });

        modelBuilder.Entity<PeescEstciv>(entity =>
        {
            entity.ToTable("PEESC_ESTCIV", tb => tb.HasComment("Entidad que se utiliza para almacenar el estado civil"));

            entity.Property(e => e.PeescCodigo).IsFixedLength();
        });

        modelBuilder.Entity<PeperPer>(entity =>
        {
            entity.ToTable("PEPER_PERS", tb => tb.HasComment("Contiene la informacion Personal de las personas"));

            entity.Property(e => e.PeperCodigo).IsFixedLength();
            entity.Property(e => e.PeescCodigo).IsFixedLength();
            entity.Property(e => e.PeperCedula).IsFixedLength();
            entity.Property(e => e.PeperCelular).IsFixedLength();
            entity.Property(e => e.PeperTeldom).IsFixedLength();
            entity.Property(e => e.PsexCodigo).IsFixedLength();

            entity.HasOne(d => d.PeescCodigoNavigation).WithMany(p => p.PeperPers).HasConstraintName("FK_PEPER_PE_PR_PEESC__PEESC_ES");

            entity.HasOne(d => d.PsexCodigoNavigation).WithMany(p => p.PeperPers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PEPER_PE_PR_PESEX__PSEX_SEX");
        });

        modelBuilder.Entity<PerolRole>(entity =>
        {
            entity.ToTable("PEROL_ROLES", tb => tb.HasComment("Entidad utilizada para representar el CARGO dentro de un DEPARTAMENTO"));

            entity.Property(e => e.PedepCodigo).IsFixedLength();
            entity.Property(e => e.PerolCodigo).IsFixedLength();

            entity.HasOne(d => d.PedepCodigoNavigation).WithMany(p => p.PerolRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PEROL_RO_PR_PEDEP__PEDEP_DE");
        });

        modelBuilder.Entity<PsexSexo>(entity =>
        {
            entity.ToTable("PSEX_SEXO", tb => tb.HasComment("Entidad utilizada para representar el sexo o genero de una empresa"));

            entity.Property(e => e.PsexCodigo).IsFixedLength();
        });

        modelBuilder.Entity<XeestEstad>(entity =>
        {
            entity.ToTable("XEEST_ESTAD", tb => tb.HasComment("Entidad utilizada para gestionar el estado de las difetrentes tablas"));

            entity.Property(e => e.XeestCodigo).IsFixedLength();
        });

        modelBuilder.Entity<XeopcOpcio>(entity =>
        {
            entity.ToTable("XEOPC_OPCIO", tb => tb.HasComment("Entidad utilizada para realizar el registro de las diferentes opciones de un sistema"));

            entity.Property(e => e.XeopcCodigo).IsFixedLength();
            entity.Property(e => e.XesisCodigo).IsFixedLength();

            entity.HasOne(d => d.XesisCodigoNavigation).WithMany(p => p.XeopcOpcios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_XEOPC_OP_XR_XESIS__XESIS_SI");
        });

        modelBuilder.Entity<XeoxpOpcpe>(entity =>
        {
            entity.ToTable("XEOXP_OPCPE", tb => tb.HasComment("Entidad utilizada para llevar el registro de las opciones que pertenecen a un perfil"));

            entity.Property(e => e.XeopcCodigo).IsFixedLength();
            entity.Property(e => e.XeperCodigo).IsFixedLength();

            entity.HasOne(d => d.XeopcCodigoNavigation).WithMany(p => p.XeoxpOpcpes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_XEOXP_OP_XR_XEOPC__XEOPC_OP");

            entity.HasOne(d => d.XeperCodigoNavigation).WithMany(p => p.XeoxpOpcpes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_XEOXP_OP_XR_XEPER__XEPER_PE");
        });

        modelBuilder.Entity<XeperPerfi>(entity =>
        {
            entity.ToTable("XEPER_PERFI", tb => tb.HasComment("Entidad utilizada para realizar la gesti?n de los diferentes perfiles"));

            entity.Property(e => e.XeperCodigo).IsFixedLength();
        });

        modelBuilder.Entity<XesisSiste>(entity =>
        {
            entity.ToTable("XESIS_SISTE", tb => tb.HasComment("Entidad utilizada para realziar la gesti?n de los diferentes subsistemas"));

            entity.Property(e => e.XesisCodigo).IsFixedLength();
        });

        modelBuilder.Entity<XeusuUsuar>(entity =>
        {
            entity.ToTable("XEUSU_USUAR", tb => tb.HasComment("Entidad relacionada para gentionar los usuario que ingrsan al sistema"));

            entity.Property(e => e.PeperCodigo).IsFixedLength();
            entity.Property(e => e.XeestCodigo).IsFixedLength();
            entity.Property(e => e.XeusuFeccre).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.XeusuFecmod).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.PeperCodigoNavigation).WithMany(p => p.XeusuUsuars)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_XEUSU_US_XR_PEPER__PEPER_PE");

            entity.HasOne(d => d.XeestCodigoNavigation).WithMany(p => p.XeusuUsuars)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_XEUSU_US_XR_XEEST__XEEST_ES");
        });

        modelBuilder.Entity<XeuxpUsupe>(entity =>
        {
            entity.ToTable("XEUXP_USUPE", tb => tb.HasComment("Entidad utilizada para realizar el registro de los diferentes usuarios que pertenecen a un perfil"));

            entity.Property(e => e.XeperCodigo).IsFixedLength();

            entity.HasOne(d => d.XeperCodigoNavigation).WithMany(p => p.XeuxpUsupes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_XEUXP_US_XR_XEPER__XEPER_PE");

            entity.HasOne(d => d.XeusuLoginNavigation).WithMany(p => p.XeuxpUsupes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_XEUXP_US_XR_XEUSU__XEUSU_US");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
