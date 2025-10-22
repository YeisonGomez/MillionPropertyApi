using MillionPropertyApi.Modules.PropertyTraces.Models;

namespace MillionPropertyApi.Modules.PropertyTraces.Interfaces;

public interface IPropertyTraceService
{
    Task<IEnumerable<PropertyTrace>> GetAllAsync();
    Task<PropertyTrace?> GetByIdAsync(string id);
    Task<IEnumerable<PropertyTrace>> GetByPropertyIdAsync(string propertyId);
    Task<PropertyTrace> CreateAsync(PropertyTrace propertyTrace);
}
