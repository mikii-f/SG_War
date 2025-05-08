using System.IO;
using UnityEngine;

public class TextManager3_2 : TextManagerOrigin
{
    private void Awake()
    {
        StringReader reader = new(Resources.Load<TextAsset>("Script3_2").text);
        while (reader.Peek() != -1)
        {
            _function.Add(reader.ReadLine().Split(','));
            _names.Add(reader.ReadLine());
            _sentences.Add(reader.ReadLine());
        }
    }
}