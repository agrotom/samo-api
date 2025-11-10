using Microsoft.AspNetCore.Mvc;
using Samo.DataAccess;

namespace Samo.Controllers;

[ApiController]
[Route("[controller]")]
public class DiaryController(SamoDbContext dbContext) : ControllerBase
{

    private readonly SamoDbContext _dbContext = dbContext;

    [HttpPost]
    public async Task<IActionResult> UpdateInsight([FromBody] UpdateInsightRequest request, CancellationToken ct)
    {
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetInsight()
    {
        return Ok();
    }
}

public class UpdateInsightRequest
{
}