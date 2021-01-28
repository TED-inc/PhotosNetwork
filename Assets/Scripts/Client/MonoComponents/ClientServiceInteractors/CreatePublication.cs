namespace TEDinc.PhotosNetwork.MonoComponents
{
    public sealed class CreatePublication : ClientServiceInteractorBase
    {
        public void Create() =>
            clientRunner.GetService<IClientPublicationService>().CreatePublication();
    }
}