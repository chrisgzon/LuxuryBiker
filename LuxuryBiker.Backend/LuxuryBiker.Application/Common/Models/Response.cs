namespace LuxuryBiker.Application.Common.Models;

public class Response
{
    public bool Error { get; set; }

    public string Message { get; set; }

    public long Number { get; set; }

    //Excluye de grillas sin url para Web Methods
    public bool ExcludeGridMethod { get; set; }

    public Response()
    {
        Error = false;
        Message = string.Empty;
    }

    public Response(bool error, long number, string mensaje)
    {
        Error = error;
        Number = number;
        Message = mensaje;
    }

    public Response(bool error, string mensaje)
    {
        Error = error;
        Message = mensaje;
    }

    public Response(string mensaje)
    {
        Error = !string.IsNullOrWhiteSpace(mensaje);
        Message = mensaje;
    }
}
public class ResponseGeneric<T> : Response
{
    public object ExtraData { get; set; }
    public T Result { get; set; }
    public T value { get; set; }
}
