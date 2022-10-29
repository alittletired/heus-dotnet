using System.ComponentModel.DataAnnotations;
using Heus.Core.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Heus.AspNetCore.Validation;

internal class ModelStateValidator: IModelStateValidator,IScopedDependency
{
    public virtual void Validate(ModelStateDictionary modelState)
    {
        var validationResult = new ValidationResult();

        AddErrors(validationResult, modelState);

        if (validationResult.Errors.Any())
        {
            throw new ValidationException(
                "ModelState is not valid! See ValidationErrors for details.",
                validationResult
            );
        }
    }

    private  void AddErrors(ValidationResult validationResult, ModelStateDictionary modelState)
    {
        if (modelState.IsValid)
        {
            return;
        }

        foreach (var state in modelState)
        {
            foreach (var error in state.Value.Errors)
            {
                validationResult.AddError(error.ErrorMessage, new[] { state.Key });
            }
        }
    }
}