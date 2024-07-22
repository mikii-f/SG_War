using System.IO;
using UnityEngine;

public class TextManager3 : TextManagerOrigin
{
    private void Awake()
    {
        StreamReader reader = new(Application.dataPath + "/StreamingAssets/Script3.txt");
        while (reader.Peek() != -1)
        {
            _function.Add(reader.ReadLine().Split(','));
            _names.Add(reader.ReadLine());
            _sentences.Add(reader.ReadLine());
        }
    }
}
