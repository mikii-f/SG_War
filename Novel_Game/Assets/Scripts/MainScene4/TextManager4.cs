using System.IO;
using UnityEngine;

public class TextManager4 : TextManagerOrigin
{
    private void Awake()
    {
        StreamReader reader = new(@"Assets/Scripts/MainScene4/Script4.txt");
        while (reader.Peek() != -1)
        {
            _function.Add(reader.ReadLine().Split(','));
            _names.Add(reader.ReadLine());
            _sentences.Add(reader.ReadLine());
        }
    }
}
