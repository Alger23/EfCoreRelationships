using System;
using System.Text.Json.Serialization;
using EfCoreRelationships.JsonConverters;

namespace EfCoreRelationships.Models
{
    // one to one
    public class User
    {
        [JsonConverter(typeof(LongToStringConverter))]
        public long Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; } 
        public DateTime Created { get; set; }
        
        public virtual Profile Profile { get; set; }
    }

    public class Profile
    {
        [JsonConverter(typeof(LongToStringConverter))]
        public long UserId { get; set; }
        public string Email { get; set; }
        
        public virtual User User { get; set; }
    }
}