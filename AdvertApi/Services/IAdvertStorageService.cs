using AdvertApi.Models;

namespace AdvertApi.Services;

public interface IAdvertStorageService
{
    Task<string> AddAsync(CreateAdvertRequest model);

    Task ConfirmAsync(ConfirmAdvertRequest model);

    Task<bool> CheckHealthAsync();

    Task<AdvertDbModel> GetByIdAsync(string id);
}
