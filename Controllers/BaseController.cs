using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Samo.DataAccess;
using Samo.Interfaces;
using Samo.Models;

namespace Samo.Controllers;

public class BaseController(SamoDbContext context) : ControllerBase
{
    protected readonly SamoDbContext _context = context;
}