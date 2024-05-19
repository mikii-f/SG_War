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
    public GameObject white;
    private Image whiteImage;
    public GameObject bwCanvas;
    public GameObject textPanel;
    public GameObject text;
    private TextManager textManager;
    // Start is called before the first frame update
    void Start()
    {
        textManager = text.GetComponent<TextManager>();
        bORect = blackOver.GetComponent<RectTransform>();
        bURect = blackUnder.GetComponent<RectTransform>();
        blackOver.SetActive(false);
        blackUnder.SetActive(false);
        whiteImage = white.GetComponent<Image>();
        whiteImage.color = new(255, 255, 255, 0);
        StartCoroutine(TitleFinish());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator TitleFinish()
    {
        yield return new WaitForSeconds(6);
        chapterTitle.SetActive(false);
        blackOver.SetActive(true);
        blackUnder.SetActive(true);
        yield return new WaitForSeconds(1);
        textPanel.SetActive(true);
        textManager.AnimationFinished();
    }

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

    public IEnumerator BlackHalfToWhite()
    {
        StartCoroutine(FadeOut(1.2f));
        while (bORect.anchoredPosition.y < 810)
        {
            yield return null;
            Vector2 posO = bORect.anchoredPosition;
            posO.y += 135 * Time.deltaTime;
            bORect.anchoredPosition = posO;
            bURect.anchoredPosition = -posO;
        }
        StartCoroutine(FadeIn(2f));
    }
    IEnumerator FadeOut(float fadeTime)
    {
        float waitTime = 0.1f;
        float alphaChangeAmount = 255.0f / (fadeTime/waitTime);
        for (float alpha = 0.0f; alpha <= 255.0f; alpha += alphaChangeAmount)
        {
            yield return new WaitForSeconds(waitTime);
            Color newColor = whiteImage.color;
            newColor.a = alpha / 255.0f;
            whiteImage.color = newColor;
        }
    }
    IEnumerator FadeIn(float fadeTime)
    {
        float waitTime = 0.1f;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alphaChangeAmount)
        {
            yield return new WaitForSeconds(waitTime);
            Color newColor = whiteImage.color;
            newColor.a = alpha / 255.0f;
            whiteImage.color = newColor;
        }
    }
}
