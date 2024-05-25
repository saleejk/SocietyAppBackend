﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocietyAppBackend.ModelEntity;
using SocietyAppBackend.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocietyAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserServices _userService;
        public readonly IConfiguration _config;

        public UserController(IUserServices register, IConfiguration config)
        {
            _userService = register;
            _config = config;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userDto)
        {
            try
            {
                var isExist = await _userService.RegisterUser(userDto);
                return Ok(isExist);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"an error occured,{ex.Message}");
            }
        }
        [HttpGet("GetAllUser")]
        public async  Task<IActionResult> GetAllUsers()
        {
            return Ok( await _userService.GetAllUsers());
        }
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await _userService.GetUserById(id));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            try
            {
                var existingUser = await _userService.Login(login);
                if (existingUser == null)
                {
                    return NotFound("username or password incorrect");
                }
                bool validatePassword = BCrypt.Net.BCrypt.Verify(login.Password, existingUser.PasswordHash);
                if (!validatePassword)
                {
                    return BadRequest("password dont match");
                }
                string token = GenerateToken(existingUser);
                return Ok(new { Token = token, email = existingUser.Email, name = existingUser.Username });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        private string GenerateToken(User users)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, users.UserId.ToString()),
            new Claim(ClaimTypes.Name, users.Username),
            new Claim(ClaimTypes.Role, users.Role),
            new Claim(ClaimTypes.Email, users.Email),
        };

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddHours(1)

            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}