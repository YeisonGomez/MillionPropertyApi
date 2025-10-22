using MillionPropertyApi.Modules.PropertyTraces.DTOs;
using MillionPropertyApi.Modules.PropertyTraces.Interfaces;
using MillionPropertyApi.Modules.PropertyTraces.Models;

namespace MillionPropertyApi.Modules.PropertyTraces.GraphQL;

[ExtendObjectType("Mutation")]
public class PropertyTraceMutation
{
    public async Task<PropertyTraceDto> CreatePropertyTrace(CreatePropertyTraceDto input, [Service] IPropertyTraceService propertyTraceService)
    {
        var trace = new PropertyTrace
        {
            DateSale = input.DateSale,
            Name = input.Name,
            Value = input.Value,
            Tax = input.Tax,
            IdProperty = input.IdProperty
        };

        var createdTrace = await propertyTraceService.CreateAsync(trace);

        return new PropertyTraceDto
        {
            IdPropertyTrace = createdTrace.IdPropertyTrace,
            DateSale = createdTrace.DateSale,
            Name = createdTrace.Name,
            Value = createdTrace.Value,
            Tax = createdTrace.Tax,
            IdProperty = createdTrace.IdProperty
        };
    }
}
