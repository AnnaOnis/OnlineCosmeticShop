using AutoMapper;
using CosmeticShop.Domain.Services;
using HttpModels.Responses;
using HttpModels.Requests;
using HttpModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CosmeticShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly IMapper _mapper;
        public CustomerController(CustomerService customerService, IMapper mapper) 
        { 
            _customerService = customerService;
            _mapper = mapper;
        }

        //Методы для администратора

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<PagedResponse<CustomerResponseDto>>> GetCustomers([FromQuery] FilterDto filterDto, CancellationToken cancellationToken)
        {
            var (customers, totalCustomers) = await _customerService.GetAllCustomers(cancellationToken, 
                                                                   filterDto.Filter, 
                                                                   filterDto.SortField, 
                                                                   filterDto.SortOrder ? "asc" : "desc", 
                                                                   filterDto.PageNumber, 
                                                                   filterDto.PageSize);

            var customerDtos = _mapper.Map<CustomerResponseDto[]>(customers);

            var response = new PagedResponse<CustomerResponseDto> { Items = customerDtos, TotalItems = totalCustomers };

            return Ok(response);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDto>> GetCustomerById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetCustomerById(id, cancellationToken);
            var customerDto = _mapper.Map<CustomerResponseDto>(customer);
            return Ok(customerDto);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _customerService.DeleteCustomer(id, cancellationToken);
            return NoContent();
        }

        //Методы для самого пользователя

        //[Authorize]
        [HttpGet("current")]
        public async Task<ActionResult<CustomerResponseDto>> GetCurrentCustomerProfile(CancellationToken cancellationToken)
        {
            Guid guid = GetCurrentCustomerId();

            var currentCustomer = await _customerService.GetCustomerById(guid, cancellationToken);

            var customerDto = _mapper.Map<CustomerResponseDto>(currentCustomer);

            return Ok(customerDto);
        }

        //[Authorize]
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

        //[Authorize]
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
