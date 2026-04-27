using System;

namespace DemoAuth.Api.Models;

public sealed record DemoTokenRequest(string UserName);

public sealed record UpdateProductRequest(string Name, decimal Price);

public sealed record UpdatePurchaseOrderRequest(string Description, decimal Amount);