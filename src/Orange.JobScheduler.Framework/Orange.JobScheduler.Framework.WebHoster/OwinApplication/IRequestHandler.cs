namespace Orange.JobScheduler.Framework.WebHoster.OwinApplication
{
    public interface IRequestHandler
    {
        HandleResult HandleRequest(BaseRequest request);
    }
}
