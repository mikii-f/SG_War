using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class TextManagerOrigin : MonoBehaviour
{
    [SerializeField] protected ImagesManagerOrigin imagesManager;
    [SerializeField] protected SystemManager systemManager;
    protected List<string> _sentences = new();    //全文を格納
    protected List<string[]> _function = new();   //機能コード(呼び出す関数を表す)を格納
    protected List<string> _names = new();        //名前を格納
    private int lineNumber = 0;                   //現在の行
    private int displayWordNumber = 0;            //表示する文字数の管理(1文字ずつ表示する用)
    private const float readTime = 0.03f;               //文字表示スピード
    private float timeCount = 0f;                 //時間保持用
    private float waitTime = 1f;                  //自動再生時の待ち時間  (この辺りはコンフィグからいじれるようにしたい)
    private string tempText;                      //表示しようとしているテキスト
    private int textLength;                       //表示しようとしているテキストの長さ
    [SerializeField] private Text mainText;
    [SerializeField] private Text nameText;
    [SerializeField] private Text logText;
    private bool isAnimation = false;
    private bool functionsOpen = false;
    public bool FunctionsOpen { set { functionsOpen = value; } }
    private bool isSpeedUp = false;
    public bool IsSpeedUp { set { isSpeedUp = value; TextFill(); } get { return isSpeedUp; } }
    private Coroutine slideCoroutine;
    private Coroutine audioCoroutine;
    private bool skip = false;  //セーブデータなどから復帰する際に任意の行から始める用

    void Start()
    {
        StartCoroutine(SaveDataCheck());
    }
    private IEnumerator SaveDataCheck()
    {
        waitTime = GameManager.instance.AutoSpeed;
        yield return null;
        //指定された行まで進む
        if (GameManager.instance.LineNumber != 0)
        {
            skip = true;
            imagesManager.Skip = true;
            imagesManager.AudioVolume(false);
            for (int i = 0; i < GameManager.instance.LineNumber - 1; i++)
            {
                GoNextLine();
                TextFill();
            }
            skip = false;
            isAnimation = false;    //関数を使わずにこちらからアニメーションオンにすることがあるため(改善検討)
            imagesManager.Skip = false;
            imagesManager.AudioVolume(true);
        }
        GoNextLine();
    }

    void Update()
    {
        if (!skip)
        {
            timeCount += Time.deltaTime;
            //ファンクションが開いているときおよび自動再生時は反応しない
            if (!functionsOpen && !isSpeedUp)
            {
                if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && lineNumber < _sentences.Count && !isAnimation)
                {
                    //表示されていない部分があったら表示
                    if (displayWordNumber < textLength)
                    {
                        TextFill();
                    }
                    //なければ次へ
                    else
                    {
                        GoNextLine();
                    }
                }
            }
            //一文字ずつ表示
            if (displayWordNumber < textLength && timeCount > readTime && !isSpeedUp)
            {
                //全角スラッシュがきたら改行
                if (tempText[displayWordNumber] == '／')
                {
                    mainText.text += '\n';
                    displayWordNumber++;
                    timeCount = 0f;
                }
                else
                {
                    mainText.text += tempText[displayWordNumber];
                    displayWordNumber++;
                    timeCount = 0f;
                }
            }
            //自動再生時
            if (isSpeedUp)
            {
                if (_sentences.Count > lineNumber && !isAnimation && timeCount > waitTime)
                {
                    GoNextLine();
                    TextFill();
                    timeCount = 0f;
                }
            }
        }
    }

    //ページ送り
    protected void GoNextLine()
    {
        if (nameText.text != "")
        {
            logText.text += nameText.text + "\n";
        }
        if (mainText.text != "")
        {
            logText.text += mainText.text + "\n\n";
        }
        SelectFunction(_function[lineNumber]);
        tempText = _sentences[lineNumber];
        textLength = tempText.Length;
        mainText.text = "";
        nameText.text = _names[lineNumber];
        displayWordNumber = 0;
        lineNumber++;
    }

    //テキストに記述した機能コードに応じて関数呼び出し(複数キャラクターの表示にも対応させたいところ)(現状だと1行の中で時間差で演出を動作させることができない)
    private void SelectFunction(string[] s)
    {
        int n = s.Length;
        for (int i = 0; i < n; i++)
        {
            switch (s[i])
            {
                case "0":
                    break;
                case "FadeOut":
                    isAnimation = true;
                    i++;
                    float fadeTime = float.Parse(s[i]);
                    i++;
                    imagesManager.FadeOutReceiver(fadeTime, s[i]);
                    break;
                case "FadeIn":
                    isAnimation = true;
                    i++;
                    fadeTime = float.Parse(s[i]);
                    i++;
                    imagesManager.FadeInReceiver(fadeTime, s[i]);
                    break;
                case "BlackOn":
                    imagesManager.BlackOn();
                    break;
                case "BlackOff":
                    imagesManager.BlackOff();
                    break;
                case "BlackOUOn":
                    imagesManager.BlackOUOn();
                    break;
                case "BlackOUOff":
                    imagesManager.BlackOUOff();
                    break;
                case "TextPanelOn":
                    imagesManager.TextPanelOn();
                    break;
                case "TextPanelOff":
                    imagesManager.TextPanelOff();
                    break;
                case "BlackHalfOpen":
                    StartCoroutine(imagesManager.BlackHalfOpen());
                    break;
                case "BlackHalfToWhite":
                    StartCoroutine(imagesManager.BlackHalfToWhite());
                    break;
                case "CharacterChange":
                    i++;
                    imagesManager.CharacterChange(s[i]);
                    break;
                case "BackgroundChange":
                    i++;
                    imagesManager.BackgroundChange(s[i]);
                    break;
                case "BGMChange":
                    i++;
                    imagesManager.BGMChange(s[i]);
                    break;
                case "CharacterMotion":
                    i++;
                    imagesManager.CharacterMotion(s[i]);
                    break;
                case "Effect":
                    i++;
                    imagesManager.Effect(s[i]);
                    break;
                case "SE":
                    i++;
                    imagesManager.SoundEffect(s[i]);
                    break;
                case "Wipe1":
                    isAnimation = true;
                    StartCoroutine(imagesManager.Wipe1());
                    break;
                case "Wipe2":
                    isAnimation = true;
                    StartCoroutine(imagesManager.Wipe2());
                    break;
                case "BackgroundSlide":
                    slideCoroutine = StartCoroutine(imagesManager.BackgroundSlide());
                    break;
                case "SlideStop":
                    if (slideCoroutine != null)
                    {
                        StopCoroutine(slideCoroutine);
                        imagesManager.SlideStop();
                    }                    
                    break;
                case "PanelVib":
                    StartCoroutine(imagesManager.PanelVib());
                    break;
                case "ZoomLook":
                    StartCoroutine(imagesManager.ZoomLook());
                    break;
                case "CharacterColor":
                    imagesManager.CharacterColor();
                    break;
                case "BackgroundColor":
                    StartCoroutine(imagesManager.BackgroundColor());
                    break;
                case "FaceChangeDelay":
                    i++;
                    float t = float.Parse(s[i]);
                    i++;
                    StartCoroutine(imagesManager.FaceChangeDelay(t, s[i]));
                    break;
                case "SEDelay":
                    i++;
                    t = float.Parse(s[i]);
                    i++;
                    StartCoroutine(imagesManager.SEDelay(t, s[i]));
                    break;
                case "CharacterRect":
                    i++;
                    int x = int.Parse(s[i]);
                    i++;
                    int y = int.Parse(s[i]);
                    imagesManager.CharacterRect(x, y);
                    break;
                case "CharacterReset":
                    imagesManager.CharacterReset();
                    break;
                case "BackgroundReset":
                    imagesManager.BackgroundReset();
                    break;
                case "VolumeFadeOut":
                    if (audioCoroutine != null)
                    {
                        StopCoroutine(audioCoroutine);
                    }
                    i++;
                    audioCoroutine = StartCoroutine(imagesManager.VolumeFadeOut(float.Parse(s[i])));
                    break;
                case "VolumeFadeIn":
                    if (audioCoroutine != null)
                    {
                        StopCoroutine(audioCoroutine);
                    }
                    i++;
                    audioCoroutine = StartCoroutine(imagesManager.VolumeFadeIn(float.Parse(s[i])));
                    break;
                case "AnimAndGoNext":
                    i++;
                    StartCoroutine(AnimationFinished(float.Parse(s[i])));
                    break;
                case "AnimationWaitSet":
                    i++;
                    StartCoroutine(AnimationWaitSet(float.Parse(s[i])));
                    break;
                case "MenuOn":
                    systemManager.MenuOn();
                    break;
                case "MenuOff":
                    systemManager.MenuOff();
                    break;
                case "TitleCoal":
                    isAnimation = true;
                    StartCoroutine(imagesManager.TitleAnimation());
                    break;
                case "Save":
                    Save();
                    break;
                case "ChangeScene":
                    imagesManager.ChangeScene();
                    break;
                default:
                    break;
            }
        }
    }

    //テキストを一気に表示する用
    protected void TextFill()
    {
        while (displayWordNumber < textLength)
        {
            //全角スラッシュがきたら改行
            if (tempText[displayWordNumber] == '／')
            {
                mainText.text += '\n';
                displayWordNumber++;
            }
            else
            {
                mainText.text += tempText[displayWordNumber];
                displayWordNumber++;
            }
        }
    }

    //アニメーションが終了したら任意の時間待った後1行進み操作可能に
    public IEnumerator AnimationFinished(float f)
    {
        //スキップ中は無視
        if (!skip)
        {
            isAnimation = true;
            yield return new WaitForSeconds(f);
            isAnimation = false;
            timeCount = 0;
            GoNextLine();
            if (isSpeedUp)
            {
                TextFill();
            }
        }
    }

    //ウィンドウ非表示を伴わないタイプのアニメーションによる操作停止用
    protected IEnumerator AnimationWaitSet(float f)
    {
        if (!skip)
        {
            isAnimation = true;
            yield return new WaitForSeconds(f);
            isAnimation = false;
        }
    }

    //シーンスキップ
    public IEnumerator SceneSkip()
    {
        imagesManager.AudioVolume(false);
        yield return new WaitForSeconds(0.15f);
        skip = true;
        imagesManager.Skip = true;
        while (lineNumber < _sentences.Count)
        {
            GoNextLine();
            TextFill();
        }
    }
    //育成に向かう
    public IEnumerator GoToGrow()
    {
        yield return new WaitForSeconds(0.15f);
        GameManager.instance.SceneName = SceneManager.GetActiveScene().name;
        GameManager.instance.LineNumber = lineNumber;
        GameManager.instance.Save();
        imagesManager.FadeOutReceiver(1, "Black");
        imagesManager.TextPanelOff();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("3DGameSelectScene");
    }
    //セーブする
    public void Save()
    {
        GameManager.instance.SceneName = SceneManager.GetActiveScene().name;
        GameManager.instance.LineNumber = lineNumber;
        GameManager.instance.Save();
    }
    //タイトルへ
    public IEnumerator GoBackTitle()
    {
        yield return new WaitForSeconds(0.15f);
        GameManager.instance.SceneName = SceneManager.GetActiveScene().name;
        GameManager.instance.LineNumber = lineNumber;
        GameManager.instance.Save();
        imagesManager.FadeOutReceiver(1, "Black");
        imagesManager.TextPanelOff();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("TitleScene");
    }
}