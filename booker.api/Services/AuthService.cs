using booker.api.Models;
using booker.api.Models.DTOs;
using booker.api.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace booker.api.Services
{
    public class AuthService: IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ApiResponse> RegisterAsync(RegisterRequestDTO registerRequestDTO)
        {
            // Validate role exists
            if (!string.IsNullOrEmpty(registerRequestDTO.Role))
            {
                var roleExists = await _roleManager.RoleExistsAsync(registerRequestDTO.Role);
                if (!roleExists)
                {
                    return new ApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { $"Role '{registerRequestDTO.Role}' does not exist" }
                    };
                }
            }

            var user = new ApplicationUser
            {
                UserName = registerRequestDTO.UserName,
                Email = registerRequestDTO.Email,
            };

            var result = await _userManager.CreateAsync(user, registerRequestDTO.Password);
            if (!result.Succeeded)
            {
                return new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = result.Errors.Select(e => e.Description).ToList()
                };
            }

            // Add role to user if specified
            if (!string.IsNullOrEmpty(registerRequestDTO.Role))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, registerRequestDTO.Role);
                if (!roleResult.Succeeded)
                {
                    return new ApiResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccess = false,
                        ErrorMessages = roleResult.Errors.Select(e => e.Description).ToList()
                    };
                }
            }

            return new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = "User registered successfully"
            };
        }

        public async Task<ApiResponse> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            // Find user by username
            var user = await _userManager.FindByNameAsync(loginRequestDTO.UserName);
            if (user == null)
            {
                return new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid credentials" }
                };
            }

            // Check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequestDTO.Password, false);
            if (!result.Succeeded)
            {
                return new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    ErrorMessages = new List<string> { "Invalid credentials" }
                };
            }

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Generate JWT token
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            var loginResponse = new LoginResponseDTO
            {
                Token = token,
                User = new UserInfo
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    Roles = roles.ToList()
                }
            };

            return new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = loginResponse
            };
        }
    }
}
