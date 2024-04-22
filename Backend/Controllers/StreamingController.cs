using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net;
using Emgu.CV;
using Emgu.CV.CvEnum;


namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class StreamingController : ControllerBase
{
    [HttpPost]
    public async Task<HttpResponseMessage> SendFrame(string frameData)
    {
        // Decode the frame data from base64
        byte[] frameBytes = Convert.FromBase64String(frameData.Split(",")[1]);

        // Process the frame using Emgu CV
        using (var image = new Mat())
        {
            CvInvoke.Imdecode(frameBytes, ImreadModes.Color, image);
            if (image.IsEmpty)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var faceDetector =
                new CascadeClassifier(@".\Resources\haarcascade_frontalface_default.xml"); // Replace with your face detection model path
            Rectangle[] faces = faceDetector.DetectMultiScale(image, 1.1, 3, Size.Empty, Size.Empty);

            // Logic to handle detected faces (e.g., send response)
            // ...

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
