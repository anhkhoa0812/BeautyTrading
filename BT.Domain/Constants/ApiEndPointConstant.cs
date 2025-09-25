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
        public const string UpdateOrder = OrderEndpoint + "/{id}";
        public const string PaymentOrder = OrderEndpoint + "/{id}/payment";
    }

    public static class Product
    {
        public const string ProductEndpoint = ApiEndpoint + "/products";
        public const string ProductWithId = ProductEndpoint + "/{id}";
        public const string ProductWithProductColor = ProductWithId + "/product-colors";
        public const string ProductWithProductVariant = ProductWithId + "/product-variants";
    }

    public static class Category
    {
        public const string CategoryEndpoint = ApiEndpoint + "/categories";
        public const string CategoryWithId = CategoryEndpoint + "/{id}";
    }

    public static class ProductVariant
    {
        public const string ProductVariantEndpoint = ApiEndpoint + "/product-variants";
        public const string ProductVariantWithId = ProductVariantEndpoint + "/{id}";
    }
    public static class News
    {
        public const string NewsEndpoint = ApiEndpoint + "/news";
        public const string NewsWithId = NewsEndpoint + "/{id}";    
    }
    
    public static class Image
    {
        public const string ImageEndpoint = ApiEndpoint + "/images";
        public const string CreateImage = ImageEndpoint;
    }
    public static class Transaction
    {
        public const string TransactionEndpoint = ApiEndpoint + "/transactions";
        public const string GetAllTransaction = TransactionEndpoint;
        public const string GetTransactionById = TransactionEndpoint + "/{id}";
        public const string Handle = TransactionEndpoint + "/handle";
        public const string Cancel = TransactionEndpoint + "/cancel";
    }
}