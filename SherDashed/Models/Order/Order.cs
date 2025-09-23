using System.ComponentModel.DataAnnotations;

namespace SherDashed.Models.Order;

public class Order
{
    public int OrderId { get; set; }
    [Required]
    public string CustomerName { get; set; } = string.Empty;
    public string PurchaseOrder { get; set; } = "N/A";
    [Required]
    public DateTime EntryDate { get; set; }
    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.None;
    public List<OrderDetails> Details { get; set; } = [];
    public bool Delivery { get; set; } = false;
}