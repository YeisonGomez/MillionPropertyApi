using MillionPropertyApi.Modules.Owners.DTOs;
using MillionPropertyApi.Modules.Owners.Interfaces;
using MillionPropertyApi.Modules.Owners.Models;

namespace MillionPropertyApi.Modules.Owners.GraphQL;

[ExtendObjectType("Mutation")]
public class OwnerMutation
{
    public async Task<OwnerDto> CreateOwner(CreateOwnerDto input, [Service] IOwnerService ownerService)
    {
        var owner = new Owner
        {
            Name = input.Name,
            Address = input.Address,
            Photo = input.Photo,
            Birthday = input.Birthday
        };

        var createdOwner = await ownerService.CreateAsync(owner);

        return new OwnerDto
        {
            IdOwner = createdOwner.IdOwner,
            Name = createdOwner.Name,
            Address = createdOwner.Address,
            Photo = createdOwner.Photo,
            Birthday = createdOwner.Birthday
        };
    }
}
