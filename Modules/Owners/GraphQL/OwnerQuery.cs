using MillionPropertyApi.Modules.Owners.DTOs;
using MillionPropertyApi.Modules.Owners.Interfaces;

namespace MillionPropertyApi.Modules.Owners.GraphQL;

[ExtendObjectType("Query")]
public class OwnerQuery
{
    public async Task<List<OwnerDto>> GetOwners([Service] IOwnerService ownerService)
    {
        var owners = await ownerService.GetAllAsync();
        return owners.Select(owner => new OwnerDto
        {
            IdOwner = owner.IdOwner,
            Name = owner.Name,
            Address = owner.Address,
            Photo = owner.Photo,
            Birthday = owner.Birthday
        }).ToList();
    }

    public async Task<OwnerDto?> GetOwner(string id, [Service] IOwnerService ownerService)
    {
        var owner = await ownerService.GetByIdAsync(id);
        if (owner == null)
        {
            return null;
        }

        return new OwnerDto
        {
            IdOwner = owner.IdOwner,
            Name = owner.Name,
            Address = owner.Address,
            Photo = owner.Photo,
            Birthday = owner.Birthday
        };
    }
}

