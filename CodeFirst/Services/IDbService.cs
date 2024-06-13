using CodeFirst.DTOs;
using CodeFirst.Models;

namespace CodeFirst.Services;

public interface IDbService
{
    Task<Patient?> DoesPatientExist(int id);
    Task DoesMedicamentExist(List<MedicamentDTO> medicaments);
}