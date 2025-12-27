namespace FeeCalculationEngine.Application.Dtos.Requests;

public record TransactionCountRequest(
    int UserId,
    DateTime ReferenceDate);