using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BervProject.WebApi.Boilerplate.Entities
{
    public class Publisher
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Book> Books { get; set; }
    }
}
