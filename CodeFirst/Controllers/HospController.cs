using System.Transactions;
using CodeFirst.Data;
using CodeFirst.DTOs;
using CodeFirst.Models;
using CodeFirst.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HospController : ControllerBase
{
    private readonly IDbService _dbService;
    private readonly ApplicationContext _context;

    public HospController(IDbService dbService, ApplicationContext context)
    {
        _dbService = dbService;
        _context = context;
    }

    [HttpPost("prescription")]
    public async Task<IActionResult> CreatePrescription([FromBody] PrescriptionDTO prescriptionDto)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.IdPatient == prescriptionDto.Patient.IdPatient);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = prescriptionDto.Patient.FirstName,
                LastName = prescriptionDto.Patient.LastName,
                Birthdate = prescriptionDto.Patient.Birthdate
            };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        try
        {
            await _dbService.VerifyMedicamentsExist(prescriptionDto.Medicaments);
        }
        catch (Exception)
        {
            return NotFound("One or more medicaments do not exist.");
        }

        if (prescriptionDto.Medicaments.Count > 10)
        {
            return BadRequest("Prescription cannot contain more than 10 medicaments.");
        }

        if (prescriptionDto.DueDate < prescriptionDto.Date)
        {
            return BadRequest("Due date cannot be earlier than the prescription date.");
        }

        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var newPrescription = new Prescription
            {
                Date = prescriptionDto.Date,
                DueDate = prescriptionDto.DueDate,
                IdDoctor = 1,
                IdPatient = patient.IdPatient
            };

            _context.Prescriptions.Add(newPrescription);
            await _context.SaveChangesAsync();

            foreach (var medicament in prescriptionDto.Medicaments)
            {
                _context.PrescriptionMedicaments.Add(new PrescriptionMedicament
                {
                    IdMedicament = medicament.IdMedicament,
                    IdPrescription = newPrescription.IdPrescription,
                    Details = medicament.Description,
                    Dose = medicament.Dose
                });
            }

            await _context.SaveChangesAsync();
            transactionScope.Complete();
        }

        return Ok("Prescription successfully created.");
    }
    
    [HttpGet]
    [Route("patient/{id:int}")]
    public async Task<IActionResult> GetPatient(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        if (patient == null)
        {
            return NotFound("Patient doesn't exist");
        }

        var patientDto = new 
        {
            patient.IdPatient,
            patient.FirstName,
            patient.LastName,
            patient.Birthdate,
            Prescriptions = patient.Prescriptions.Select(p => new 
            {
                p.IdPrescription,
                p.Date,
                p.DueDate,
                Doctor = new
                {
                    p.Doctor.IdDoctor,
                    p.Doctor.FirstName,
                    p.Doctor.LastName
                },
                PrescriptionMedicaments = p.PrescriptionMedicaments.Select(pm => new 
                {
                    pm.IdMedicament,
                    pm.Dose,
                    pm.Details,
                    Medicament = new
                    {
                        pm.Medicament.Name,
                        pm.Medicament.Description,
                        pm.Medicament.Type
                    }
                }).ToList()
            }).OrderBy(p => p.DueDate).ToList()
        };

        return Ok(patientDto);
    }
}
