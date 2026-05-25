namespace Capsula.Application.Exceptions.Images;

public class ImageValidationException : Exception
{
    public ImageValidationException(string message) : base(message)
    {
    }
}
