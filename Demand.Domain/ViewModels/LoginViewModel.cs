using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Domain.ViewModels
{
    public class LoginViewModel
    {
        public string UserEmail{ get; set; }

        public string Password { get; set; }

        public string Result{ get; set; }
    }
}
