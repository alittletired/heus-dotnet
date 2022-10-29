using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Heus.AspNetCore.Validation;

internal interface IModelStateValidator
{
    void Validate(ModelStateDictionary modelState);

}