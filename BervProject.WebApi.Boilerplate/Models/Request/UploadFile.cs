namespace BervProject.WebApi.Boilerplate.Models.Request;

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Upload File
/// </summary>
public class UploadFile
{
    /// <summary>
    /// File
    /// </summary>
    [Required]
    public IFormFile File { get; set; }
}