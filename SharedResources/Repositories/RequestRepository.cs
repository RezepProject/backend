using SharedResources.Entities;

namespace SharedResources.Repositories;

public class RequestRepository
{
    private static RequestRepository? _instance;

    public static RequestRepository Instance
    {
        get { return _instance ??= new RequestRepository(); }
    }

    public IList<Request> Requests { get; set; }
}