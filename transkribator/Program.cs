using transkribator;

YandexApiConnector yandexApiConnector = new YandexApiConnector("privateKey.txt");

Console.Write("Введите название бакета: "); // audio-convert-1233321
string? backet = Console.ReadLine();

Console.Write("Введите название файла: "); // Int1.mp3
string? file = Console.ReadLine();

Console.Write("Длительность записи в секундах: ");
int delay = int.Parse(Console.ReadLine()) * 1000 / 6 + 5000;

Console.Clear();

string uri = $"https://storage.yandexcloud.net/{backet}/{file}";
var id = await yandexApiConnector.ConvertStartAsync(uri);
Console.WriteLine($"ID операции: {id}");
bool isReady = false;
await Task.Delay(delay);
while (!isReady)
{
    Console.Write(".");
    await Task.Delay(20000);
    var deserializedResponseString = await yandexApiConnector.TryGetResultAsync(id);

    if (deserializedResponseString?.done == true)
    {
        isReady = true;
        var dirInfo = new DirectoryInfo(@"files");
        if (!dirInfo.Exists)
        {
            dirInfo.Create();
        }

        await using var streamWriter = new StreamWriter($"files/{file?.Split('.')[0]}-Converted.txt");
        foreach (var chunk in deserializedResponseString.response.chunks)
        {
            if (chunk.channelTag == "1")
            {
                await streamWriter.WriteLineAsync(chunk.alternatives[0].text);
            }
        }

        Console.WriteLine($"\n\nГотово!\nПуть к файлу: {dirInfo.FullName + "\\" + file?.Split('.')[0]}-Converted.txt");
    }
}
Console.ReadLine();