namespace BervProject.WebApi.Boilerplate.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

/// <summary>
/// Publisher
/// </summary>
public class Publisher
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name
    /// </summary>
    [Required]
    public string Name { get; set; }
    /// <summary>
    /// Books
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Book> Books { get; set; }
}