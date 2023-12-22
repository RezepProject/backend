using SharedResources.Entities;

namespace SharedResources.Repositories;

public class ResponseRepository
{
    private static ResponseRepository? _instance;

    public static ResponseRepository Instance
    {
        get { return _instance ??= new ResponseRepository(); }
    }

    public IList<Response> Responses { get; set; }
}