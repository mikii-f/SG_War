using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImagesManager2_2 : ImagesManagerOrigin
{
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite vier2;
    [SerializeField] private Sprite vier4;
    [SerializeField] private Sprite vier5;
    [SerializeField] private Sprite vier8;
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
    public override void CharacterChange(string image)
    {
        switch (image)
        {
            case "transparent":
                _characterImage.sprite = noneSprite;
                break;
            case "vier":
                _characterImage.sprite = vier;
                break;
            case "vier2":
                _characterImage.sprite = vier2;
                break;
            case "vier4":
                _characterImage.sprite = vier4;
                break;
            case "vier5":
                _characterImage.sprite = vier5;
                break;
            case "vier8":
                _characterImage.sprite = vier8;
                break;
            case "vier_battle":
                _characterImage.sprite = vier_battle;
                break;
            case "Ghost1":
                _characterImage.sprite = ghost1;
                break;
            default:
                break;
        }
    }

    //”wŒiØ‚è‘Ö‚¦
    public override void BackgroundChange(string image)
    {
        switch (image)
        {
            case "Black":
                _backgroundImage.sprite = backgroundBlack;
                break;
            case "MyRoom":
                _backgroundImage.sprite = backgroundMyRoom;
                break;
            case "Road":
                _backgroundImage.sprite = backgroundRoad;
                break;
            default:
                break;
        }
    }

    public override void Effect(string image)
    {
        if (!skip)
        {
            switch (image)
            {
                case "Sword":
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
