using System.Collections.Generic;

namespace BervProject.WebApi.Boilerplate.Services
{
    public interface IEmailService
    {
        void SendEmail(List<string> receiver);
    }
}
