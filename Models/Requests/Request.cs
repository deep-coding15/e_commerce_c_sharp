namespace E_commerce_c_charp.Models.Requests;
public record AddToCartRequest(int ProductId, int Quantity = 1);
public record RemoveToCartRequest(int ProductId);
//public record