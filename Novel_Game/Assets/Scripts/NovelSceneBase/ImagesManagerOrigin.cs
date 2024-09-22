using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class ImagesManagerOrigin : MonoBehaviour
{
    [SerializeField] private GameObject chapterTitle;
    [SerializeField] private GameObject blackOver;
    [SerializeField] private GameObject blackUnder;
    [SerializeField] private GameObject blackAll;
    private RectTransform bORect;
    private RectTransform bURect;
    private RectTransform bARect;
    private Image blackOverImage;
    private Image blackUnderImage;
    protected Image blackAllImage;
    [SerializeField] private GameObject white;
    private Image whiteImage;
    [SerializeField] private GameObject textPanel;
    [SerializeField] private TextManagerOrigin textManager;
    [SerializeField] private GameObject character1;
    protected Image _characterImage;
    protected RectTransform _characterRect;
    [SerializeField] private GameObject background1;
    protected Image _backgroundImage;
    protected RectTransform _backgroundRect;
    [SerializeField] protected Sprite noneSprite;
    [SerializeField] protected Sprite backgroundBlack;
    protected AudioSource audioSource;
    [SerializeField] protected AudioSource seSource;
    protected bool skip = false;
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
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.instance.BgmVolume;
        seSource.volume = GameManager.instance.SeVolume;
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
            BGMChange("Chapter");
            StartCoroutine(VolumeFadeIn(0));
            yield return new WaitForSeconds(6);
            chapterTitle.SetActive(false);
            StartCoroutine(VolumeFadeOut(0.2f));
            yield return new WaitForSeconds(1);
            AnimationFinished(0);
        }
    }

    //ワイプなどの処理と比べて、少し粗い遷移の方が「それっぽい」気がするためyield return nullにしない
    protected IEnumerator FadeOut(float fadeTime, Image image)
    {
        if (!skip)
        {
            Color temp = image.color;
            temp.a = 0;
            image.color = temp;
            while (image.color.a < 1)
            {
                yield return new WaitForSeconds(0.1f);
                temp = image.color;
                temp.a = Mathf.Min(1, temp.a + 0.1f / fadeTime);
                image.color = temp;
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
            Color temp = image.color;
            temp.a = 1;
            image.color = temp;
            while (image.color.a > 0)
            {
                yield return new WaitForSeconds(0.1f);
                temp = image.color;
                temp.a = Mathf.Max(0, temp.a - 0.1f / fadeTime);
                image.color = temp;
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
            case "White":
                StartCoroutine(FadeOut(n, whiteImage));
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
            case "White":
                StartCoroutine(FadeIn(n, whiteImage));
                break;
            case "Character":
                StartCoroutine(FadeIn(n, _characterImage));
                break;
            default:
                StartCoroutine(FadeIn(n, blackAllImage));
                break;
        }

    }

    //コルーチンを呼び出すものばかりになるなら個別の関数だけにした方がいいか？
    public void CharacterMotion(string s)
    {
        switch (s)
        {
            case "Jump":
                //座標移動とフェードでジャンプを表す
                StartCoroutine(FadeIn(0.5f, _characterImage));
                Vector2 temp = _characterRect.anchoredPosition;
                temp.y += 200;
                _characterRect.anchoredPosition = temp;
                break;
            case "Bounce":
                //ぴょんぴょん跳ねる
                StartCoroutine(Bounce());
                break;
            case "Vibe":
                StartCoroutine(Vibe());
                break;
            case "BackSlide":
                StartCoroutine(BackSlide());
                break;
            case "HorizontalSlide":
                StartCoroutine(HorizontalSlide());
                break;
            case "Approach":
                StartCoroutine(Approach());
                break;
            default:
                break;
        }
    }
    private IEnumerator Bounce()
    {
        if (!skip)
        {
            while (_characterRect.anchoredPosition.y < -310)
            {
                Vector2 temp = _characterRect.anchoredPosition;
                temp.y += 500 * Time.deltaTime;
                _characterRect.anchoredPosition = temp;
                yield return null;
            }
            while (_characterRect.anchoredPosition.y > -360)
            {
                Vector2 temp = _characterRect.anchoredPosition;
                temp.y -= 500 * Time.deltaTime;
                _characterRect.anchoredPosition = temp;
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            while (_characterRect.anchoredPosition.y < -310)
            {
                Vector2 temp = _characterRect.anchoredPosition;
                temp.y += 500 * Time.deltaTime;
                _characterRect.anchoredPosition = temp;
                yield return null;
            }
            while (_characterRect.anchoredPosition.y > -360)
            {
                Vector2 temp = _characterRect.anchoredPosition;
                temp.y -= 500 * Time.deltaTime;
                _characterRect.anchoredPosition = temp;
                yield return null;
            }
        }
        _characterRect.anchoredPosition = new(0, -360);
    }
    private IEnumerator Vibe()
    {
        if (!skip)
        {
            Vector2 temp = _characterRect.anchoredPosition;
            temp.x += 20;
            _characterRect.anchoredPosition = temp;
            yield return new WaitForSeconds(0.1f);
            temp.x -= 40;
            _characterRect.anchoredPosition = temp;
            yield return new WaitForSeconds(0.1f);
            temp.x += 40;
            _characterRect.anchoredPosition = temp;
            yield return new WaitForSeconds(0.1f);
            temp.x -= 40;
            _characterRect.anchoredPosition = temp;
            yield return new WaitForSeconds(0.1f);
            temp.x = 0;
            _characterRect.anchoredPosition = temp;
        }
    }
    private IEnumerator BackSlide()
    {
        if (!skip)
        {
            StartCoroutine(FadeOut(0.5f, _characterImage));
            _characterRect.localScale = new(2, 2);
            while (_characterRect.localScale.x > 1)
            {
                yield return null;
                float temp = _characterRect.localScale.x;
                temp -= 2 * Time.deltaTime;
                _characterRect.localScale = new(temp, temp);
            }
            _characterRect.localScale = new(1, 1);
        }
    }
    private IEnumerator HorizontalSlide()
    {
        if (!skip)
        {
            _characterRect.anchoredPosition = new(960, -360);
            StartCoroutine(FadeOut(0.5f, _characterImage));
            while (_characterRect.anchoredPosition.x > 0)
            {
                yield return null;
                Vector2 temp = _characterRect.anchoredPosition;
                temp.x -= 1920 * Time.deltaTime;
                _characterRect.anchoredPosition = temp;
            }
        }
        _characterRect.anchoredPosition = new(0, -360);
    }
    private IEnumerator Approach()
    {
        if (!skip)
        {
            yield return new WaitForSeconds(0.2f);
            while (_characterRect.localScale.x < 1.3f)
            {
                float temp = _characterRect.localScale.x;
                Vector2 temp2 = _characterRect.anchoredPosition;
                temp += 1.5f * Time.deltaTime;
                temp2.y -= 1000 * Time.deltaTime;
                _characterRect.localScale = new(temp, temp);
                _characterRect.anchoredPosition = temp2;
                yield return null;
            }
        }
        _characterRect.localScale = new(1.3f, 1.3f);
        _characterRect.anchoredPosition = new(0, -560);
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
                pos.x = Mathf.Min(0, pos.x + 960 * Time.deltaTime);
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
            while (bARect.anchoredPosition.x < 1920)
            {
                yield return null;
                Vector2 pos = bARect.anchoredPosition;
                pos.x = Mathf.Min(1920, pos.x + 960 * Time.deltaTime);
                bARect.anchoredPosition = pos;
            }
            blackAllImage.color = Color.clear;
            bARect.anchoredPosition = new(0, 0);
            yield return new WaitForSeconds(0.5f);
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
                temp += 48 * Time.deltaTime;
                _backgroundRect.anchoredPosition = new(temp, 0);
            }
            _backgroundRect.anchoredPosition = new(480, 0);
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
    //話していないキャラクターの暗転用
    public void CharacterColor()
    {
        _characterImage.color = new(0.5f, 0.5f, 0.5f, 1);
    }
    //背景の暗転用(現状使う場面は多くないが、値を引数で指定するのもアリ)
    public IEnumerator BackgroundColor()
    {
        float c = 1;
        while (c > 0.3f)
        {
            c -= 0.7f * Time.deltaTime;
            _backgroundImage.color = new(c, c, c, 1);
            yield return null;
        }
    }
    //1文の間で表情を切り替える用
    public IEnumerator FaceChangeDelay(float t, string image)
    {
        if (!skip)
        {
            yield return new WaitForSeconds(t);
            CharacterChange(image);
        }
        else
        {
            CharacterChange(image);
        }
    }
    //キャラクターの位置を設定
    public void CharacterRect(int x, int y)
    {
        _characterRect.anchoredPosition = new(x, y);
    }
    //キャラクターのサイズ・位置・透明度をリセット
    public void CharacterReset()
    {
        _characterRect.localScale = new(1, 1);
        _characterRect.anchoredPosition = new(0, -360);
        _characterImage.color = Color.white;
    }
    //背景のサイズおよび位置をリセット
    public void BackgroundReset()
    {
        _backgroundRect.localScale = new(1, 1);
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
    //BGMのフェード
    public IEnumerator VolumeFadeOut(float fadeTime)
    {
        if (!skip || fadeTime != 0)
        {
            while (audioSource.volume > 0)
            {
                float v = audioSource.volume;
                v = Mathf.Max(0, v - GameManager.instance.BgmVolume * Time.deltaTime / fadeTime);
                audioSource.volume = v;
                yield return null;
            }
        }
        else
        {
            audioSource.volume = 0;
        }
    }
    public IEnumerator VolumeFadeIn(float fadeTime)
    {
        float targetVolume = GameManager.instance.BgmVolume;
        if (!skip || fadeTime != 0)
        {
            while (audioSource.volume < targetVolume)
            {
                float v = audioSource.volume;
                v = Mathf.Min(targetVolume, v + targetVolume * Time.deltaTime / fadeTime);
                audioSource.volume = v;
                yield return null;
            }
        }
        else
        {
            audioSource.volume = targetVolume;
        }
    }

    //シーン切り替え
    public abstract void ChangeScene();

    //アニメーションの終了を各テキストマネージャーに伝えるための関数
    private void AnimationFinished(float waitTime)
    {
        StartCoroutine(textManager.AnimationFinished(waitTime));
    }

    //以下各クラスに必要だが異なる処理にするもの(主に全てのシーンで画像をアタッチしなくて良いように)
    //よく考えれば画像などはアタッチしなくてもエラーにならないのだから大した手間ではなかったが……

    //立ち絵の変更用(シーンごとに必要な分だけ記述)
    public abstract void CharacterChange(string image);
    //背景について同上
    public abstract void BackgroundChange(string image);
    //BGMについて同上
    public abstract void BGMChange(string bgm);
    //エフェクト
    public abstract void Effect(string image);
    //SE
    public abstract void SoundEffect(string se);
    //シーンごとにStartで異なる処理を(差分だけ)記述するための関数
    protected abstract void StartSet();
}
