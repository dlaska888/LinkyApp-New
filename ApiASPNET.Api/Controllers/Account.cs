using System.Security.Claims;
using ApiASPNET.Api.Models.Dtos.Account;
using ApiASPNET.Api.Models.Entities.Master;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiASPNET.Api.Models.Entities;
using ApiASPNET.Api.Providers;

namespace ApiASPNET.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class Account(UserManager<AppUser> userManager, IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<GetAccountDto> Get()
    {
        var user = await userManager.Users
            .FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        return mapper.Map<GetAccountDto>(user!);
    }
}