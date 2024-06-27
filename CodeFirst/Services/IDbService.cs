using CodeFirst.DTOs;
using CodeFirst.Models;

namespace CodeFirst.Services;

public interface IDbService
{
    Task<Patient?> DoesPatientExist(int id);
    Task VerifyMedicamentsExist(List<MedicamentDTO> medicaments);

    Task RegisterUser(User user);
    Task<User> GetUser(string login);
    Task SetUserToken(User u, string token);
    Task<User> GetUserByToken(string token);
}