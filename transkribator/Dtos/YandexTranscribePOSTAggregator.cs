namespace transkribator.Dtos;

public class YandexTranscribePOSTAggregator
{
    public object config { get; set; } = new Config();
    public object audio { get; set; }
    
    public YandexTranscribePOSTAggregator(string audioUri)
    {
        audio = new { uri = audioUri };
    }
}


public class Config
{
    public Specification specification { get; set; } = new Specification(); 
}   

public class Specification
{
    public string languageCode { get; set; } = "ru-RU";
    public string model { get; set; } = "general";
    public string literature_text { get; set; } = "true";
    public string audioEncoding { get; set; } = "MP3";
}   