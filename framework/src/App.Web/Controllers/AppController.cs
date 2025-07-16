using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers;

[Route("/")]
[ApiController]
public class AppController : Controller
{
    private readonly IHostEnvironment _environment;
    private readonly ISuma _suma;
    private readonly ILogger<AppController> _logger;

    public AppController(IHostEnvironment environment, ISuma suma, ILogger<AppController> logger)
    {
        _environment = environment;
        _suma = suma;
        _logger = logger;
    }

    [HttpGet()]
    public App Get()
    {
        _logger.LogInformation("Call to sum : {0}", _suma.Sum());
        return new App() { Version = "1.0.0", Environment = _environment.EnvironmentName };
    }
}

public class App
{
    public string Version { get; set; }
    public string Environment { get; set; }
}
