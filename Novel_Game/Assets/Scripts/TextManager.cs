using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    private List<string> _sentences = new();    //全文を格納
    private List<int> _function = new();        //機能コード(呼び出す関数を表す)を格納
    private List<string> _names = new();        //名前を格納
    private int lineNumber = 0;                 //現在の行
    private int displayWordNumber = 0;          //表示する文字数の管理(1文字ずつ表示する用)
    private float readTime = 0.03f;             //文字表示スピード
    private float timeCount = 0f;               //時間保持用
    private string tempText;                    //表示しようとしているテキスト
    private int textLength;                     //表示しようとしているテキストの長さ
    private Text _text;
    public Text nameText;
    public GameObject imManager;
    private ImagesManager imagesManager;
    private bool isAnimation = true; 

    private void Awake()
    {
        StreamReader reader = new(@"Assets/Scripts/Script.txt");
        while (reader.Peek() != -1)
        {
            _function.Add(int.Parse(reader.ReadLine()));
            _names.Add(reader.ReadLine());
            _sentences.Add(reader.ReadLine());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        
        imagesManager = imManager.GetComponent<ImagesManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && _sentences.Count > lineNumber && !isAnimation)
        {
            //表示されていない部分があったら表示
            if (displayWordNumber < textLength)
            {
                TextFill();
            }
            //なければ次へ
            else
            {
                SelectFunction(_function[lineNumber]);
                tempText = _sentences[lineNumber];
                textLength = tempText.Length;
                _text.text = "";
                nameText.text = _names[lineNumber];
                displayWordNumber = 0;
                lineNumber++;
            }
        }
        //一文字ずつ表示
        if (displayWordNumber < textLength && timeCount > readTime)
        {
            //全角スラッシュがきたら改行
            if (tempText[displayWordNumber] == '／')
            {
                _text.text += '\n';
                displayWordNumber++;
                timeCount = 0f;
            }
            else
            {
                _text.text += tempText[displayWordNumber];
                displayWordNumber++;
                timeCount = 0f;
            }
        }
    }

    //テキストに記述した機能コードに応じて関数呼び出し
    private void SelectFunction(int n)
    {
        switch (n){
            case 0:
                break;
            case 1:
                StartCoroutine(imagesManager.BlackHalfOpen());
                break;
            case 2:
                StartCoroutine(imagesManager.BlackHalfToWhite());
                break;
            case 10:
                imagesManager.CharacterHide();
                break;
            case 21:
                imagesManager.BackGroundChange(1);
                break;
            case 22:
                imagesManager.BackGroundChange(2);
                break;
            default:
                break;
        }
    }

    //テキストを一気に表示する用
    private void TextFill()
    {
        while (displayWordNumber < textLength)
        {
            //全角スラッシュがきたら改行
            if (tempText[displayWordNumber] == '／')
            {
                _text.text += '\n';
                displayWordNumber++;
            }
            else
            {
                _text.text += tempText[displayWordNumber];
                displayWordNumber++;
            }
        }
    }

    public void AnimationFinished()
    {
        isAnimation = false;
    }
}
