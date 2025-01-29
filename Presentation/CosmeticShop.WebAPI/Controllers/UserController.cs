using AutoMapper;
using CosmeticShop.Domain.Services;
using CosmeticShop.WebAPI.Filters;
using HttpModels;
using HttpModels.Requests;
using HttpModels.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CosmeticShop.WebAPI.Controllers
{
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

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers([FromQuery] FilterDto filterDto, CancellationToken cancellationToken)
        {
            var (users, totalUsers) = await _userService.GetAllSortedUsers(cancellationToken,
                                                             filterDto.Filter,
                                                             filterDto.SortField,
                                                             filterDto.SortOrder ? "asc" : "desc",
                                                             filterDto.PageNumber,
                                                             filterDto.PageSize);

            var responseDtos = _mapper.Map<UserResponseDto[]>(users);
            var response = new PagedResponse<UserResponseDto> {Items = responseDtos, TotalItems = totalUsers };
            return Ok(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserInfo([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserInfo(id, cancellationToken);
            var responseDto = _mapper.Map<UserResponseDto>(user);
            return Ok(responseDto);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _userService.DeleteUser(id, cancellationToken);

            return NoContent(); 
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("add_user")]
        public async Task<IActionResult> AddNewUser([FromBody] UserAddRequestDto userRequestDto, CancellationToken cancellationToken)
        {
            await _userService.AddUser(userRequestDto.Email,
                                       userRequestDto.Password,
                                       userRequestDto.FirstName,
                                       userRequestDto.LastName,
                                       userRequestDto.Role,
                                       cancellationToken);

            return NoContent();
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
