namespace DemoAuth.Mvc.Models;

public sealed record TicketViewModel(int Id, string Titulo);
public sealed record PurchaseOrderViewModel(int Id, string Description, decimal Amount);

public sealed record ProductViewModel(int Id, string Name, decimal Price);

public sealed class LoginViewModel
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}