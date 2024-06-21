using CodeFirst.Data;
using CodeFirst.DTOs;
using CodeFirst.Models;

namespace CodeFirst.Services;

public class DbService : IDbService
{
    public ApplicationContext Context;

    public DbService(ApplicationContext context)
    {
        Context = context;
    }

    public async Task<Patient?> DoesPatientExist(int id)
    {
        return await Context.Patients.FindAsync(id);
    }

    public async Task VerifyMedicamentsExist(List<MedicamentDTO> medicaments)
    {
        foreach (MedicamentDTO m in medicaments)
        {
            if (await Context.Medicaments.FindAsync(m.IdMedicament) == null)
            {
                throw new Exception();
            }
        }
    }
}