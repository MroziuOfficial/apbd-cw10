using CodeFirst.Data;
using CodeFirst.DTOs;
using CodeFirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers;

[ApiController]
public class HospController : ControllerBase
{
    public IDbService Service;
    public ApplicationContext Context;

    public HospController(IDbService s, ApplicationContext c)
    {
        Service = s;
        Context = c;
    }

    [HttpPost]
    [Route("prescription")]
    public async Task<IActionResult> AddPrescription(PrescriptionDTO prescriptionDto)
    {
        var patient = await Service.DoesPatientExist(prescriptionDto.Patient.IdPatient);
        return Ok("Added the prescription");
    }
}