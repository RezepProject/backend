using System;
using SharedResources.Repositories;

namespace LayerSystemController
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Waiting for requests...");
                var req = RequestRepository.Instance.Requests[0];
                RequestRepository.Instance.Requests.RemoveAt(0);
                Console.WriteLine("Request received.");
                Console.WriteLine($"Handling request {req.Id} ...");
                RequestHandler.HandleRequest(req);
                Console.WriteLine("Request handled.");

                await Task.Delay(1000);
            }
        }
    }
}