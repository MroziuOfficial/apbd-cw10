using CodeFirst.Data;
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
    public async Task
}