using System.Collections.Generic;
using System.Collections;
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
    public Text _text;
    public Text nameText;
    public GameObject imManager;
    private ImagesManager imagesManager;
    private bool isAnimation = false; 

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
        imagesManager = imManager.GetComponent<ImagesManager>();
        StartCoroutine(GoNextLine());
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
                StartCoroutine(GoNextLine());
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

    //ページ送り
    private IEnumerator GoNextLine()
    {
        yield return null;
        SelectFunction(_function[lineNumber]);
        tempText = _sentences[lineNumber];
        textLength = tempText.Length;
        _text.text = "";
        nameText.text = _names[lineNumber];
        displayWordNumber = 0;
        lineNumber++;
    }

    //テキストに記述した機能コードに応じて関数呼び出し
    //0番台→演出用の黒白関係　10番台→立ち絵関係　20番台→背景関係
    private void SelectFunction(int n)
    {
        switch (n){
            case 0:
                break;
            case 1:
                imagesManager.BlackOnOff();
                break;
            case 2:
                StartCoroutine(imagesManager.BlackHalfOpen());
                break;
            case 3:
                StartCoroutine(imagesManager.BlackHalfToWhite());
                break;
            case 10:
                imagesManager.CharacterChange(0);
                break;
            case 11:
                imagesManager.CharacterChange(1);
                break;
            case 12:
                imagesManager.CharacterChange(2);
                break;
            case 20:
                imagesManager.BackGroundChange(0);
                break;
            case 21:
                imagesManager.BackGroundChange(1);
                break;
            case 22:
                imagesManager.BackGroundChange(2);
                break;
            case 23:
                imagesManager.BackGroundChange(3);
                break;
            case 100:
                isAnimation = true;
                StartCoroutine(imagesManager.TitleAnimation());
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
    
    //アニメーションが終了したら1行進み操作可能に
    public void AnimationFinished()
    {
        isAnimation = false;
        GoNextLine();
    }
}
