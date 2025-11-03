namespace BervProject.WebApi.Boilerplate.Services.AWS;

using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Email Service Interface
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send Email
    /// </summary>
    /// <param name="receiver"></param>
    /// <returns></returns>
    Task SendEmail(List<string> receiver);
}