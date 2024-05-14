namespace backend.Entities;

public class Setting
{
    public Setting()
    {
        Name = "Rezep - " + new Guid();
        Language = "de-DE";
        TalkingSpeed = 0.7;
        GreetingMessage = "Hallo, ich bin Rezep. Wie kann ich Ihnen helfen?";
        // TODO: implement State
        State = true;
    }

    public int Id { get; set; }
    public int ConfigUserId { get; set; }
    public int ConfigUser { get; set; }
    public string Name { get; set; }
    public string BackgroundImage { get; set; }
    public string Language { get; set; }
    public double TalkingSpeed { get; set; }
    public string GreetingMessage { get; set; }
    public bool State { get; set; }
}

public class CreateSetting
{
    public string Name { get; set; }
    public string BackgroundImage { get; set; }
    public string Language { get; set; }
    public double TalkingSpeed { get; set; }
    public string GreetingMessage { get; set; }
    public bool State { get; set; }
}