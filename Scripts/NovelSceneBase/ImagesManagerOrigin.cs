using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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

    //���Ăǂ����Ō�������𗬗p�������Ă���C�����邽�߂��낻�뎩�����ŏ��������Ă���������
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
    public IEnumerator FaceChangeDelay(float t, int n)
    {
        if (!skip)
        {
            yield return new WaitForSeconds(t);
            CharacterChange(n);
        }
        else
        {
            CharacterChange(n);
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
        _backgroundRect.localScale = new(100, 100);
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

    //�V�[���؂�ւ�
    public abstract void ChangeScene();

    //�A�j���[�V�����̏I�����e�e�L�X�g�}�l�[�W���[�ɓ`���邽�߂̊֐�
    protected void AnimationFinished(float waitTime)
    {
        StartCoroutine(textManager.AnimationFinished(waitTime));
    }

    //�ȉ��e�N���X�ɕK�v�����قȂ鏈���ɂ������(��ɑS�ẴV�[���ŉ摜���A�^�b�`���Ȃ��ėǂ��悤��)

    //�����G�̕ύX�p(�V�[�����ƂɕK�v�ȕ������L�q)
    public abstract void CharacterChange(int n);
    //�w�i�ɂ��ē���
    public abstract void BackgroundChange(int n);
    //���ʂŎg���G�t�F�N�g�������Ă�����ASprite���������ŗ^����Ȃ�O�Őݒ肷��Ȃ�őΉ����邱�Ƃɂ��ċ@�\�͋��ʉ����������ǂ���
    public abstract void Effect(int n);
    //�V�[�����Ƃ�Start�ňقȂ鏈����(��������)�L�q���邽�߂̊֐�
    protected abstract void StartSet();
}