using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class ImagesManagerOrigin : MonoBehaviour
{
    [SerializeField] protected GameObject chapterTitle;
    [SerializeField] private GameObject blackOver;
    [SerializeField] private GameObject blackUnder;
    [SerializeField] private GameObject blackAll;
    protected RectTransform bORect;
    protected RectTransform bURect;
    protected RectTransform bARect;
    protected Image blackOverImage;
    protected Image blackUnderImage;
    protected Image blackAllImage;
    [SerializeField] private GameObject white;
    protected Image whiteImage;
    [SerializeField] protected GameObject textPanel;
    [SerializeField] private TextManagerOrigin textManager;
    [SerializeField] private GameObject character1;
    protected Image _characterImage;
    protected RectTransform _characterRect;
    [SerializeField] private GameObject background1;
    protected Image _backgroundImage;
    protected RectTransform _backgroundRect;
    [SerializeField] protected Sprite noneSprite;
    [SerializeField] protected Sprite backgroundBlack;
    private bool skip = false;
    public bool Skip { set { skip = value; } }
    // Start is called before the first frame update
    void Start()
    {
        bORect = blackOver.GetComponent<RectTransform>();
        bURect = blackUnder.GetComponent<RectTransform>();
        bARect = blackAll.GetComponent<RectTransform>();
        blackOverImage = blackOver.GetComponent<Image>();
        blackUnderImage = blackUnder.GetComponent<Image>();
        blackAllImage = blackAll.GetComponent<Image>();
        whiteImage = white.GetComponent<Image>();
        _characterImage = character1.GetComponent<Image>();
        _characterRect = character1.GetComponent<RectTransform>();
        _backgroundImage = background1.GetComponent<Image>();
        _backgroundRect = background1.GetComponent<RectTransform>();
        whiteImage.color = new(1, 1, 1, 0);
        blackOverImage.color = Color.clear;
        blackUnderImage.color = Color.clear;
        chapterTitle.SetActive(false);
        textPanel.SetActive(false);
        StartSet();
    }

    public IEnumerator TitleAnimation()
    {
        if (!skip)
        {
            yield return new WaitForSeconds(1);
            chapterTitle.SetActive(true);
            yield return new WaitForSeconds(6);
            chapterTitle.SetActive(false);
            yield return new WaitForSeconds(1);
            AnimationFinished(0);
        }
    }

    protected IEnumerator FadeOut(float fadeTime, Image image)
    {
        if (!skip)
        {
            float waitTime = 0.1f;
            float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
            for (float alpha = 0.0f; alpha <= 255.0f; alpha += alphaChangeAmount)
            {
                Color newColor = image.color;
                newColor.a = alpha / 255.0f;
                image.color = newColor;
                yield return new WaitForSeconds(waitTime);
            }
        }
        //スキップ中の場合結果のみ反映
        else
        {
            Color newColor = image.color;
            newColor.a = 1;
            image.color = newColor;
        }
    }
    protected IEnumerator FadeIn(float fadeTime, Image image)
    {
        if (!skip)
        {
            float waitTime = 0.1f;
            float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
            for (float alpha = 255.0f; alpha >= 0f; alpha -= alphaChangeAmount)
            {
                Color newColor = image.color;
                newColor.a = alpha / 255.0f;
                image.color = newColor;
                yield return new WaitForSeconds(waitTime);
            }
        }
        //スキップ中の場合結果のみ反映
        else
        {
            Color newColor = image.color;
            newColor.a = 0;
            image.color = newColor;
        }
    }
    //テキスト側からフェード対象を選択する用
    public void FadeOutReceiver(float n, string image)
    {
        switch (image)
        {
            case "Black":
                StartCoroutine(FadeOut(n, blackAllImage));
                break;
            case "Character":
                StartCoroutine(FadeOut(n, _characterImage));
                break;
            default:
                StartCoroutine(FadeOut(n, blackAllImage));
                break;
        }
    }
    public void FadeInReceiver(float n, string image)
    {
        switch (image)
        {
            case "Black":
                StartCoroutine(FadeIn(n, blackAllImage));
                break;
            case "Character":
                StartCoroutine(FadeIn(n, _characterImage));
                break;
            default:
                StartCoroutine(FadeIn(n, blackAllImage));
                break;
        }

    }

    //ワイプはひとまとまりで使われるはずだが、万が一途中でセーブされてもいいようにする
    //ワイプ(前半)
    public IEnumerator Wipe1()
    {
        if (!skip)
        {
            bARect.anchoredPosition = new(-1920, 0);
            blackAllImage.color = Color.black;
            textPanel.SetActive(false);
            while (bARect.anchoredPosition.x < 0)
            {
                yield return null;
                Vector2 pos = bARect.anchoredPosition;
                pos.x += 960 * Time.deltaTime;
                bARect.anchoredPosition = pos;
            }
            AnimationFinished(0);
        }
        else
        {
            blackAllImage.color = Color.black;
            textPanel.SetActive(false);
            bARect.anchoredPosition = Vector2.zero;
        }
    }

    //ワイプ(後半)
    public IEnumerator Wipe2()
    {
        if (!skip)
        {
            yield return new WaitForSeconds(0.5f);
            while (bARect.anchoredPosition.x < 1930)
            {
                yield return null;
                Vector2 pos = bARect.anchoredPosition;
                pos.x += 960 * Time.deltaTime;
                bARect.anchoredPosition = pos;
            }
            yield return new WaitForSeconds(0.5f);
            blackAllImage.color = Color.clear;
            bARect.anchoredPosition = new(0, 0);
            textPanel.SetActive(true);
            AnimationFinished(0);
        }
        else
        {
            blackAllImage.color = Color.clear;
            textPanel.SetActive(true);
            bARect.anchoredPosition = new(0, 0);
        }
    }

    //黒背景が半分開く
    public IEnumerator BlackHalfOpen()
    {
        if (!skip)
        {
            blackOverImage.color = Color.black;
            blackUnderImage.color = Color.black;
            while (bORect.anchoredPosition.y < 540)
            {
                yield return null;
                Vector2 posO = bORect.anchoredPosition;
                posO.y += 135 * Time.deltaTime;
                bORect.anchoredPosition = posO;
                bURect.anchoredPosition = -posO;
            }
        }
        //スキップ中の場合結果のみ反映
        else
        {
            blackOverImage.color = Color.black;
            blackUnderImage.color = Color.black;
            bORect.anchoredPosition = new (0, 540);
            bURect.anchoredPosition = new (0, -540);
        }
    }
    //黒背景が開けるとともに光に包まれ徐々に戻る
    public IEnumerator BlackHalfToWhite()
    {
        if (!skip)
        {
            StartCoroutine(FadeOut(1.2f, whiteImage));
            while (bORect.anchoredPosition.y < 810)
            {
                yield return null;
                Vector2 posO = bORect.anchoredPosition;
                posO.y += 135 * Time.deltaTime;
                bORect.anchoredPosition = posO;
                bURect.anchoredPosition = -posO;
            }
            StartCoroutine(FadeIn(2f, whiteImage));
            blackOverImage.color = Color.clear;
            blackUnderImage.color = Color.clear;
            bORect.anchoredPosition = new(0, 270);
            bURect.anchoredPosition = new(0, -270);
        }
        //スキップ中の場合結果のみ反映
        else
        {
            blackOverImage.color = Color.clear;
            blackUnderImage.color = Color.clear;
            bORect.anchoredPosition = new(0, 270);
            bURect.anchoredPosition = new(0, -270);
        }
    }

    //ズームして背景をスライド(引数でズーム倍率や速さを変えられるようにすれば汎用性上がる)
    public IEnumerator BackgroundSlide()
    {
        if (!skip)
        {
            _backgroundRect.localScale *= 1.5f;
            _backgroundRect.anchoredPosition = new(-480, 0);
            while (_backgroundRect.anchoredPosition.x < 480)
            {
                yield return null;
                float temp = _backgroundRect.anchoredPosition.x;
                temp += 96 * Time.deltaTime;
                _backgroundRect.anchoredPosition = new(temp, 0);
            }
        }
        else
        {
            _backgroundRect.localScale *= 1.5f;
            _backgroundRect.anchoredPosition = new(480, 0);
        }
    }
    //テキスト側からコルーチンをストップした時用
    public void SlideStop()
    {
        _backgroundRect.anchoredPosition = new(480, 0);
    }

    //キャラクターのサイズ・位置・透明度をリセット
    public void CharacterReset()
    {
        _characterRect.localScale = new(100, 100);
        _characterRect.anchoredPosition = new(0, -360);
        _characterImage.color = Color.white;
    }
    //背景のサイズおよび位置をリセット
    public void BackgroundReset()
    {
        _backgroundRect.localScale = new(100, 100);
        _backgroundRect.anchoredPosition = new Vector2(0, 0);
    }
    //黒のオンオフ(別の方法を探したいが……)
    public void BlackOn()
    {
        blackAllImage.color = Color.black;
    }
    public void BlackOff()
    {
        blackAllImage.color = Color.clear;
    }
    public void BlackOUOn()
    {
        blackOverImage.color = Color.black;
        blackUnderImage.color = Color.black;
    }
    public void BlackOUOff()
    {
        blackOverImage.color = Color.clear;
        blackUnderImage.color = Color.clear;
    }
    //テキストパネルのオンオフ
    public void TextPanelOn()
    {
        textPanel.SetActive(true);
    }
    public void TextPanelOff()
    {
        textPanel.SetActive(false);
    }

    //シーン切り替え(進行度を自動で保存する)
    public void ChangeScene(string sceneName)
    {
        GameManager.instance.SceneName = SceneManager.GetActiveScene().name;
        GameManager.instance.LineNumber = 0;
        GameManager.instance.Save();
        SceneManager.LoadScene(sceneName);
    }

    //アニメーションの終了を各テキストマネージャーに伝えるための関数
    protected void AnimationFinished(float waitTime)
    {
        StartCoroutine(textManager.AnimationFinished(waitTime));
    }

    //以下各クラスに必要だが異なる処理にするもの

    //立ち絵の変更用(シーンごとに必要な分だけ記述)
    public abstract void CharacterChange(int n);
    //背景について同上
    public abstract void BackgroundChange(int n);
    //シーンごとにStartで異なる処理を(差分だけ)記述するための関数
    protected abstract void StartSet();
}
