// Controllers/AuthController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Claims;
using Cbeua.Core.Helpers;
using Cbeua.Domain.DTO;
using Cbeua.Domain.Entities;
using Cbeua.Domain.Interfaces.IServices;
using CbeuaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Api_BaseController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly IUserService _userLoginLogService;

    public AuthController(IAuthService authService, ILogger<AuthController> logger, IUserService userService)
    {
        _authService = authService;
        _logger = logger;
        _userLoginLogService = userService;
    }

    [HttpPost("login")]
    public async Task<CustomApiResponse> Login([FromBody] LoginRequestDTO request)
    {
        if (!ModelState.IsValid)
        {
            return ApiResponseFactory.Fail("Invalid request data", System.Net.HttpStatusCode.BadRequest);
        }

        var result = await _authService.LoginAsync(request);

        if (!result.IsSucess )
        {
            return ApiResponseFactory.Fail (result.CustomMessage, System.Net.HttpStatusCode.BadRequest);
            
        }
        return ApiResponseFactory.Success(result.Value, result.CustomMessage, System.Net.HttpStatusCode.OK);
              
        

    }

    [HttpPost("register")]
    public async Task<CustomApiResponse> Register([FromBody] RegisterRequestDTO request)
    {
        if (!ModelState.IsValid)
        {
            return ApiResponseFactory.Fail("Invalid request data", System.Net.HttpStatusCode.BadRequest);
        }

        var result = await _authService.RegisterAsync(request);

        if (!result.IsSucess)
        {
            return ApiResponseFactory.Fail(result.CustomMessage, System.Net.HttpStatusCode.BadRequest);
        }

        return ApiResponseFactory.Success(result.Value, result.CustomMessage, System.Net.HttpStatusCode.OK);
        
    }

    [HttpPost("forgot-password")]
    public async Task<CustomApiResponse> ForgotPassword([FromBody] ForgotPasswordRequestDTO request)
    {
        if (!ModelState.IsValid)
        {
            return ApiResponseFactory.Fail("Invalid request data", System.Net.HttpStatusCode.BadRequest);
        }

        var result = await _authService.ForgotPasswordAsync(request.Email);

        if (!result.IsSucess)
        {
            return ApiResponseFactory.Fail(result.CustomMessage, System.Net.HttpStatusCode.BadRequest);
        }

        return ApiResponseFactory.Success(result.Value, result.CustomMessage, System.Net.HttpStatusCode.OK);
    }

    [HttpPost("reset-password")]
    public async Task<CustomApiResponse> ResetPassword([FromBody] ResetPasswordRequestDTO request)
    {
        if (!ModelState.IsValid)
        {
            return ApiResponseFactory.Fail("Invalid request data", System.Net.HttpStatusCode.BadRequest);
        }

        var result = await _authService.ResetPasswordAsync(request);

        if (!result.IsSucess)
        {
            return ApiResponseFactory.Fail(result.CustomMessage, System.Net.HttpStatusCode.BadRequest);
        }

        return ApiResponseFactory.Success(result.Value, result.CustomMessage, System.Net.HttpStatusCode.OK);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<CustomApiResponse> ChangePassword([FromBody] ChangePasswordRequestDTO request)
    {
        if (!ModelState.IsValid)
        {
            return ApiResponseFactory.Fail("Invalid request data", System.Net.HttpStatusCode.BadRequest);
        }

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _authService.ChangePasswordAsync(userId, request);

        if (!result.IsSucess)
        {
            return ApiResponseFactory.Fail(result.CustomMessage, System.Net.HttpStatusCode.BadRequest);
        }

        return ApiResponseFactory.Success(result.Value, result.CustomMessage, System.Net.HttpStatusCode.OK);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<CustomApiResponse> GetCurrentUser()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _authService.GetCurrentUserAsync(userId);

        if (!result.IsSucess)
        {
            return ApiResponseFactory.Fail(result.CustomMessage, System.Net.HttpStatusCode.NotFound);
        }

        return ApiResponseFactory.Success(result.Value, result.CustomMessage, System.Net.HttpStatusCode.OK);
    }

    [HttpPost("logout")]
    [Authorize]
    public CustomApiResponse Logout()
    {
        // For JWT, logout is handled client-side by removing the token
        // You might want to implement token blacklisting for added security
        return ApiResponseFactory.Success(null, "Logged out successfully", System.Net.HttpStatusCode.OK);
    }
}