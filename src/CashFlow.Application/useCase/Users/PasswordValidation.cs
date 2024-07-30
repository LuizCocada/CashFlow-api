using System.Text.RegularExpressions;
using CashFlow.Exception;
using FluentValidation;
using FluentValidation.Validators;

namespace CashFlow.Application.useCase.Users;
public class PasswordValidation<T> : PropertyValidator<T, string>
{
    private const string KEY_ERROR_MESSAGE = "ErrorMessage";
    public override string Name => "PasswordValidation";

    protected override string GetDefaultMessageTemplate(string errorCode) //necessário para o funcionamento do AppendArgument()
    {
        // é necessario desta forma. => return "{ErrorMessage};" => a mensagem precisa da chave mesmo sendo string.
        //iremos fazer desta forma. => return $"{{{KEY_ERROR_MESSAGE}}}" pois se fosse feito com $"{KEY_ERROR_MESSAGE}" e mensagem retornada seria
        //=> "ErrorMessage", porem nos precisamos das chaves

        return $"{{{KEY_ERROR_MESSAGE}}}";
    }
    
    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument(KEY_ERROR_MESSAGE, ResourceErrorMessages.PASSWORD_INVALID);
            return false;
        }
        
        if (password.Length < 8)
        {
            context.MessageFormatter.AppendArgument(KEY_ERROR_MESSAGE, ResourceErrorMessages.PASSWORD_INVALID);
            return false;
        }

        if (Regex.IsMatch(password, @"[A-Z]+") == false) //bool para letra maiuscula.
        {
            context.MessageFormatter.AppendArgument(KEY_ERROR_MESSAGE, ResourceErrorMessages.PASSWORD_INVALID);
            return false;
        }
        
        if (Regex.IsMatch(password, @"[a-z]+") == false) 
        {
            context.MessageFormatter.AppendArgument(KEY_ERROR_MESSAGE, ResourceErrorMessages.PASSWORD_INVALID);
            return false;
        }
        
        if (Regex.IsMatch(password, @"[0-9]+") == false) 
        {
            context.MessageFormatter.AppendArgument(KEY_ERROR_MESSAGE, ResourceErrorMessages.PASSWORD_INVALID);
            return false;
        }
        
        if (Regex.IsMatch(password, @"[\!\?\*\.]+") == false) 
        {
            context.MessageFormatter.AppendArgument(KEY_ERROR_MESSAGE, ResourceErrorMessages.PASSWORD_INVALID);
            return false;
        }
        
        return true;
    }

}