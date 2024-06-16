using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImagesManager : MonoBehaviour
{
    public GameObject chapterTitle;
    public GameObject blackOver;
    public GameObject blackUnder;
    private RectTransform bORect;
    private RectTransform bURect;
    private Image blackOverImage;
    private Image blackUnderImage;
    public GameObject white;
    private Image whiteImage;
    public GameObject bwCanvas;
    public GameObject textPanel;
    public GameObject tManager;
    private TextManager textManager;
    public GameObject character1;
    private Image _characterImage; 
    public Sprite noneSprite;
    public Sprite vier;
    public Sprite el;
    public GameObject background;
    private Image _backgroundImage;
    public Sprite backgroundBlack;
    public Sprite backgroundImage1;
    public Sprite backgroundImage2;
    public Sprite backgroundImage3;

    // Start is called before the first frame update
    void Start()
    {
        textManager = tManager.GetComponent<TextManager>();
        bORect = blackOver.GetComponent<RectTransform>();
        bURect = blackUnder.GetComponent<RectTransform>();
        blackOverImage = blackOver.GetComponent<Image>();
        blackUnderImage = blackUnder.GetComponent<Image>();
        _characterImage = character1.GetComponent<Image>();
        _backgroundImage = background.GetComponent<Image>();
        blackOverImage.color = new(0, 0, 0, 0.7f);
        blackUnderImage.color = new(0, 0, 0, 0.7f);
        whiteImage = white.GetComponent<Image>();
        whiteImage.color = new(255, 255, 255, 0);
        chapterTitle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //èÕÉ^ÉCÉgÉãÇÃââèo
    public IEnumerator TitleAnimation()
    {
        StartCoroutine(FadeOut(2f, blackOverImage));
        StartCoroutine(FadeOut(2f, blackUnderImage));
        yield return new WaitForSeconds(3f);
        textPanel.SetActive(false);
        CharacterChange(0);
        blackOver.SetActive(false);
        blackUnder.SetActive(false);
        yield return new WaitForSeconds(1f);
        chapterTitle.SetActive(true);
        yield return new WaitForSeconds(6);
        chapterTitle.SetActive(false);
        yield return new WaitForSeconds(1);
        textPanel.SetActive(true);
        textManager.AnimationFinished();
    }

    //çïÇÃÉIÉìÉIÉt(ç°ÇÕÉIÉìÇæÇØ)
    public void BlackOnOff()
    {
        blackOver.SetActive(true);
        blackUnder.SetActive(true);
    }

    //çïîwåiÇ™îºï™äJÇ≠
    public IEnumerator BlackHalfOpen()
    {
        while (bORect.anchoredPosition.y < 540)
        {
            yield return null;
            Vector2 posO = bORect.anchoredPosition;
            posO.y += 135 * Time.deltaTime;
            bORect.anchoredPosition = posO;
            bURect.anchoredPosition = -posO;
        }
    }

    //çïîwåiÇ™äJÇØÇÈÇ∆Ç∆Ç‡Ç…åıÇ…ïÔÇ‹ÇÍèôÅXÇ…ñﬂÇÈ
    public IEnumerator BlackHalfToWhite()
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
    }
    IEnumerator FadeOut(float fadeTime, Image image)
    {
        float waitTime = 0.1f;
        float alphaChangeAmount = 255.0f / (fadeTime/waitTime);
        for (float alpha = 0.0f; alpha <= 255.0f; alpha += alphaChangeAmount)
        {
            yield return new WaitForSeconds(waitTime);
            Color newColor = whiteImage.color;
            newColor.a = alpha / 255.0f;
            image.color = newColor;
        }
    }
    IEnumerator FadeIn(float fadeTime, Image image)
    {
        float waitTime = 0.1f;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alphaChangeAmount)
        {
            yield return new WaitForSeconds(waitTime);
            Color newColor = whiteImage.color;
            newColor.a = alpha / 255.0f;
            image.color = newColor;
        }
    }

    //óßÇøäGä÷åW
    public void CharacterChange(int n)
    {
        switch (n)
        {
            case 0:
                _characterImage.sprite = noneSprite;
                break;
            case 1:
                _characterImage.sprite = vier;
                break;
            case 2:
                _characterImage.sprite = el;
                break;
            default:
                break;
        }
    }

    //îwåiêÿÇËë÷Ç¶
    public void BackGroundChange(int n)
    {
        switch (n)
        {
            case 0:
                _backgroundImage.sprite = backgroundBlack;
                break;
            case 1:
                _backgroundImage.sprite = backgroundImage1;
                break;
            case 2:
                _backgroundImage.sprite = backgroundImage2;
                break;
            case 3:
                _backgroundImage.sprite = backgroundImage3;
                break;
            default:
                break;
        }
    }
}
