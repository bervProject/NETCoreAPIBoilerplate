using System;
using System.ComponentModel.DataAnnotations;

namespace BervProject.WebApi.Boilerplate.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual Publisher Publisher { get; set; }
    }
}
