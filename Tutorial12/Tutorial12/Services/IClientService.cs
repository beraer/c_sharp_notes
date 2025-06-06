namespace Tutorial12.Services;

public interface IClientService
{
    Task<bool> DeleteClientAsync(int clientId);
} 