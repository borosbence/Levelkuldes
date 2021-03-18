namespace Levelkuldes.ViewInterfaces
{
    public interface IMessageView
    {
        string Felado { get; }
        string errorFelado { set; }
        string Targy { get; }
        string errorTargy { set; }
        string uzenetHTML { set; }
        void ShowError();
    }
}
