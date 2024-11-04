using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinkyAppBackend.Api.Models.Entities;
using LinkyAppBackend.Api.Providers;
using LinkyAppBackend.Api.Models.Dtos.Account;
using LinkyAppBackend.Api.Models.Entities.Master;

namespace LinkyAppBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
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