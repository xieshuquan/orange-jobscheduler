namespace Orange.JobScheduler.WebHoster.OwinApplication
{
    public interface IRequestHandler
    {
        HandleResult HandleRequest(BaseRequest request);
    }
}
