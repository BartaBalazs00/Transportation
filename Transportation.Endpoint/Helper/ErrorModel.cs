namespace Transportation.Endpoint.Helper
{
    public class ErrorModel
    {
        public string Message { get; }

        public ErrorModel(string message)
        {
            this.Message = message;
        }
    }
}