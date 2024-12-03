﻿using AutoMapper;
using CosmeticShop.Domain.Services;
using HttpModels.Responses;
using HttpModels.Requests;
using HttpModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CosmeticShop.Domain.Entities;
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
        public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetCustomers([FromQuery] FilterDto filterDto, CancellationToken cancellationToken)
        {
            var customers = await _customerService.GetAllCustomers(cancellationToken, 
                                                                   filterDto.Filter, 
                                                                   filterDto.SortField, 
                                                                   filterDto.SortOrder ? "asc" : "desc", 
                                                                   filterDto.PageNumber, 
                                                                   filterDto.PageSize);

            var customerDtos = _mapper.Map<CustomerResponseDto[]>(customers);

            return Ok(customerDtos);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponseDto>> GetCustomerById(Guid id, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetCustomerById(id, cancellationToken);
            var customerDto = _mapper.Map<CustomerResponseDto>(customer);
            return Ok(customerDto);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id, CancellationToken cancellationToken)
        {
            await _customerService.DeleteCustomer(id, cancellationToken);
            return NoContent();
        }

        //Методы для самого пользователя

        [HttpGet("current")]
        public async Task<ActionResult<CustomerResponseDto>> GetCurrentCustomerProfile(CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var guid = Guid.Parse(strId);

            var currentCustomer = await _customerService.GetCustomerById(guid, cancellationToken);

            var customerDto = _mapper.Map<CustomerResponseDto>(currentCustomer);

            return Ok(customerDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerResponseDto>> UpdateCustomerProfile(Guid id, CustomerUpdateRequestDto customerRequestDto, CancellationToken cancellationToken)
        {
            var updatedCustomer = await _customerService.UpdateCustomerProfile(id,
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
        public async Task<IActionResult> ResetPasword (string newPassword, CancellationToken cancellationToken)
        {
            var strId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var guid = Guid.Parse(strId);

            await _customerService.ResetPassword(guid, newPassword, cancellationToken);

            return NoContent();
        }
    }
}
