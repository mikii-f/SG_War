using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImagesManager2_2 : ImagesManagerOrigin
{
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite vier_battle;
    [SerializeField] private Sprite ghost1;
    [SerializeField] private Sprite backgroundMyRoom;
    [SerializeField] private Sprite backgroundRoad;
    [SerializeField] private GameObject effectsObject;
    private Image effectsImage;
    private RectTransform effectsRect;
    [SerializeField] private Sprite swordEffect;

    protected override void StartSet()
    {
        effectsImage = effectsObject.GetComponent<Image>();
        effectsRect = effectsObject.GetComponent<RectTransform>();
    }

    //—§‚¿ŠGŠÖŒW
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
            case 21:
                _characterImage.sprite = vier_battle;
                break;
            case 101:
                _characterImage.sprite = ghost1;
                break;
            default:
                break;
        }
    }

    //”wŒiØ‚è‘Ö‚¦
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
            default:
                break;
        }
    }

    public override void Effect(int n)
    {
        if (!skip)
        {
            switch (n)
            {
                case 0:
                    StartCoroutine(SwordEffect());
                    break;
                default:
                    break;
            }
        }
    }
    private IEnumerator SwordEffect()
    {
        effectsRect.localScale = new(2, 2);
        effectsImage.sprite = swordEffect;
        StartCoroutine(FadeIn(0.5f, effectsImage));
        while (effectsRect.localScale.x > 0.5)
        {
            yield return null;
            float temp = effectsRect.localScale.x;
            temp -= 3 * Time.deltaTime;
            effectsRect.localScale = new(temp, temp);
        }
        effectsImage.sprite = noneSprite;
        effectsRect.localScale = new(1, 1);
        effectsImage.color = Color.white;
    }

    public override void ChangeScene()
    {
        SceneManager.LoadScene("3DGameScene0");
    }
}
