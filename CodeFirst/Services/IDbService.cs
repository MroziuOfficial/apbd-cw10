using CodeFirst.DTOs;
using CodeFirst.Models;

namespace CodeFirst.Services;

public interface IDbService
{
    Task<Patient?> DoesPatientExist(int id);
    Task VerifyMedicamentsExist(List<MedicamentDTO> medicaments);
}