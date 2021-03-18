namespace Levelkuldes.ViewInterfaces
{
    public interface IAddressView
    {
        string cimzettFajlNev { get; set; }
        string errorCimzettFajl { set; }
        int BeolvasottElemek { set; }
        string eredmenyKimenet { get; set; }
    }
}
