using AdvertApi.Models;

namespace AdvertApi.Services;

public interface IAdvertStorageService
{
    Task<string> Add(CreateAdvertRequest model);

    Task Confirm(ConfirmAdvertRequest model);

    Task<bool> CheckHealthAsync();
}
