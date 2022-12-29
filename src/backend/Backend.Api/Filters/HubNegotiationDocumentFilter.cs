using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Backend.Api.Filters
{
    public class HubNegotiationDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths.Add("/messaging/negotiate", new OpenApiPathItem
            {
                Operations = new Dictionary<OperationType, OpenApiOperation>
                {
                    { OperationType.Post, new OpenApiOperation {
                        OperationId="HubNegotiation",
                        Summary="Endpoint to negotiate connection with Azure SignalR Service Hub",
                        Responses = new OpenApiResponses{
                            { "200", new OpenApiResponse{Description="Success" } },
                            { "401", new OpenApiResponse{Description="Unauthorized" } }
                        }
                    } }
                }
            });
        }
    }
}
