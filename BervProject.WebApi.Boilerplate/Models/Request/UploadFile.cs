using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BervProject.WebApi.Boilerplate.Models.Request
{
    public class UploadFile
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
