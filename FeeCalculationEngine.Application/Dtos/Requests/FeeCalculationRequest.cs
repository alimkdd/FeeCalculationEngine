namespace FeeCalculationEngine.Application.Dtos.Requests;

public record FeeCalculationRequest(
    int UserId,
    int TransactionTypeId,
    decimal Amount,
    int PaymentMethodId,
    int PromoCodeId);