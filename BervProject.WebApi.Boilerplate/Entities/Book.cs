namespace BervProject.WebApi.Boilerplate.Entities;

using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Book Entity
/// </summary>
public class Book
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
    /// Publisher
    /// </summary>
    public virtual Publisher Publisher { get; set; }
}