namespace backend.Entities;

public class BackgroundImage
{
    public int Id { get; set; }
    public string Base64Image { get; set; }
}

public class CreateBackgroundImage
{
    public string Base64Image { get; set; }
}