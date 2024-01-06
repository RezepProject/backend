using SharedResources.Entities;
using SharedResources.Repositories;

namespace LayerSystemController;

public static class ResponseHandler
{
    public static Response? GetById(Guid id)
    {
        return ResponseRepository.GetInstance().Responses.FirstOrDefault(r => r.Id == id);
    }
}