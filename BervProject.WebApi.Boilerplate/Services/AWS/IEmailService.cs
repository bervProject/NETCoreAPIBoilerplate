using System.Collections.Generic;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Services.AWS
{
    public interface IEmailService
    {
        Task SendEmail(List<string> receiver);
    }
}
