namespace CaracalGateway.Model
{
    public class ServiceResponse
    {
        public bool Success { get; set; } = true;
        public string? Error { get; set; }
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T? Data { get; set; }
    }
}
