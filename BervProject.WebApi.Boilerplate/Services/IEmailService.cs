using System.Collections.Generic;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services
{
    public interface IEmailService
    {
        Task SendEmail(List<string> receiver);
    }
}
