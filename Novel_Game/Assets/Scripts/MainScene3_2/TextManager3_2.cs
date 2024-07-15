using System.IO;
using UnityEngine;

public class TextManager3_2 : TextManagerOrigin
{
    private void Awake()
    {
        StreamReader reader = new(@"Assets/Scripts/MainScene3_2/Script3_2.txt");
        while (reader.Peek() != -1)
        {
            _function.Add(reader.ReadLine().Split(','));
            _names.Add(reader.ReadLine());
            _sentences.Add(reader.ReadLine());
        }
    }
}
