using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace BimManagerPortal.Application.Other.Services.Validations;

public class ValidationService
{
    public static List<ValidationResult> ValidateObjectRecursive(object obj)
    {
        var results = new List<ValidationResult>();
        ValidateObjectRecursive(obj, results, new HashSet<object>());
        return results;
    }

    private static void ValidateObjectRecursive(object obj, List<ValidationResult> results, HashSet<object> validatedObjects)
    {
        if (obj == null || validatedObjects.Contains(obj))
            return;

        validatedObjects.Add(obj);
        var context = new ValidationContext(obj, serviceProvider: null, items: null);
        
        // Валидация текущего объекта
        Validator.TryValidateObject(obj, context, results, true);

        // Рекурсивная валидация всех свойств
        var properties = obj.GetType().GetProperties()
            .Where(p => p.GetValue(obj) != null && 
                        (p.PropertyType.IsClass || p.PropertyType.IsGenericType));

        foreach (var property in properties)
        {
            var value = property.GetValue(obj);
            
            if (value is IEnumerable enumerable && !(value is string))
            {
                foreach (var item in enumerable)
                {
                    if (item != null && item.GetType().IsClass)
                        ValidateObjectRecursive(item, results, validatedObjects);
                }
            }
            else if (value != null && value.GetType().IsClass)
            {
                ValidateObjectRecursive(value, results, validatedObjects);
            }
        }
    }
}