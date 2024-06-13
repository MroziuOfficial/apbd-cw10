using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirst.Models;

[Table("prescription")]
public class Prescription
{
    [Key]
    public int IdPrescription { get; set; }

    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public DateTime DueDate { get; set; }

   [Required]
    public int IdPatient { get; set; }

    [ForeignKey(nameof(IdPatient))] public Patient Patient { get; set; } = null!;

    [Required]
    public int IdDoctor { get; set; }

    [ForeignKey(nameof(IdDoctor))] public Doctor Doctor { get; set; } = null!;

    public ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}