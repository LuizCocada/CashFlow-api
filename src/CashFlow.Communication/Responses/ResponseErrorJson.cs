namespace CashFlow.Communication.Responses;
public class ResponseErrorJson
{
    public List<string> ErrorMessages { get; set; }

    public ResponseErrorJson(string ErroMessage)
    {
        ErrorMessages = [ErroMessage];
    }


    public ResponseErrorJson(List<string> errorMessages )
    {
        ErrorMessages = errorMessages;
    }
}



// O PRIMEIRO Construtor recebe UMA mensagem de erro e ErroMessage inicializa uma nova lista recebendo a string


// O SEGUNDO Construtor recebe uma Lista de errors