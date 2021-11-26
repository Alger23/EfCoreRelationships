using System;
using System.Text.Json.Serialization;
using EfCoreRelationships.JsonConverters;

namespace EfCoreRelationships.Models
{
    public class User
    {
        [JsonConverter(typeof(LongToStringConverter))]
        public long Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; } 
        public DateTime Created { get; set; }
    }
}