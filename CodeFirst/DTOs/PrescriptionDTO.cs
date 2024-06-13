namespace CodeFirst.DTOs;

public class PrescriptionDTO
{
    public PatientDTO Patient { get; set; }
    public List<MedicamentDTO> Medicaments { get; set; } = new List<MedicamentDTO>();
    public DateTime DateTime { get; set; }
    public DateTime DueDate { get; set; }
}