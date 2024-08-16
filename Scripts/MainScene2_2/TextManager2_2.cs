using System.IO;
using UnityEngine;

public class TextManager2_2 : TextManagerOrigin
{
    private void Awake()
    {
        StreamReader reader = new(Application.dataPath + "/StreamingAssets/Script2_2.txt");
        while (reader.Peek() != -1)
        {
            _function.Add(reader.ReadLine().Split(','));
            _names.Add(reader.ReadLine());
            _sentences.Add(reader.ReadLine());
        }
    }
}
