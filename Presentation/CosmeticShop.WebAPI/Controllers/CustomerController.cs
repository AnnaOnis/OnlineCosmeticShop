using AutoMapper;
using CosmeticShop.Domain.Services;
using HttpModels.Responses;
using HttpModels.Requests;
using HttpModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CosmeticShop.WebAPI.Filters;
using Microsoft.AspNetCore.Authorization;

namespace CosmeticShop.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ExceptionHandlingFilter]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly IMapper _mapper;
        public CustomerController(CustomerService customerService, IMapper mapper) 
        { 
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet("current")]
        public async Task<ActionResult<CustomerResponseDto>> GetCurrentCustomerProfile(CancellationToken cancellationToken)
        {
            Guid guid = GetCurrentCustomerId();

            var currentCustomer = await _customerService.GetCustomerById(guid, cancellationToken);

            var customerDto = _mapper.Map<CustomerResponseDto>(currentCustomer);

            return Ok(customerDto);
        }

        [HttpPut]
        public async Task<ActionResult<CustomerResponseDto>> UpdateCustomerProfile([FromBody] CustomerUpdateRequestDto customerRequestDto, CancellationToken cancellationToken)
        {
            Guid guid = GetCurrentCustomerId();

            var updatedCustomer = await _customerService.UpdateCustomerProfile(guid,
                                                         customerRequestDto.Email,
                                                         customerRequestDto.FirstName,
                                                         customerRequestDto.LastName,
                                                         customerRequestDto.PhoneNumber,
                                                         customerRequestDto.ShippingAddress,
                                                         cancellationToken);

            var customerDto = _mapper.Map<CustomerResponseDto>(updatedCustomer);

            return Ok(updatedCustomer);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasword ([FromBody] PasswordResetRequestDto passwordResetRequest, CancellationToken cancellationToken)
        {
            Guid guid = GetCurrentCustomerId();

            //if (guid == Guid.Empty)
            //{ 
            //    return Unauthorized(new ErrorResponse("Ошибка авторизации"));
            //}

            await _customerService.ResetPassword(guid, passwordResetRequest.NewPassword, cancellationToken);

            return NoContent();
        }

        private Guid GetCurrentCustomerId()
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(strId == null)
            {
                return Guid.Empty;
            }
            var guid = Guid.Parse(strId);
            return guid;
        }
    }
}
