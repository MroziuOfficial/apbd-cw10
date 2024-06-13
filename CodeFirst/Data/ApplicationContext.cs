using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Data;

public class ApplicationContext : DbContext
{
    protected ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Patient?> Patients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Patient>().HasData(
            new Patient(){IdPatient = 1, FirstName = "Jakub", LastName = "Mrozowski", Birthdate = new DateTime(2003, 11, 21)}
        );
        
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor(){IdDoctor = 1, FirstName = "Marcin", LastName = "Klucznik", Email = "km@onet.pl"}
        );
        
        modelBuilder.Entity<Medicament>().HasData(
            new Medicament(){IdMedicament = 1, Name = "a", Description = "b", Type = "c"}
        );
        
        modelBuilder.Entity<Prescription>().HasData(
            new Prescription(){IdPrescription = 1, Date = new DateTime(2023, 4, 16), DueDate = new DateTime(2024, 6, 13), IdPatient = 1, IdDoctor = 1}
        );

        modelBuilder.Entity<PrescriptionMedicament>().HasData(
            new PrescriptionMedicament(){IdPrescription = 1, IdMedicament = 1,  Dose = 4, Details = "adasdasdasdasd"}

        );
    }
}