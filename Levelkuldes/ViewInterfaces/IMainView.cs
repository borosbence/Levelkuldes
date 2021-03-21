using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Levelkuldes.ViewInterfaces
{
    public interface IMainView
    {
        string Allapot { set; }
        void ShowProgress(int progressPercentage, string userState = null);
    }
}
