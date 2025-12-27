namespace FeeCalculationEngine.Application.Dtos.Requests;

public record TransactionRequest(
    int UserId,
    int TransactionTypeId,
    decimal Amount,
    decimal Fee);