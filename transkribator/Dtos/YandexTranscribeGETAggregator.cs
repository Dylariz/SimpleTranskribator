namespace transkribator.Dtos;

public class YandexTranscribeGETAggregator
{
    public bool done { get; set; }
    public Response response { get; set; }
}

public class Response
{
    public Chunk[] chunks { get; set; }
    public string id { get; set; }
    public string createdAt { get; set; }
    public string modifiedAt { get; set; }
}

public class Chunk
{
    public Alternative[] alternatives { get; set; }
    public string channelTag { get; set; }
}

public class Alternative
{
    public Word[] words { get; set; }
    public string text { get; set; }
    public string confidence { get; set; }
}

public class Word
{
    public string startTime { get; set; }
    public string endTime { get; set; }
    public List<Alternative> alternatives { get; set; }
    public string word { get; set; }
    public string confidence { get; set; }
}