
using BackEndRecursosHumanosFilasur.Models.ContentResponse;
using Microsoft.EntityFrameworkCore;

namespace BackEndRecursosHumanosFilasur.Data;

public class BaseContext : DbContext
{
    public BaseContext(DbContextOptions<BaseContext> options) : base(options) { }


    #region Adelanto de sueldo
    public DbSet<AdvancedSalaryRequestEntity> AdvancedSalaryRequestEntity => Set<AdvancedSalaryRequestEntity>();
    public DbSet<LimitAdvanceResponse> LimitAdvanceResponse => Set<LimitAdvanceResponse>();
    #endregion
    
    #region Solicitud de Prestamo
    public DbSet<LoanRequestEntity> LoanRequestEntity => Set<LoanRequestEntity>();
    public DbSet<LoanAmountsEntity> LoanAmountsEntities => Set<LoanAmountsEntity>();
    // public DbSet<LoanRequestFilentity> LoanRequestFileEntity => Set<LoanRequestFilentity>();
    public DbSet<LoanEmployeeResponse> LoanEmployeeResponse => Set<LoanEmployeeResponse>();
    public DbSet<RequestValidationState> RequestValidationState => Set<RequestValidationState>();
    //public DbSet<LoanRequestSuportEntity> LoanRequestSuportEntities => Set<LoanRequestSuportEntity>();

    public DbSet<SalarayAdvanceEmployeeAccount> SalarayAdvanceEmployeeAccount => Set<SalarayAdvanceEmployeeAccount>();
    public DbSet<LoanLimitResponse> LoanLimitResponse => Set<LoanLimitResponse>();
    public DbSet<LoanAmountsEntity> LoanAmountsEntity => Set<LoanAmountsEntity>();
    public DbSet<LoanReasonEntity> LoanReasonEntity => Set<LoanReasonEntity>();
    public DbSet<LoanCreatedRequestResponse> LoanCreatedRequestResponse => Set<LoanCreatedRequestResponse>();
    #endregion
    
    #region Archivos del Prestamo
    public DbSet<LoanFileEntity> LoanFileEntity => Set<LoanFileEntity>();
    public DbSet<LoanFileResponse> LoanFileResponse => Set<LoanFileResponse>();
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Table Transaction
        modelBuilder.Entity<AdvancedSalaryRequestEntity>().ToTable("xtus_rrhh_adelanto_sueldo","filasur").HasKey(x => x.id_adelanto_sueldo);
        modelBuilder.Entity<LoanRequestEntity>().ToTable("xtus_rrhh_prestamo", "filasur").HasKey(x => x.id_prestamo);
        modelBuilder.Entity<LoanAmountsEntity>().ToTable("xtus_rrhh_prestamo_montos", "filasur").HasKey(x => x.id_prestamo_montos);
        // modelBuilder.Entity<LoanRequestFilentity>().ToTable("xtus_rrhh_prestamo_archivos", "inka").HasKey(x => x.id_archivos);
        modelBuilder.Entity<LoanReasonEntity>().ToTable("xtus_rrhh_motivos_solicitudes", "filasur").HasKey(x => x.id_motivo);
        modelBuilder.Entity<LoanFileEntity>().ToTable("xtus_rrhh_prestamo_archivos", "filasur").HasKey(x => x.id_archivos);
        //modelBuilder.Entity<LoanRequestSuportEntity>().ToTable("xtus_rrhh_prestamo_sustento", "inka").HasKey(x => x.id_prestamo);
        #endregion
        
        #region Store Procedure Query Select
        // modelBuilder.Entity<PeriodResponse>().HasNoKey();
        // modelBuilder.Entity<DetailPeriodResponse>().HasNoKey();
        modelBuilder.Entity<LoanEmployeeResponse>().HasNoKey();
        modelBuilder.Entity<SalarayAdvanceEmployeeAccount>().HasNoKey();
        modelBuilder.Entity<LoanLimitResponse>().HasNoKey();
        modelBuilder.Entity<LoanCreatedRequestResponse>().HasNoKey();
        modelBuilder.Entity<LimitAdvanceResponse>().HasNoKey();
        modelBuilder.Entity<LoanFileResponse>().HasNoKey();
        modelBuilder.Entity<RequestValidationState>().HasNoKey();
        #endregion
    }
}