using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImagesManager3_2 : ImagesManagerOrigin
{
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite vier2;
    [SerializeField] private Sprite vier4;
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
                _characterImage.sprite = vier2;
                break;
            case 4:
                _characterImage.sprite = vier4;
                break;
            case 8:
                _characterImage.sprite = vier8;
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
            Vector3 temp2 = effectsRect.localRotation.eulerAngles;
            temp -= 3 * Time.deltaTime;
            temp2.z += 1440 * Time.deltaTime;
            effectsRect.localScale = new(temp, temp);
            effectsRect.localRotation = Quaternion.Euler(temp2);
        }
        effectsImage.sprite = noneSprite;
        effectsImage.color = Color.white;
        effectsRect.localScale = new(1, 1);
    }

    public override void ChangeScene()
    {
        GameManager.instance.LineNumber = 0;
        SceneManager.LoadScene("MainScene4");
    }
}
