using Domain.Common;

namespace Domain.Deals;

public class Deal {
    public Guid Id { get; private set; }

    public Guid BuyerId { get; private set; }
    public Guid SellerId { get; private set; }

    public string? PaymentProofUrl { get; private set; }

    public DateTime StartedAtUtc { get; private set; }
    public DateTime? FinishedAtUtc { get; private set; }

    public TimeSpan? Duration => FinishedAtUtc != null
        ? FinishedAtUtc - StartedAtUtc
        : null;

    public DealStatus Status { get; private set; }

    private Deal() { }

    public Deal(Guid id, Guid buyerId, Guid sellerId, IDateTimeProvider timeProvider) {
        Id = id;
        BuyerId = buyerId;
        SellerId = sellerId;

        StartedAtUtc = timeProvider.UtcNow;
        Status = DealStatus.Active;
    }

    public void LoadPaymentProofUrl(string url) 
        => PaymentProofUrl = url;

    public void Dispute() {
        if (Status != DealStatus.Active) {
            throw new InvalidOperationException("Deal is not active");
        }

        Status = DealStatus.Disputing;
    }

    public void Close(IDateTimeProvider timeProvider) {
        if (Status == DealStatus.Closed) {
            throw new InvalidOperationException("Deal already finished");
        }

        FinishedAtUtc = timeProvider.UtcNow;
    }
}
