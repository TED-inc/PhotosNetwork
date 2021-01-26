namespace TEDinc.PhotosNetwork
{
    public delegate void SetActivePage(bool enabled);
    public interface IClientServiceBase
    {
        event SetActivePage onPageActiveStateChange;
        void ShowPage();
        void HidePage();
    }
}
