namespace BervProject.WebApi.Boilerplate.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Email Send Request
/// </summary>
public class EmailSendRequest
{
    /// <summary>
    /// Destination
    /// </summary>
    [Required]
    public List<string> To { get; } = new();
}