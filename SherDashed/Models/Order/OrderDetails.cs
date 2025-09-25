namespace SherDashed.Models.Order;

public class OrderDetails
{
    public int OrderDetailsId { get; set; }
    public string SalesNumber { get; set; } = string.Empty;
    public Size? Size { get; set; }
    public string ProductNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string BatchDate { get; set; } = string.Empty;
}