using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers;

[Route("api/app")]
[ApiController]
public class AppController : Controller
{
    private readonly IHostEnvironment _environment;

    public AppController(IHostEnvironment environment)
    {
        _environment = environment;
    }
    [HttpGet()]
    public App Get()
    {
        return  new App()
        {
            Version = "1.0.0",
            Environment = _environment.EnvironmentName
        };
    }
}

public class App
{
    public string Version { get; set; }
    public string Environment { get; set; }
}
