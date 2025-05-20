namespace Application.Old.Requests;

public record LoadPaymentProofRequest(
    Guid OrderId, 
    string PaymentProofUrl);