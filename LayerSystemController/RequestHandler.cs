using SharedResources.Entities;
using SharedResources.Repositories;

namespace LayerSystemController;

public static class RequestHandler
{
    public static void HandleRequest(Request req)
    {
        var res = new Response()
        {
            Id = req.Id,
            Text = "MySuperAwesomeResult,WhichIsCompletelyDone"
        };
        // TODO
        ResponseRepository.GetInstance().Responses.Add(res);
    }

    public static void AddRequest(Request req)
    {
        RequestRepository.GetInstance().Requests.Add(req);
    }
}