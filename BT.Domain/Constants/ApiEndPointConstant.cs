namespace BT.Domain.Constants;

public class ApiEndPointConstant
{
    public const string RootEndPoint = "/api";
    public const string ApiVersion = "/v1";
    public const string ApiEndpoint = RootEndPoint + ApiVersion;

    public static class Authentication
    {
        public const string AuthenticationEndpoint = ApiEndpoint + "/auth";
        public const string Login = AuthenticationEndpoint + "/login";
        public const string Register = AuthenticationEndpoint + "/register";
    }
    
    public static class Order
    {
        public const string OrderEndpoint = ApiEndpoint + "/order";
        public const string CreateOrder = OrderEndpoint;
        public const string GetAllOrder = OrderEndpoint;
        public const string GetOrder = OrderEndpoint + "/{id}";
    }
}