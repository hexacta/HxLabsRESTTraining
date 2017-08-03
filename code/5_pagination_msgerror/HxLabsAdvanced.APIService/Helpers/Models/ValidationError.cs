using Newtonsoft.Json;

namespace HxLabsAdvanced.APIService.Helpers.Models
{
    //REF 8 Herramienta de validaciones
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }

        public ValidationError(string field, string message)
        {
            this.Field = string.IsNullOrWhiteSpace(field) ? null : field;

            this.Message = message;
        }
    }
}
