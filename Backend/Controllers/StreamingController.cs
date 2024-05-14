using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class StreamingController : ControllerBase
{
    [HttpPost]
    public ActionResult<string> SendFrame([FromBody] FrameObject frame)
    {
        // Decode the frame data from base64
        var frameBytes = Convert.FromBase64String(frame.Data);

        // Process the frame using Emgu CV
        using (var image = new Mat())
        {
            CvInvoke.Imdecode(frameBytes, ImreadModes.Color, image);

            if (image.IsEmpty) return BadRequest();

            var faceDetector =
                new CascadeClassifier(@".\Resources\haarcascade_frontalface_default.xml");
            var faces = faceDetector.DetectMultiScale(image, 1.1, 3, Size.Empty, Size.Empty);

            return Ok(faces);
        }
    }

    public class FrameObject
    {
        public string Data { get; set; } = "";
    }
}