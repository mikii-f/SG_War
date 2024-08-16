using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImagesManager4 : ImagesManagerOrigin
{
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite vier3;
    [SerializeField] private Sprite vier4;
    [SerializeField] private Sprite vier7;
    [SerializeField] private Sprite vier8;
    [SerializeField] private Sprite vier_battle;
    [SerializeField] private Sprite vier_battle2;
    [SerializeField] private Sprite vier_battle3;
    [SerializeField] private Sprite vier_battle4;
    [SerializeField] private Sprite vier_battle8;
    [SerializeField] private Sprite el_battle;
    [SerializeField] private Sprite el_enemy;
    [SerializeField] private Sprite ghost1;
    [SerializeField] private Sprite command;
    [SerializeField] private Sprite backgroundMyRoom;
    [SerializeField] private Sprite backgroundRoad;
    [SerializeField] private Sprite backgroundRooftop;
    [SerializeField] private Sprite backgroundRoadNight;
    [SerializeField] private Sprite backgroundNightSky;
    [SerializeField] private GameObject effectsObject;
    private Image effectsImage;
    private RectTransform effectsRect;
    [SerializeField] private Sprite windEffect;
    private Material _material;
    private Coroutine _coroutine;

    protected override void StartSet()
    {
        effectsImage = effectsObject.GetComponent<Image>();
        effectsRect = effectsObject.GetComponent<RectTransform>();
        blackAllImage.color = Color.clear;
        _material = effectsImage.material;
    }

    //óßÇøäGä÷åW
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
            case 3:
                _characterImage.sprite = vier3;
                break;
            case 4:
                _characterImage.sprite = vier4;
                break;
            case 7:
                _characterImage.sprite = vier7;
                break;
            case 8:
                _characterImage.sprite = vier8;
                break;
            case 21:
                _characterImage.sprite = vier_battle;
                break;
            case 22:
                _characterImage.sprite = vier_battle2;
                break;
            case 23:
                _characterImage.sprite = vier_battle3;
                break;
            case 24:
                _characterImage.sprite = vier_battle4;
                break;
            case 28:
                _characterImage.sprite = vier_battle8;
                break;
            case 71:
                _characterImage.sprite = el_battle;
                break;
            case 91:
                _characterImage.sprite = el_enemy;
                break;
            case 101:
                _characterImage.sprite = ghost1;
                break;
            case 106:
                _characterImage.sprite = command;
                break;
            default:
                break;
        }
    }

    //îwåiêÿÇËë÷Ç¶
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
            case 4:
                _backgroundImage.sprite = backgroundRooftop;
                break;
            case 5:
                _backgroundImage.sprite = backgroundRoadNight;
                break;
            case 6:
                _backgroundImage.sprite = backgroundNightSky;
                break;
            default:
                break;
        }
    }

    public override void Effect(int n)
    {
        switch (n)
        {
            case 1:
                effectsImage.sprite = windEffect;
                effectsRect.sizeDelta = new(3840, 1080);
                _coroutine = StartCoroutine(WindEffect());
                break;
            case 2:
                StopCoroutine(_coroutine);
                effectsImage.sprite = noneSprite;
                effectsImage.color = Color.white;
                effectsRect.sizeDelta = new(1920, 1080);
                _material.SetTextureOffset("_MainTex", Vector2.zero);
                break;
            default:
                break;
        }
    }

    private IEnumerator WindEffect()
    {
        float timeCount = 0;
        while (true)
        {
            //1ïbÇ≈àÍé¸
            float offset = Mathf.Repeat(timeCount, 1);
            _material.SetTextureOffset("_MainTex", new(offset, 0));
            yield return null;
            timeCount += Time.deltaTime;
        }
    }

    public override void ChangeScene()
    {
        SceneManager.LoadScene("BattleScene3");
    }
}
