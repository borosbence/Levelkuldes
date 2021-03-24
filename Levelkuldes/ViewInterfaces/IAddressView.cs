using System.Collections.Generic;

namespace Levelkuldes.ViewInterfaces
{
    public interface IAddressView
    {
        string cimzettFajl { get; set; }
        string errorCimzettFajl { set; }
        int BeolvasottElemek { set; }
        string eredmenyKimenet { get; set; }
        string[] Fejlecek { get; set; }
        int cimzettOszlop { get; }
    }
}
