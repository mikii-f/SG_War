using System.IO;
using UnityEngine;

public class TextManager0 : TextManagerOrigin
{
    private ImagesManager0 imagesManager;

    private void Awake()
    {
        StreamReader reader = new(@"Assets/Scripts/MainScene0/Script0.txt");
        while (reader.Peek() != -1)
        {
            _function.Add(reader.ReadLine().Split(','));
            _names.Add(reader.ReadLine());
            _sentences.Add(reader.ReadLine());
        }
    }

    protected override void StartSet()
    {
        imagesManager = imManager.GetComponent<ImagesManager0>();
    }

    //テキストに記述した機能コードに応じて関数呼び出し
    protected override void SelectFunction(string[] s)
    {
        int n = s.Length;
        for (int i = 0; i < n; i++)
        {
            switch (s[i])
            {
                case "0":
                    break;
                case "ChangeScene":
                    i++;
                    imagesManager.ChangeScene(s[i]);
                    break;
            }
        }
    }
}
