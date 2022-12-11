using Microsoft.AspNetCore.Mvc;
using Mpms.Models.Base;

namespace Mpms.Controllers;

[ApiController]
[Route("[controller]")]
public class ExplorerController : ControllerBase
{
    private readonly ILogger<ExplorerController> _logger;

    public ExplorerController(ILogger<ExplorerController> logger)
    {
        _logger = logger;
    }

    public ActionResult<IEnumerable<FileSystemItem>> GetList(string? path)
    {
        return NoContent();
    }
}