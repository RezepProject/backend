namespace backend.Entities;

public class Setting
{
    public Setting()
    {
    }

    public int Id { get; set; }
    public int ConfigUserId { get; set; }
    public int ConfigUser { get; set; }
    public string Name { get; set; }
    public int DId { get; set; }
    public int BackgroundImageId { get; set; }
    public BackgroundImage? BackgroundImage { get; set; }
    public string Language { get; set; }
    public double TalkingSpeed { get; set; }
    public string GreetingMessage { get; set; }
    public bool State { get; set; }
    public string AiInUse { get; set; }
}

public class CreateSetting
{
    public int BackgroundImageId { get; set; }
    public string Name { get; set; }
    public string Language { get; set; }
    public double TalkingSpeed { get; set; }
    public string GreetingMessage { get; set; }
    public bool State { get; set; }
    public string AiInUse { get; set; }
}