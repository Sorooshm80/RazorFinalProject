using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAdminSettings
    {
        string Password { get; }
    }

    public class AdminSettings : IAdminSettings
    {
        public string Password { get; set; }
    }
}
