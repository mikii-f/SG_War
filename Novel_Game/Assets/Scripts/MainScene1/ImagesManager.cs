using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImagesManager : ImagesManagerOrigin
{
    private TextManager textManager;
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite el;
    [SerializeField] private Sprite backgroundMyRoom;
    [SerializeField] private Sprite backgroundRoad;
    [SerializeField] private Sprite backgroundCity;

    protected override void StartSet()
    {
        textManager = tManager.GetComponent<TextManager>();
        blackOverImage.color = new(0, 0, 0, 0.7f);
        blackUnderImage.color = new(0, 0, 0, 0.7f);
    }

    //黒背景が半分開く
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

    //黒背景が開けるとともに光に包まれ徐々に戻る
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

    //立ち絵関係
    public override void CharacterChange(int n)
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

    //背景切り替え
    public override void BackgroundChange(int n)
    {
        switch (n)
        {
            case 0:
                _backgroundImage.sprite = backgroundBlack;
                break;
            case 1:
                _backgroundImage.sprite = backgroundMyRoom;
                break;
            case 2:
                _backgroundImage.sprite = backgroundRoad;
                break;
            case 3:
                _backgroundImage.sprite = backgroundCity;
                break;
            default:
                break;
        }
    }

    //テキストへのアニメーション終了通知
    protected override void AnimationFinished(float waitTime)
    {
        StartCoroutine(textManager.AnimationFinished(waitTime));
    }
}
