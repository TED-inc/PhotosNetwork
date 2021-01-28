namespace TEDinc.PhotosNetwork.MonoComponents
{
    public sealed class PostComment : ClientServiceInteractorBase
    {
        public void Post() =>
            clientRunner.GetService<IClientCommentService>().PostComment();
    }
}