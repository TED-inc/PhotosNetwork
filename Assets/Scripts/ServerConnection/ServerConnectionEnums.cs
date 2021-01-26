namespace TEDinc.PhotosNetwork
{
    public enum GetDataMode
    {
        Before,
        After,
    }

    public enum Result
    {
        Failed,
        Complete,
        ParticularlyComplete,
    }

    public enum ServerConnectionState
    {
        Disconnected,
        Pending,
        Connected,
    }
}