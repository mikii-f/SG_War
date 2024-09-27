using System.IO;
using UnityEngine;

public class TextManager0 : TextManagerOrigin
{
    //今のところどのTextManagerも同じはたらきをするため、ファイル名とシーン名を紐付ければシーンごとに分ける必要はない
    //が、今後変えたくなる可能性もあるためとりあえずこのままで
    private void Awake()
    {
        StreamReader reader = new(Application.dataPath + "/StreamingAssets/Script0.txt");
        while (reader.Peek() != -1)
        {
            _function.Add(reader.ReadLine().Split(','));
            _names.Add(reader.ReadLine());
            _sentences.Add(reader.ReadLine());
        }
    }
}
