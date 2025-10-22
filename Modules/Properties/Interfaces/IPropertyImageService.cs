namespace MillionPropertyApi.Modules.Properties.Interfaces;

public interface IPropertyImageService
{
    Task<string?> GetFirstImageAsync(string propertyId);
    Task<List<string>> GetAllImagesAsync(string propertyId);
}

