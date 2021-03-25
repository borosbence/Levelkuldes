namespace Levelkuldes.ViewInterfaces
{
    public interface IMessageView
    {
        string Felado { get; }
        string errorFelado { set; }
        string Targy { get; }
        string errorTargy { set; }
        string uzenetFajl { set; }
        void ShowError();
    }
}
