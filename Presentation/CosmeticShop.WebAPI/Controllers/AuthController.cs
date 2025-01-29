using AutoMapper;
using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Services;
using CosmeticShop.WebAPI.Filters;
using HttpModels.Requests;
using HttpModels.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExceptionHandlingFilter]
    public class AuthController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly UserService _userService;
        private readonly JwtTokenService _jwtTokenService;
        private readonly ITokenGenerationService _tokenGenerationService;
        private readonly IMapper _mapper;

        public AuthController(CustomerService customerService, 
                              UserService userService, 
                              JwtTokenService jwtTokenService,
                              ITokenGenerationService tokenGenerationService,
                              IMapper mapper)
        {
            _customerService = customerService;
            _userService = userService;
            _jwtTokenService = jwtTokenService;
            _tokenGenerationService = tokenGenerationService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> RegisterCustomer([FromBody] CustomerRegisterRequestDto customerRegisterRequestDto, 
                                                                                      CancellationToken cancellationToken)
        {
            var registeredCustomer = await _customerService.Register(customerRegisterRequestDto.FirstName,
                                            customerRegisterRequestDto.LastName,
                                            customerRegisterRequestDto.Email,
                                            customerRegisterRequestDto.Password,
                                            customerRegisterRequestDto.PhoneNumber,
                                            customerRegisterRequestDto.ShippingAddress,
                                            cancellationToken);

            var tokenDto = _tokenGenerationService.GenerateToken(registeredCustomer.Id, cancellationToken);

            var newJwtToken = new JwtToken(tokenDto.token, tokenDto.jti, tokenDto.UserId, tokenDto.Expiration);
            await _jwtTokenService.AddTokenAsync(newJwtToken, cancellationToken);

            var responseDto = _mapper.Map<CustomerResponseDto>(registeredCustomer);
            responseDto.Token = newJwtToken.Token;

            return Ok(responseDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            var role = await DetermineUserRoleAsync(loginRequest.Email, cancellationToken);

            if(role == "Customer")
            {
                var logingCustomer = await _customerService.Login(loginRequest.Email, loginRequest.Password, cancellationToken);
                var tokenDto = _tokenGenerationService.GenerateToken(logingCustomer.Id, cancellationToken);

                var newJwtToken = new JwtToken(tokenDto.token, tokenDto.jti, tokenDto.UserId, tokenDto.Expiration);
                await _jwtTokenService.AddTokenAsync(newJwtToken, cancellationToken);

                var responseDto = _mapper.Map<CustomerResponseDto>(logingCustomer);
                responseDto.Token = newJwtToken.Token;

                return Ok(responseDto);
            }
            else if(role == "User")
            {
                var logingUser = await _userService.Login(loginRequest.Email, loginRequest.Password, cancellationToken);
                var tokenDto = _tokenGenerationService.GenerateToken(logingUser.Id, cancellationToken, logingUser.Role);

                var newJwtToken = new JwtToken(tokenDto.token, tokenDto.jti, tokenDto.UserId, tokenDto.Expiration);
                await _jwtTokenService.AddTokenAsync(newJwtToken, cancellationToken);

                var responseDto = _mapper.Map<UserResponseDto>(logingUser);
                responseDto.Token = newJwtToken.Token;

                return Ok(responseDto);
            }
            else
            {
                return Conflict(new ErrorResponse("Аккаунт с таким Email не найден!"));
            }

        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest logoutRequest, CancellationToken cancellationToken)
        {
            // Предполагается, что в запросе передается токен, который нужно аннулировать
            var token = logoutRequest.Token;

            var jti = _tokenGenerationService.ExtractJtiFromToken(token, cancellationToken);

            // Удаляем токен из хранилища
            var result = await _jwtTokenService.RemoveTokenByJtiAsync(jti, cancellationToken);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest(new ErrorResponse("Не удалось выйти из системы. Попробуйте еще раз."));
            }
        }

        private async Task<string> DetermineUserRoleAsync(string email, CancellationToken cancellationToken)
        {
            if (await _customerService.ExistsByEmail(email, cancellationToken))
            {
                return "Customer";
            }
            else if (await _userService.ExistsByEmail(email, cancellationToken))
            {
                return "User";
            }
            return "Unknown";
        }
    }
}
