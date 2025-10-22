using MillionPropertyApi.Modules.Properties.Models;
using MillionPropertyApi.Modules.Properties.DTOs;

namespace MillionPropertyApi.Modules.Properties.Interfaces;

public interface IPropertyService
{
    Task<(IEnumerable<Property> properties, int totalCount)> GetAllAsync(PropertyFilterDto? filter = null);
    Task<Property?> GetByIdAsync(string id);
    Task<IEnumerable<Property>> GetByOwnerIdAsync(string ownerId);
    Task<Property> CreateAsync(Property property);
}
