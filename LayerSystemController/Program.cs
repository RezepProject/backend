using System;
using SharedResources.Entities;
using SharedResources.Repositories;

namespace LayerSystemController
{
    static class Program
    {
        public static async Task Main(string[] args)
        {
            RequestRepository.GetInstance().Requests.Add(new Request()
            {
                Text = "MySuperAwesomeRequest"
            });

            Console.WriteLine("Layer System Controller started.");
            while (true)
            {
                Console.WriteLine("Checking for requests ...");
                Console.WriteLine($"Requests in queue: {RequestRepository.GetInstance().Requests.Count}");
                if (RequestRepository.GetInstance().Requests.Count > 0)
                {
                    var req = RequestRepository.GetInstance().Requests[0];
                    RequestRepository.GetInstance().Requests.RemoveAt(0);
                    Console.WriteLine("Request received.");
                    Console.WriteLine($"Handling request {req.Id} ...");
                    RequestHandler.HandleRequest(req);
                    Console.WriteLine($"Request handled in {DateTime.Now - req.CreatedAt}.");
                }
                else
                {
                    await Task.Delay(1000);
                }
            }
        }
    }
}