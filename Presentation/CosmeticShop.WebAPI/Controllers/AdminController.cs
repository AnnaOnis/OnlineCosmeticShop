using System.Security.Claims;
using AutoMapper;
using CosmeticShop.Domain.Entities;
using CosmeticShop.Domain.Services;
using CosmeticShop.WebAPI.Filters;
using HttpModels;
using HttpModels.Requests;
using HttpModels.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    [ExceptionHandlingFilter]
    public class AdminController : ControllerBase
    {
        private readonly CategoryService _categoryService;
        private readonly CustomerService _customerService;
        private readonly OrderService _orderService;
        private readonly ReviewService _reviewService;
        private readonly ProductService _productService;
        private readonly UserService _userService;
        private readonly PaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;

        public AdminController(CategoryService categoryService,
                               CustomerService customerService,
                               OrderService orderService,
                               ReviewService reviewService,
                               ProductService productService,
                               UserService userService,
                               PaymentService paymentService,
                               IMapper mapper,
                               ILogger<AdminController> logger)
        {
            _categoryService = categoryService;
            _customerService = customerService;
            _orderService = orderService;
            _reviewService = reviewService;
            _productService = productService;
            _userService = userService;
            _paymentService = paymentService;
            _mapper = mapper;
            _logger = logger;
        }

        //Работа с товарами

        [HttpPost("product/create")]
        public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody] ProductRequestDto productRequestDto, CancellationToken cancellationToken)
        {
            Product newProduct = _mapper.Map<Product>(productRequestDto);

            if (newProduct == null) return BadRequest();

            await _productService.AddProductAsync(newProduct, cancellationToken);
            ProductResponseDto responseDto = _mapper.Map<ProductResponseDto>(newProduct);
            return Ok(responseDto);
        }

        [HttpPut("product/{id}")]
        public async Task<ActionResult<ProductResponseDto>> UpdateProduct([FromRoute] Guid id, [FromBody] ProductRequestDto productRequestDto, CancellationToken cancellationToken)
        {
            var updatingProduct = await _productService.GetProductByIdAsync(id, cancellationToken);

            updatingProduct.Name = productRequestDto.Name;
            updatingProduct.Price = productRequestDto.Price;
            updatingProduct.Description = productRequestDto.Description;
            updatingProduct.Manufacturer = productRequestDto.Manufacturer;
            updatingProduct.CategoryId = productRequestDto.CategoryId;
            updatingProduct.Rating = productRequestDto.Rating;
            updatingProduct.StockQuantity = productRequestDto.StockQuantity;
            updatingProduct.ImageUrl = productRequestDto.ImageUrl;

            await _productService.UpdateProductAsync(updatingProduct, cancellationToken);

            ProductResponseDto responseDto = _mapper.Map<ProductResponseDto>(updatingProduct);
            return Ok(responseDto);
        }

        [HttpDelete("product/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _productService.DeleteProductAsync(id, cancellationToken);
            return NoContent();
        }

        //Работа с котегориями товаров

        [HttpPut("category/{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] CategoryRequestDto categoryRequestDto, CancellationToken cancellationToken)
        {
            var updatingCategory = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);

            string newName = categoryRequestDto.CategoryName;
            var parrentId = categoryRequestDto.ParentCategoryId;

            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, newName, cancellationToken, parrentId);
            var responseDto = _mapper.Map<CategoryResponseDto>(updatedCategory);

            return Ok(responseDto);
        }

        [HttpDelete("category/{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _categoryService.DeleteCategoryAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPost("category")]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CategoryRequestDto categoryRequestDto, CancellationToken cancellationToken)
        {
            string name = categoryRequestDto.CategoryName;
            var parentId = categoryRequestDto.ParentCategoryId;

            var category = await _categoryService.AddCategoryAsync(name, cancellationToken, parentId is not null ? parentId : null);
            var responseDto = _mapper.Map<CategoryResponseDto>(category);
            return Ok(responseDto);
        }

        //Работа с заказами и платежами

        [HttpGet("orders")]
        public async Task<ActionResult<PagedResponse<OrderResponseDto>>> GetOrders([FromQuery] FilterDto filterDto, CancellationToken cancellationToken)
        {
            var (orders, totalOrders) = await _orderService.GetAllOrdersAsync(cancellationToken,
                                                               filterDto.Filter,
                                                               filterDto.SortField,
                                                               filterDto.SortOrder ? "asc" : "desc",
                                                               filterDto.PageNumber,
                                                               filterDto.PageSize);

            var responseDtos = _mapper.Map<OrderResponseDto[]>(orders);

            foreach (var order in responseDtos)
            {
                var payment = await _paymentService.GetPaymentByOrderId(order.Id, cancellationToken);
                order.OrderPaymentStatus = payment.Status;
            }

            
            var response = new PagedResponse<OrderResponseDto> { Items = responseDtos, TotalItems = totalOrders };

            return Ok(response);
        }

        [HttpPut("order/status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] OrderUpdateRequestDto orderRequestDto, CancellationToken cancellationToken)
        {
            await _orderService.UpdateOrderStatusAsync(orderRequestDto.Id, orderRequestDto.NewStatus, cancellationToken);

            return NoContent();
        }

        [HttpPut("payment/status")]
        public async Task<IActionResult> UpdutePaymentSattus([FromBody] PaymentUpdateStatusRequestDto requestDto, CancellationToken cancellationToken)
        {
            var payment = await _paymentService.GetPaymentByOrderId(requestDto.OrderId, cancellationToken);
            await _paymentService.UpdatePaymentStatusAsync(payment.Id, requestDto.NewPaymentStatus, cancellationToken);
            return NoContent();
        }

        [HttpDelete("order/{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _orderService.DeleteOrder(id, cancellationToken);
            return NoContent();
        }

        //Работа с отзывами

        [HttpGet("reviews")]
        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetAllNotApprovedReviews(CancellationToken cancellationToken)
        {
            var reviews = await _reviewService.GetAllNotApprovedReviewsAsync(cancellationToken);
            var reviewDtos = _mapper.Map<ReviewResponseDto[]>(reviews);
            return Ok(reviewDtos);
        }

        [HttpDelete("review/{id}")]
        public async Task<IActionResult> DeleteReview([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _reviewService.DeleteReviewAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPut("review/{id}/approve")]
        public async Task<IActionResult> ApproveReview([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _reviewService.ApproveReviewAsync(id, cancellationToken);
            return NoContent();
        }

        //Работа с клиентами магазина

        [HttpGet("customers")]
        public async Task<ActionResult<PagedResponse<CustomerResponseDto>>> GetCustomers([FromQuery] FilterDto filterDto, CancellationToken cancellationToken)
        {
            var (customers, totalCustomers) = await _customerService.GetAllCustomersSorted(cancellationToken,
                                                                   filterDto.Filter,
                                                                   filterDto.SortField,
                                                                   filterDto.SortOrder ? "asc" : "desc",
                                                                   filterDto.PageNumber,
                                                                   filterDto.PageSize);

            var customerDtos = _mapper.Map<CustomerResponseDto[]>(customers);

            var response = new PagedResponse<CustomerResponseDto> { Items = customerDtos, TotalItems = totalCustomers };

            return Ok(response);
        }

        [HttpGet("customer/{id}")]
        public async Task<ActionResult<CustomerResponseDto>> GetCustomerById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetCustomerById(id, cancellationToken);
            var customerDto = _mapper.Map<CustomerResponseDto>(customer);
            return Ok(customerDto);
        }

        [HttpDelete("customer/{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _customerService.DeleteCustomer(id, cancellationToken);
            return NoContent();
        }

        //Работа с пользователями

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAllUsers([FromQuery] FilterDto filterDto, CancellationToken cancellationToken)
        {
            var (users, totalUsers) = await _userService.GetAllSortedUsers(cancellationToken,
                                                             filterDto.Filter,
                                                             filterDto.SortField,
                                                             filterDto.SortOrder ? "asc" : "desc",
                                                             filterDto.PageNumber,
                                                             filterDto.PageSize);

            var responseDtos = _mapper.Map<UserResponseDto[]>(users);
            var response = new PagedResponse<UserResponseDto> { Items = responseDtos, TotalItems = totalUsers };
            return Ok(response);
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserInfo([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserInfo(id, cancellationToken);
            var responseDto = _mapper.Map<UserResponseDto>(user);
            return Ok(responseDto);
        }

        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            await _userService.DeleteUser(id, cancellationToken);

            return NoContent();
        }

        [HttpPost("add_user")]
        public async Task<IActionResult> AddNewUser([FromBody] UserAddRequestDto userRequestDto, CancellationToken cancellationToken)
        {
            _logger.LogInformation(userRequestDto.Email + "\n" 
                                 + userRequestDto.Password + "\n"
                                 + userRequestDto.FirstName + "\n"
                                 + userRequestDto.LastName + "\n"
                                 + userRequestDto.Role);

            await _userService.AddUser(userRequestDto.Email,
                                       userRequestDto.Password,
                                       userRequestDto.FirstName,
                                       userRequestDto.LastName,
                                       userRequestDto.Role,
                                       cancellationToken);

            return NoContent();
        }

        [HttpPost("update_user/{id}")]
        public async Task<ActionResult<UserResponseDto>> UpdateUserProfile([FromRoute] Guid id, [FromBody] AdminUserUpdateRequestDto userRequestDto, CancellationToken cancellationToken)
        {
            var updatedUser = await _userService.UpdateUserForAdmin(id,
                                                            userRequestDto.NewPassword,
                                                            userRequestDto.Email,
                                                            userRequestDto.FirstName,
                                                            userRequestDto.LastName,
                                                            userRequestDto.UserRole,
                                                            cancellationToken);

            var responseDto = _mapper.Map<UserResponseDto>(updatedUser);
            return Ok(responseDto);
        }

        // Статистика

        [HttpGet("statistic")]
        public async Task<ActionResult<StatisticResponse>> GetStatistic (CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAll(cancellationToken);
            int totalOrders = orders.Count;

            var totalRevenue = orders.Where(o => o.Status == Domain.Enumerations.OrderStatus.Delivered).Sum(o => o.TotalAmount);

            DateTime now = DateTime.Now;
            DateTime firstDay = new DateTime(now.Year, now.Month, 1);
            DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);
            var customers = await _customerService.GetAll(cancellationToken);
            var newCustomers = customers.Select(c => c.DateRegistered >= firstDay && c.DateRegistered <= lastDay).ToList();
            int newCustomerCount = newCustomers.Count();

            var reviews = await _reviewService.GetAllApprovedReviews(cancellationToken);
            var approvedReviewCount = reviews.Count();

            var response = new StatisticResponse
            {
                OrderCount = totalOrders,
                TotalRevenue = totalRevenue,
                NewCustomerCount = newCustomerCount,
                ApprovedReviewCount = approvedReviewCount
            };

            return Ok(response);
        }


    }
}
