using FluentValidation;

namespace TeSystemBackend.API.Extensions;

public static class ValidationExtensions
{
    public static async Task ValidateAndThrowAsync<T>(this IValidator<T> validator, T instance)
    {
        var validationResult = await validator.ValidateAsync(instance);
        if (!validationResult.IsValid)
        {
            throw new FluentValidation.ValidationException(validationResult.Errors);
        }
    }
}


