using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Models;

[Table("prescription_medicament")]
[PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
public class PrescriptionMedicament
{
    public int IdPrescription { get; set; }

    [ForeignKey(nameof(IdPrescription))] public Prescription Prescription { get; set; } = null!;
    
    public int IdMedicament { get; set; }

    [ForeignKey(nameof(IdMedicament))] public Medicament Medicament { get; set; } = null!;

    public int Dose { get; set; }

    [Required] [MaxLength(100)] public string Details { get; set; } = null!;
}