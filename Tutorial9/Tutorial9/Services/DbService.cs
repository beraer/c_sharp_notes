using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Tutorial9.Model;

namespace Tutorial9.Services;

public class DbService : IDbService
{
    private readonly IConfiguration _configuration;

    public DbService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<int> CreateProductWarehouseAsync(CreateProductWarehouseDTO dto)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();
        
        DbTransaction transaction = await connection.BeginTransactionAsync();
        command.Transaction = transaction as SqlTransaction;

        try
        {
            //check product with the given id exists
            command.Parameters.Clear();
            command.CommandText = @"SELECT 1
                                    FROM [Product]
                                    WHERE IdProduct = @IdProduct";
            command.Parameters.AddWithValue("@IdProduct", dto.IdProduct);

            var exists = await command.ExecuteScalarAsync();
            if (exists is null)
            {
                throw new Exception($"Product with ID {dto.IdProduct} not found");
            }

            //check warehouse id exists
            command.Parameters.Clear();
            command.CommandText = @"SELECT 1
                                    FROM [Warehouse]
                                    WHERE IdWarehouse = @IdWarehouse";
            command.Parameters.AddWithValue("@IdWarehouse", dto.IdWarehouse);
            var warehouse = await command.ExecuteScalarAsync();
            if (warehouse is null)
                throw new Exception($"Warehouse with ID {dto.IdWarehouse} not found");

            //check amount
            if (dto.Amount <= 0)
                throw new Exception($"Product with ID {dto.IdProduct} does not have an amount");

            //check in order table
            command.Parameters.Clear();
            command.CommandText = @"SELECT IdOrder
                                    FROM [Order]
                                    WHERE IdProduct = @IdProduct
                                    AND Amount = @Amount
                                    AND CreatedAt < @CreatedAt";
            command.Parameters.AddWithValue("@IdProduct", dto.IdProduct);
            command.Parameters.AddWithValue("@Amount", dto.Amount);
            command.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);
            var orderId = await command.ExecuteScalarAsync();
            if (orderId is null)
                throw new Exception($"Product with ID {dto.IdProduct} not found in order list");

            //is it completed
            command.Parameters.Clear();
            command.CommandText = @"SELECT 1
                                    FROM [Product_Warehouse]
                                    WHERE IdOrder = @IdOrder";
            command.Parameters.AddWithValue("@IdOrder", orderId);
            var IsOrderCompleted = await command.ExecuteScalarAsync();
            if (IsOrderCompleted is not null)
                throw new Exception($"Product with ID {dto.IdProduct} completed already");

            //update the FullfilledAt 
            command.Parameters.Clear();
            command.CommandText = @"UPDATE [Order] 
                                    SET FulfilledAt = @FulfilledAt
                                    WHERE IdOrder = @IdOrder";
            command.Parameters.AddWithValue("@FulfilledAt", DateTime.Now);
            command.Parameters.AddWithValue("@IdOrder", orderId);
            await command.ExecuteNonQueryAsync();

            //find price of the product
            command.Parameters.Clear();
            command.CommandText = @"SELECT [Price]
                                    FROM [Product]
                                    WHERE IdProduct = @IdProduct";
            command.Parameters.AddWithValue("@IdProduct", dto.IdProduct);
            var price = await command.ExecuteScalarAsync();
            decimal unitPrice = Convert.ToDecimal(price);
            decimal totalPrice = unitPrice * dto.Amount;

            //insert
            command.Parameters.Clear();
            command.CommandText =
                @"
                    INSERT INTO [Product_Warehouse] 
                    (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)
                    OUTPUT INSERTED.IdProductWarehouse
                    VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt);
                ";
            command.Parameters.AddWithValue("@IdProduct", dto.IdProduct);
            command.Parameters.AddWithValue("@IdWarehouse", dto.IdWarehouse);
            command.Parameters.AddWithValue("@IdOrder", orderId);
            command.Parameters.AddWithValue("@Amount", dto.Amount);
            command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
            command.Parameters.AddWithValue("@Price", totalPrice);

            var insertedIdObj = await command.ExecuteScalarAsync();
            if (insertedIdObj is null)
                throw new Exception("Insert failed");

            await transaction.CommitAsync();
            return Convert.ToInt32(insertedIdObj);

        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

    }
    
    public async Task<int> CreateProductWarehouseUsingProcAsync(CreateProductWarehouseDTO dto)
    {
        if (dto.Amount <= 0)
            throw new ArgumentException("Amount must be greater than 0");

        using var con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        await con.OpenAsync();

        using var cmd = new SqlCommand("AddProductToWarehouse", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@IdProduct", dto.IdProduct);
        cmd.Parameters.AddWithValue("@IdWarehouse", dto.IdWarehouse);
        cmd.Parameters.AddWithValue("@Amount", dto.Amount);
        cmd.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);

        try
        {
            var result = await cmd.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException("Stored procedure returned no ID");
            }

            return Convert.ToInt32(result);
        }
        catch (SqlException ex)
        {
            // Optionally parse error numbers and raise more detailed responses
            throw new InvalidOperationException($"SQL error occurred: {ex.Message}", ex);
        }
    }

    
}


/*private readonly IConfiguration _configuration;
    public DbService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task DoSomethingAsync()
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();

        DbTransaction transaction = await connection.BeginTransactionAsync();
        command.Transaction = transaction as SqlTransaction;

        // BEGIN TRANSACTION
        try
        {
            command.CommandText = "INSERT INTO Animal VALUES (@IdAnimal, @Name);";
            command.Parameters.AddWithValue("@IdAnimal", 1);
            command.Parameters.AddWithValue("@Name", "Animal1");
        
            await command.ExecuteNonQueryAsync();
        
            command.Parameters.Clear();
            command.CommandText = "INSERT INTO Animal VALUES (@IdAnimal, @Name);";
            command.Parameters.AddWithValue("@IdAnimal", 2);
            command.Parameters.AddWithValue("@Name", "Animal2");
        
            await command.ExecuteNonQueryAsync();
            
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }
        // END TRANSACTION
    }

    public async Task ProcedureAsync()
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();
        
        command.CommandText = "NazwaProcedury";
        command.CommandType = CommandType.StoredProcedure;
        
        command.Parameters.AddWithValue("@Id", 2);
        
        await command.ExecuteNonQueryAsync();
        
    }*/