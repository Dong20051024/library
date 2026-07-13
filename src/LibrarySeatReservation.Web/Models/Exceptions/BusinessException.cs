namespace LibrarySeatReservation.Web.Models.Exceptions;

public class BusinessException : Exception
{
    public string UserMessage { get; }

    public BusinessException(string userMessage) : base(userMessage)
    {
        UserMessage = userMessage;
    }
}
