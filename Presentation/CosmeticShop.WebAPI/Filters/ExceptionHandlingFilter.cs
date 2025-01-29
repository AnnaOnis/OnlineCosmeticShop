using HttpModels.Responses;
using Microsoft.AspNetCore.Mvc.Filters;
using CosmeticShop.Domain.Exceptions.Category;
using CosmeticShop.Domain.Exceptions.Order;
using CosmeticShop.Domain.Exceptions.Payment;
using CosmeticShop.Domain.Exceptions.Product;
using CosmeticShop.Domain.Exceptions;
using CosmeticShop.Domain.Exceptions.Review;
using CosmeticShop.Domain.Exceptions.Customer;
using CosmeticShop.Domain.Exceptions.Users;
using Microsoft.AspNetCore.Mvc;

namespace CosmeticShop.WebAPI.Filters
{
    public class ExceptionHandlingFilter : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var response = TryGetErrorResponseFromExeption(context);

            if (response != null)
            {
                context.Result = new ObjectResult(response)
                {
                    StatusCode = response.StatusCode
                };

                context.ExceptionHandled = true;
            }

        }

        public ErrorResponse? TryGetErrorResponseFromExeption(ExceptionContext context)
        {
            return context.Exception switch
            {
                CategoryNotFoundException => new ErrorResponse("Категория не найдена.", StatusCodes.Status404NotFound),
                InvalidCategoryNameException => new ErrorResponse("Такая категория уже существует.", StatusCodes.Status409Conflict),
                OrderNotFoundException => new ErrorResponse("Заказ не найден.", StatusCodes.Status404NotFound),
                InvalidPaymentMethodException => new ErrorResponse("Неверный способ оплаты.", StatusCodes.Status409Conflict),
                PaymentNotFoundException => new ErrorResponse("Платеж не найден.", StatusCodes.Status404NotFound),
                ProductAlreadyInFavoritesException => new ErrorResponse("Товар уже есть в избранном", StatusCodes.Status409Conflict),
                ProductNotFoundException => new ErrorResponse("Товар не найден.", StatusCodes.Status404NotFound),
                ReviewAlreadyApprovedException => new ErrorResponse("Отзыв уже одобрен.", StatusCodes.Status409Conflict),
                ReviewNotFoundException => new ErrorResponse("Отзыв не найден.", StatusCodes.Status404NotFound),
                CustomerNotFoundException => new ErrorResponse("Клиент не найден.", StatusCodes.Status404NotFound),
                EmailAlreadyExistsException => new ErrorResponse("Пользователь с таким email уже зарегистрирован.", StatusCodes.Status409Conflict),
                InvalidPasswordException => new ErrorResponse("Неверный пароль.", StatusCodes.Status409Conflict),
                UserNotFoundException => new ErrorResponse("Пользователь не найден.", StatusCodes.Status404NotFound),
                CartNotFoundException => new ErrorResponse("Корзина не найдена.", StatusCodes.Status404NotFound),
                DomainException => new ErrorResponse("Неизвестная ошибка.", StatusCodes.Status409Conflict),
                _ => null
            };
        }
    }
}
