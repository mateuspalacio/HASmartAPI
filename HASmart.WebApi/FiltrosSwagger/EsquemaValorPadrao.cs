using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HASmart.WebApi.FiltrosSwagger
{
    public class EsquemaValorPadrao : ISchemaFilter
    {
        public void Apply(OpenApiSchema esquema, SchemaFilterContext contexto)
        {
            //Verifica se a propriedade do esquema é nula:
            if (esquema.Properties == null)
            {
                return;
            }

            //Verifica a lista de propriedades de acordo com o seu esquema:
            foreach (var property in esquema.Properties)
            {
                if (property.Value.Default != null && property.Value.Example == null)
                {
                    property.Value.Example = property.Value.Default;
                }
            }
        }
    }
}