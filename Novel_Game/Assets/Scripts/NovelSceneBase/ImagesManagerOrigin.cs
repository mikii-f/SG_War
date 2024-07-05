using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class ImagesManagerOrigin : MonoBehaviour
{
    [SerializeField] protected GameObject chapterTitle;
    [SerializeField] protected GameObject blackOver;
    [SerializeField] protected GameObject blackUnder;
    protected RectTransform bORect;
    protected RectTransform bURect;
    protected Image blackOverImage;
    protected Image blackUnderImage;
    [SerializeField] protected GameObject white;
    protected Image whiteImage;
    [SerializeField] protected GameObject textPanel;
    [SerializeField] protected GameObject tManager;
    [SerializeField] protected GameObject character1;
    protected Image _characterImage;
    protected RectTransform _characterRect;
    [SerializeField] protected GameObject background1;
    protected Image _backgroundImage;
    protected RectTransform _backgroundRect;
    [SerializeField] protected Sprite noneSprite;
    [SerializeField] protected Sprite backgroundBlack;
    // Start is called before the first frame update
    void Start()
    {
        bORect = blackOver.GetComponent<RectTransform>();
        bURect = blackUnder.GetComponent<RectTransform>();
        blackOverImage = blackOver.GetComponent<Image>();
        blackUnderImage = blackUnder.GetComponent<Image>();
        whiteImage = white.GetComponent<Image>();
        _characterImage = character1.GetComponent<Image>();
        _characterRect = character1.GetComponent<RectTransform>();
        _backgroundImage = background1.GetComponent<Image>();
        _backgroundRect = background1.GetComponent<RectTransform>();
        whiteImage.color = new(255, 255, 255, 0);
        chapterTitle.SetActive(false);
        StartSet();
    }

    public IEnumerator TitleAnimation()
    {
        yield return new WaitForSeconds(1);
        chapterTitle.SetActive(true);
        yield return new WaitForSeconds(6);
        chapterTitle.SetActive(false);
        yield return new WaitForSeconds(1);
        AnimationFinished(0);
    }

    protected IEnumerator FadeOut(float fadeTime, Image image)
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
    protected IEnumerator FadeIn(float fadeTime, Image image)
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
    //�e�L�X�g������t�F�[�h�Ώۂ�I������p
    public void FadeOutReceiver(float n, string image)
    {
        switch (image)
        {
            case "Black":
                StartCoroutine(FadeOut(n, blackOverImage));
                StartCoroutine(FadeOut(n, blackUnderImage));
                break;
            case "Character":
                StartCoroutine(FadeOut(n, _characterImage));
                break;
            default:
                StartCoroutine(FadeOut(n, blackOverImage));
                StartCoroutine(FadeOut(n, blackUnderImage));
                break;
        }
    }
    public void FadeInReceiver(float n, string image)
    {
        switch (image)
        {
            case "Black":
                StartCoroutine(FadeIn(n, blackOverImage));
                StartCoroutine(FadeIn(n, blackUnderImage));
                break;
            case "Character":
                StartCoroutine(FadeIn(n, _characterImage));
                break;
            default:
                StartCoroutine(FadeIn(n, blackOverImage));
                StartCoroutine(FadeIn(n, blackUnderImage));
                break;
        }

    }

    //���C�v(�O��)
    public IEnumerator Wipe1()
    {
        bORect.anchoredPosition = new(-1920, 270);
        bURect.anchoredPosition = new(-1920, -270);
        textPanel.SetActive(false);
        while (bORect.anchoredPosition.x < 0)
        {
            yield return null;
            Vector2 posO = bORect.anchoredPosition;
            Vector2 posU = bURect.anchoredPosition;
            posO.x += 960 * Time.deltaTime;
            posU.x += 960 * Time.deltaTime;
            bORect.anchoredPosition = posO;
            bURect.anchoredPosition = posU;
        }
        AnimationFinished(0);
    }

    //���C�v(�㔼)
    public IEnumerator Wipe2()
    {
        yield return new WaitForSeconds(0.5f);
        while (bORect.anchoredPosition.x < 1920)
        {
            yield return null;
            Vector2 posO = bORect.anchoredPosition;
            Vector2 posU = bURect.anchoredPosition;
            posO.x += 960 * Time.deltaTime;
            posU.x += 960 * Time.deltaTime;
            bORect.anchoredPosition = posO;
            bURect.anchoredPosition = posU;
        }
        yield return new WaitForSeconds(0.5f);
        textPanel.SetActive(true);
        AnimationFinished(0);
    }

    //�Y�[�����Ĕw�i���X���C�h(�����ŃY�[���{���⑬����ς�����悤�ɂ���Δėp���オ��)
    public IEnumerator BackgroundSlide()
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
    //�e�L�X�g������R���[�`�����X�g�b�v�������p
    public void SlideStop()
    {
        _backgroundRect.anchoredPosition = new(480, 0);
    }
    //�L�����N�^�[�̃T�C�Y�E�ʒu�E�����x�����Z�b�g
    public void CharacterReset()
    {
        _characterRect.localScale = new(100, 100);
        _characterRect.anchoredPosition = new(0, -360);
        _characterImage.color = Color.white;
    }

    //�w�i�̃T�C�Y����шʒu�����Z�b�g
    public void BackgroundReset()
    {
        _backgroundRect.localScale = new(100, 100);
        _backgroundRect.anchoredPosition = new Vector2(0, 0);
    }
    //�悭�g�����I�u�W�F�N�g�p���Z�b�g�֐�
    public void BlackReset()
    {
        bORect.anchoredPosition = new(0, 270);
        bURect.anchoredPosition = new(0, -270);
        blackOverImage.color = Color.white;
        blackUnderImage.color = Color.white;
    }

    //���̃I���I�t
    public void BlackOnOff(bool ToF)
    {
        blackOver.SetActive(ToF);
        blackUnder.SetActive(ToF);
    }
    //�e�L�X�g�p�l���̃I���I�t
    public void TextPanelOnOff(bool ToF)
    {
        textPanel.SetActive(ToF);
    }

    //�V�[���؂�ւ�
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //�ȉ��e�N���X�ɕK�v�����قȂ鏈���ɂ������

    //�����G�̕ύX�p(�V�[�����ƂɕK�v�ȕ������L�q)
    public abstract void CharacterChange(int n);
    //�w�i�ɂ��ē���
    public abstract void BackgroundChange(int n);
    //�V�[�����Ƃ�Start�ňقȂ鏈����(��������)�L�q���邽�߂̊֐�
    protected abstract void StartSet();
    //�A�j���[�V�����̏I�����e�e�L�X�g�}�l�[�W���[�ɓ`���邽�߂̊֐�
    protected abstract void AnimationFinished(float waitTime);
}
