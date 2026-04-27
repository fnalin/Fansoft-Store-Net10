using System;

namespace DemoAuth.Api.Models;



public sealed class PurchaseOrder

{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
