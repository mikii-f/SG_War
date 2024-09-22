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

    //���C�v�Ȃǂ̏����Ɣ�ׂāA�����e���J�ڂ̕����u������ۂ��v�C�����邽��yield return null�ɂ��Ȃ�
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
        //�X�L�b�v���̏ꍇ���ʂ̂ݔ��f
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
        //�X�L�b�v���̏ꍇ���ʂ̂ݔ��f
        else
        {
            Color newColor = image.color;
            newColor.a = 0;
            image.color = newColor;
        }
    }
    //�e�L�X�g������t�F�[�h�Ώۂ�I������p
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

    //�R���[�`�����Ăяo�����̂΂���ɂȂ�Ȃ�ʂ̊֐������ɂ��������������H
    public void CharacterMotion(string s)
    {
        switch (s)
        {
            case "Jump":
                //���W�ړ��ƃt�F�[�h�ŃW�����v��\��
                StartCoroutine(FadeIn(0.5f, _characterImage));
                Vector2 temp = _characterRect.anchoredPosition;
                temp.y += 200;
                _characterRect.anchoredPosition = temp;
                break;
            case "Bounce":
                //�҂��҂�񒵂˂�
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
    //���C�v�͂ЂƂ܂Ƃ܂�Ŏg����͂������A������r���ŃZ�[�u����Ă������悤�ɂ���
    //���C�v(�O��)
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

    //���C�v(�㔼)
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

    //���w�i�������J��
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
        //�X�L�b�v���̏ꍇ���ʂ̂ݔ��f
        else
        {
            blackOverImage.color = Color.black;
            blackUnderImage.color = Color.black;
            bORect.anchoredPosition = new (0, 540);
            bURect.anchoredPosition = new (0, -540);
        }
    }
    //���w�i���J����ƂƂ��Ɍ��ɕ�܂ꏙ�X�ɖ߂�
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
        //�X�L�b�v���̏ꍇ���ʂ̂ݔ��f
        else
        {
            blackOverImage.color = Color.clear;
            blackUnderImage.color = Color.clear;
            bORect.anchoredPosition = new(0, 270);
            bURect.anchoredPosition = new(0, -270);
        }
    }

    //�Y�[�����Ĕw�i���X���C�h(�����ŃY�[���{���⑬����ς�����悤�ɂ���Δėp���オ��)
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
    //�e�L�X�g������R���[�`�����X�g�b�v�������p
    public void SlideStop()
    {
        _backgroundRect.anchoredPosition = new(480, 0);
    }
    //�b���Ă��Ȃ��L�����N�^�[�̈Ó]�p
    public void CharacterColor()
    {
        _characterImage.color = new(0.5f, 0.5f, 0.5f, 1);
    }
    //�w�i�̈Ó]�p(����g����ʂ͑����Ȃ����A�l�������Ŏw�肷��̂��A��)
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
    //1���̊Ԃŕ\���؂�ւ���p
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
    //�L�����N�^�[�̈ʒu��ݒ�
    public void CharacterRect(int x, int y)
    {
        _characterRect.anchoredPosition = new(x, y);
    }
    //�L�����N�^�[�̃T�C�Y�E�ʒu�E�����x�����Z�b�g
    public void CharacterReset()
    {
        _characterRect.localScale = new(1, 1);
        _characterRect.anchoredPosition = new(0, -360);
        _characterImage.color = Color.white;
    }
    //�w�i�̃T�C�Y����шʒu�����Z�b�g
    public void BackgroundReset()
    {
        _backgroundRect.localScale = new(1, 1);
        _backgroundRect.anchoredPosition = new Vector2(0, 0);
    }
    //���̃I���I�t(�ʂ̕��@��T���������c�c)
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
    //�e�L�X�g�p�l���̃I���I�t
    public void TextPanelOn()
    {
        textPanel.SetActive(true);
    }
    public void TextPanelOff()
    {
        textPanel.SetActive(false);
    }
    //BGM�̃t�F�[�h
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

    //�V�[���؂�ւ�
    public abstract void ChangeScene();

    //�A�j���[�V�����̏I�����e�e�L�X�g�}�l�[�W���[�ɓ`���邽�߂̊֐�
    private void AnimationFinished(float waitTime)
    {
        StartCoroutine(textManager.AnimationFinished(waitTime));
    }

    //�ȉ��e�N���X�ɕK�v�����قȂ鏈���ɂ������(��ɑS�ẴV�[���ŉ摜���A�^�b�`���Ȃ��ėǂ��悤��)
    //�悭�l����Ή摜�Ȃǂ̓A�^�b�`���Ȃ��Ă��G���[�ɂȂ�Ȃ��̂�����債����Ԃł͂Ȃ��������c�c

    //�����G�̕ύX�p(�V�[�����ƂɕK�v�ȕ������L�q)
    public abstract void CharacterChange(string image);
    //�w�i�ɂ��ē���
    public abstract void BackgroundChange(string image);
    //BGM�ɂ��ē���
    public abstract void BGMChange(string bgm);
    //�G�t�F�N�g
    public abstract void Effect(string image);
    //SE
    public abstract void SoundEffect(string se);
    //�V�[�����Ƃ�Start�ňقȂ鏈����(��������)�L�q���邽�߂̊֐�
    protected abstract void StartSet();
}
