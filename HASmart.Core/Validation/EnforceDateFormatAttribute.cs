using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace HASmart.Core.Validation {
    public class EnforceDateFormatAttribute : ValidationAttribute {
        public EnforceDateFormatAttribute() : base("Datas devem estar no formato DD/MM/YYYY") { }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (!(value is DateTime date)) {
                return new ValidationResult(this.ErrorMessage);
            }

            return date != DateTime.MaxValue ? ValidationResult.Success : new ValidationResult(this.ErrorMessage);
        }
    }

    public class CustomDateTimeConverter : JsonConverter<DateTime> {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            try {
                return DateTime.ParseExact(reader.GetString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            } catch {
                return DateTime.MaxValue;
            }
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) {
            writer.WriteStringValue(value.ToString("dd/MM/yyyy"));
        }
    }
}
