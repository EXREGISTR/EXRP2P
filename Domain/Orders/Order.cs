using Domain.Common;

namespace Domain.Orders;

public class Order {
    public Guid Id { get; private set; }

    public Guid CreatorId { get; private set; }

    public Guid FromCurrencyId { get; private set; }
    public Guid TargetCurrencyId { get; private set; }

    public decimal Amount { get; private set; }
    public decimal Price { get; private set; }

    public string[] PaymentBanks { get; private set; } = [];

    public OrderStatus Status { get; private set; }
    public OrderType Type { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    private Order() { }

    public Order(
        Guid creatorId,
        decimal amount, 
        decimal price,
        Guid fromCurrencyId,
        Guid targetCurrencyId, 
        string[] paymentBanks,
        OrderType type,
        IDateTimeProvider timeProvider) {
        Id = Guid.CreateVersion7();

        CreatorId = creatorId;
        Amount = amount;
        Price = price;

        FromCurrencyId = fromCurrencyId;
        TargetCurrencyId = targetCurrencyId;
        PaymentBanks = paymentBanks;

        Status = OrderStatus.Pending;
        Type = type;
        
        CreatedAt = timeProvider.UtcNow;
    }

    public void Reserve() {
        if (Status != OrderStatus.Pending) {
            throw OrderExceptions.CanReserveOnlyForOrderWaitingDeal;
        }

        Status = OrderStatus.Active;
    }

    public void ConfirmPayment() {
        if (Status != OrderStatus.Active) {
            throw OrderExceptions.CanConfirmPaymentOnlyForActiveOrder;
        } 

        Status = OrderStatus.Paid;
    }
}