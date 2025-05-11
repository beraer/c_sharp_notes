using Tutorial9.Model;

namespace Tutorial9.Services;

public interface IDbService
{
    Task<int> CreateProductWarehouseAsync(CreateProductWarehouseDTO dto);
    Task<int> CreateProductWarehouseUsingProcAsync(CreateProductWarehouseDTO dto);
}