using MillionPropertyApi.Modules.PropertyTraces.DTOs;
using MillionPropertyApi.Modules.PropertyTraces.Interfaces;

namespace MillionPropertyApi.Modules.PropertyTraces.GraphQL;

[ExtendObjectType("Query")]
public class PropertyTraceQuery
{
    public async Task<List<PropertyTraceDto>> GetPropertyTraces([Service] IPropertyTraceService propertyTraceService)
    {
        var traces = await propertyTraceService.GetAllAsync();
        return traces.Select(trace => new PropertyTraceDto
        {
            IdPropertyTrace = trace.IdPropertyTrace,
            DateSale = trace.DateSale,
            Name = trace.Name,
            Value = trace.Value,
            Tax = trace.Tax,
            IdProperty = trace.IdProperty
        }).ToList();
    }

    public async Task<PropertyTraceDto?> GetPropertyTrace(string id, [Service] IPropertyTraceService propertyTraceService)
    {
        var trace = await propertyTraceService.GetByIdAsync(id);
        if (trace == null)
        {
            return null;
        }

        return new PropertyTraceDto
        {
            IdPropertyTrace = trace.IdPropertyTrace,
            DateSale = trace.DateSale,
            Name = trace.Name,
            Value = trace.Value,
            Tax = trace.Tax,
            IdProperty = trace.IdProperty
        };
    }

    public async Task<List<PropertyTraceDto>> GetTracesByProperty(string propertyId, [Service] IPropertyTraceService propertyTraceService)
    {
        var traces = await propertyTraceService.GetByPropertyIdAsync(propertyId);
        return traces.Select(trace => new PropertyTraceDto
        {
            IdPropertyTrace = trace.IdPropertyTrace,
            DateSale = trace.DateSale,
            Name = trace.Name,
            Value = trace.Value,
            Tax = trace.Tax,
            IdProperty = trace.IdProperty
        }).ToList();
    }
}

