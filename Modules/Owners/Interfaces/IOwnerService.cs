using MillionPropertyApi.Modules.Owners.Models;

namespace MillionPropertyApi.Modules.Owners.Interfaces;

public interface IOwnerService
{
    Task<IEnumerable<Owner>> GetAllAsync();
    Task<Owner?> GetByIdAsync(string id);
    Task<Owner> CreateAsync(Owner owner);
}
