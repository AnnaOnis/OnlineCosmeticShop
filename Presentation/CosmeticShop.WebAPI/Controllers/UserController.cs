using AutoMapper;
using CosmeticShop.Domain.Services;
using CosmeticShop.WebAPI.Filters;
using HttpModels;
using HttpModels.Requests;
using HttpModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CosmeticShop.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ExceptionHandlingFilter]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UserController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("current")]
        public async Task<ActionResult<UserResponseDto>> GetCurrentUser(CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var guid = Guid.Parse(strId);

            var user = await _userService.GetUserInfo(guid, cancellationToken);
            var responseDto = _mapper.Map<UserResponseDto>(user);
            return Ok(responseDto);
        }

        [HttpPut]
        public async Task<ActionResult<UserResponseDto>> UpdateUserProfile([FromBody] UserUpdateRequestDto userRequestDto, CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var guid = Guid.Parse(strId);

            var updatedUser = await _userService.UpdateUser(guid,
                                                            userRequestDto.Email,
                                                            userRequestDto.FirstName,
                                                            userRequestDto.LastName,
                                                            cancellationToken);

            var responseDto = _mapper.Map<UserResponseDto>(updatedUser);
            return Ok(responseDto);
        }

        [HttpPost("reset_password")]
        public async Task<IActionResult> ResetPasword([FromBody] PasswordResetRequestDto passwordResetRequest, CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var guid = Guid.Parse(strId);

            await _userService.ResetPassword(guid, passwordResetRequest.NewPassword, cancellationToken);
            return NoContent();
        }
    }
}
