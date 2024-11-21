

global using jwtWebApi.Models;
global using jwtWebApi.Services.UserService;
global using JwtWebApi.Dto;


global using Microsoft.IdentityModel.Tokens;
//   TokenValidationParameters,
//   var key = new SymmetricSecurityKey(Enconding.UTF8.GetyBytes(_config..)),
//   new SigningCredentials(key, SecurityAlgorithms.HmacSha256),

global using System.IdentityModel.Tokens.Jwt;
//  var handle = new JwtSecurityTokenHandler(), 
//  var token = new JwtSecurityToken(iss, aud,  nbf, exp, clams, signingCredentials )
//  handle.WriteToken(token)


global using System.Security.Claims;
//new Claim(ClaimTypes.Name, user.Username),

global using Microsoft.AspNetCore.Mvc;
//  Api - CotrollerBase, BadRequest, Ok , HttpPostAttribute, ActionResult, FromServices,  HttpPost/Get/Delete/Update/ etc.

global using Microsoft.OpenApi.Models;
//  OpenApiSecurityScheme

global using Swashbuckle.AspNetCore.Filters;
//  SecurityRequirementsOperationFilter

global using System.Text;
//  Encoding
