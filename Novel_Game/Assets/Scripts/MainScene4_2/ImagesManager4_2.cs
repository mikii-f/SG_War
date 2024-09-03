using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImagesManager4_2 : ImagesManagerOrigin
{
    [SerializeField] private Sprite vier_battle;
    [SerializeField] private Sprite vier_battle2;
    [SerializeField] private Sprite vier_battle3;
    [SerializeField] private Sprite vier_battle4;
    [SerializeField] private Sprite vier_battle5;
    [SerializeField] private Sprite vier_battle6;
    [SerializeField] private Sprite vier_battle7;
    [SerializeField] private Sprite vier_battle8;
    [SerializeField] private Sprite el_battle;
    [SerializeField] private Sprite el_battle2;
    [SerializeField] private Sprite el_battle3;
    [SerializeField] private Sprite el_battle4;
    [SerializeField] private Sprite el_battle5;
    [SerializeField] private Sprite el_enemy;
    [SerializeField] private Sprite ghost1;
    [SerializeField] private Sprite command;
    [SerializeField] private Sprite backgroundMyRoom;
    [SerializeField] private Sprite backgroundRoad;
    [SerializeField] private Sprite backgroundRooftop;
    [SerializeField] private Sprite backgroundRooftop2;
    [SerializeField] private Image backgroundImage2;
    [SerializeField] private GameObject effectsObject;
    private Image effectsImage;
    private RectTransform effectsRect;
    [SerializeField] private GameObject effectsObject2;
    private Image effectsImage2;
    private RectTransform effectsRect2;
    [SerializeField] private Sprite swordEffect;
    [SerializeField] private Sprite swordEffect2;
    [SerializeField] private Sprite bloodEffect;
    [SerializeField] private Sprite jumpEffect;
    [SerializeField] private Sprite healEffect;

    protected override void StartSet()
    {
        effectsImage = effectsObject.GetComponent<Image>();
        effectsRect = effectsObject.GetComponent<RectTransform>();
        effectsImage2 = effectsObject2.GetComponent<Image>();
        effectsRect2 = effectsObject2.GetComponent<RectTransform>();
    }

    //立ち絵関係
    public override void CharacterChange(string image)
    {
        switch (image)
        {
            case "transparent":
                _characterImage.sprite = noneSprite;
                break;
            case "vier_battle":
                _characterImage.sprite = vier_battle;
                break;
            case "vier_battle2":
                _characterImage.sprite = vier_battle2;
                break;
            case "vier_battle3":
                _characterImage.sprite = vier_battle3;
                break;
            case "vier_battle4":
                _characterImage.sprite = vier_battle4;
                break;
            case "vier_battle5":
                _characterImage.sprite = vier_battle5;
                break;
            case "vier_battle6":
                _characterImage.sprite = vier_battle6;
                break;
            case "vier_battle7":
                _characterImage.sprite = vier_battle7;
                break;
            case "vier_battle8":
                _characterImage.sprite = vier_battle8;
                break;
            case "el_battle":
                _characterImage.sprite = el_battle;
                break;
            case "el_battle2":
                _characterImage.sprite = el_battle2;
                break;
            case "el_battle3":
                _characterImage.sprite = el_battle3;
                break;
            case "el_battle4":
                _characterImage.sprite = el_battle4;
                break;
            case "el_battle5":
                _characterImage.sprite = el_battle5;
                break;
            case "el_enemy":
                _characterImage.sprite = el_enemy;
                break;
            case "Ghost1":
                _characterImage.sprite = ghost1;
                break;
            case "Command":
                _characterImage.sprite = command;
                break;
            default:
                break;
        }
    }

    //背景切り替え
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
            case "Rooftop":
                _backgroundImage.sprite = backgroundRooftop;
                break;
            case "Rooftop2":
                StartCoroutine(FadeOut(3.0f, backgroundImage2));
                backgroundImage2.sprite = backgroundRooftop2;
                break;
            default:
                break;
        }
    }

    //エフェクト(透明度とかサイズの処理に注意 特にスキップした場合)
    public override void Effect(string image)
    {
        if (!skip)
        {
            switch (image)
            {
                case "Sword":
                    StartCoroutine(SwordEffect());
                    break;
                case "BloodEffect":
                    StartCoroutine(BloodEffect());
                    break;
                case "Jump":
                    effectsImage.sprite = jumpEffect;
                    StartCoroutine(FadeIn(0.5f, effectsImage));
                    break;
                case "Heal":
                    effectsImage.sprite = healEffect;
                    effectsImage.color = new(1, 1, 1, 0.3f);
                    effectsRect.localScale = new(1.6f, 1.6f);
                    break;
                case "HealFinish":
                    StartCoroutine(HealFinish());
                    break;
                default:
                    break;
            }
        }
    }
    private IEnumerator SwordEffect()
    {
        StartCoroutine(Sword1());
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(Sword2());
    }
    private IEnumerator Sword1()
    {
        effectsRect2.localScale = new(0.5f, 0.5f);
        effectsImage.sprite = swordEffect2;
        effectsRect.localEulerAngles = new(0, 0, 90);
        while (effectsRect.localScale.x < 2)
        {
            yield return null;
            float temp = effectsRect.localScale.x;
            temp += 6 * Time.deltaTime;
            effectsRect.localScale = new(temp, temp);
        }
        yield return StartCoroutine(FadeIn(0.25f, effectsImage));
        effectsRect.localEulerAngles = Vector3.zero;
        effectsRect.localScale = new(1, 1);
        effectsImage.sprite = noneSprite;
        effectsImage.color = Color.white;
    }
    private IEnumerator Sword2()
    {
        effectsRect2.localScale = new(3, 3);
        effectsImage2.sprite = swordEffect;
        StartCoroutine(FadeIn(0.5f, effectsImage2));
        while(effectsRect2.localScale.x > 0.5f)
        {
            yield return null;
            float temp = effectsRect2.localScale.x;
            temp -= 5 * Time.deltaTime;
            effectsRect2.localScale = new(temp, temp);
        }
        effectsRect2.localScale = new(1, 1);
        effectsImage2.sprite = noneSprite;
        effectsImage2.color = Color.white;
    }
    private IEnumerator BloodEffect()
    {
        effectsImage.sprite = bloodEffect;
        yield return StartCoroutine(FadeOut(0.5f, effectsImage));
        yield return StartCoroutine(FadeIn(0.5f, effectsImage));
        effectsImage.sprite = noneSprite;
        effectsImage.color = Color.white;
    }
    private IEnumerator HealFinish()
    {
        if (!skip)
        {
            yield return StartCoroutine(FadeIn(0.5f, effectsImage));
        }
        effectsImage.sprite = noneSprite;
        effectsImage.color = Color.white;
        effectsRect.localScale = new(1, 1);
    }
    public override void ChangeScene()
    {
        GameManager.instance.LineNumber = 0;
        SceneManager.LoadScene("EndRoal");
    }
}
