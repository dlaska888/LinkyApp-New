using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace ApiASPNET.Api.Sieve;

public class AppSieveProcessor(IOptions<SieveOptions> options) : SieveProcessor(options);