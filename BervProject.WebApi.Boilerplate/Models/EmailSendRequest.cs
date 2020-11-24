using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BervProject.WebApi.Boilerplate.Models
{
    public class EmailSendRequest
    {
        [Required]
        public List<string> To { get; set; }
    }
}
