using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TextManager0 : TextManagerOrigin
{
    public GameObject imManager0;
    private ImagesManager0 imagesManager0;

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
    // Start is called before the first frame update
    void Start()
    {
        mainText = GameObject.Find("Text").GetComponent<Text>();
        nameText = GameObject.Find("Name").GetComponent<Text>();
        imagesManager0 = imManager0.GetComponent<ImagesManager0>();
        GoNextLine();
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
                    imagesManager0.ChangeScene();
                    break;
            }
        }
    }
}
