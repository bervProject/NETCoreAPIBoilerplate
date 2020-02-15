using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BervProject.WebApi.Boilerplate.Models
{
    public class EmailSendRequest
    {
        public List<string> To { get; set; }
    }
}
