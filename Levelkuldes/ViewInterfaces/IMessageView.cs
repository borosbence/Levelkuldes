namespace Levelkuldes.ViewInterfaces
{
    public interface IMessageView
    {
        string felado { get; }
        string errorFelado { set; }
        string targy { get; }
        string errorTargy { set; }
        string uzenetHTML { set; }
        void ShowError();
    }
}
