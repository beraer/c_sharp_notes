using GroupB.DTOs;

namespace GroupB.Services;

public interface IDbService
{
    Task<RespondCustomerPurchasesDto> GetTicketsForCustomerAsync(int customerId);
    Task AddCustomerWithPurchasesAsync(RequestCustomerPurchaseDto dto);
}