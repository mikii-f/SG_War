using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    private List<string> _sentences = new();    //全文を格納
    private List<int> _function = new();        //機能コード(呼び出す関数を表す)を格納
    private int lineNumber = 0;                 //現在の行
    private Text _text;
    public GameObject background;               //そのうち別スクリプトで管理
    private SpriteRenderer _spriteRenderer;
    public Sprite backgroundImage2;

    private void Awake()
    {
        StreamReader reader = new(@"Assets/Scripts/Script.txt");
        while (reader.Peek() != -1)
        {
            _function.Add(int.Parse(reader.ReadLine()));
            _sentences.Add(reader.ReadLine());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
        _spriteRenderer = background.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && _sentences.Count > lineNumber)
        {
            SelectFunction(_function[lineNumber]);
            _text.text = _sentences[lineNumber];
            lineNumber++;
        }
    }

    private void SelectFunction(int n)
    {
        switch (n){
            case 0:
                break;
            case 1:
                _spriteRenderer.sprite = backgroundImage2;
                break;
            default:
                break;
        }
    }
}
