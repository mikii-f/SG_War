using System.Collections;
using Unity.VisualScripting;
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
    private AudioSource effectsAudio;
    [SerializeField] private Sprite windEffect;
    private Material _material;
    private Coroutine _coroutine;
    [SerializeField] private AudioClip bgmTitle;
    [SerializeField] private AudioClip bgmReach;
    [SerializeField] private AudioClip bgmChapter;
    [SerializeField] private AudioClip bgmRoad;
    [SerializeField] private AudioClip bgmRoadNight;
    [SerializeField] private AudioClip bgmEncounter;
    [SerializeField] private AudioClip bgmComedy;
    [SerializeField] private AudioClip bgmSurprise;
    [SerializeField] private AudioClip seSlide;
    [SerializeField] private AudioClip seFoot;

    protected override void StartSet()
    {
        effectsImage = effectsObject.GetComponent<Image>();
        effectsRect = effectsObject.GetComponent<RectTransform>();
        effectsAudio = effectsObject.GetComponent<AudioSource>();
        blackAllImage.color = Color.clear;
        _material = effectsImage.material;
    }

    //�����G�֌W
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
            case "vier3":
                _characterImage.sprite = vier3;
                break;
            case "vier4":
                _characterImage.sprite = vier4;
                break;
            case "vier7":
                _characterImage.sprite = vier7;
                break;
            case "vier8":
                _characterImage.sprite = vier8;
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
            case "vier_battle8":
                _characterImage.sprite = vier_battle8;
                break;
            case "el_battle":
                _characterImage.sprite = el_battle;
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

    //�w�i�؂�ւ�
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
            case "RoadNight":
                _backgroundImage.sprite = backgroundRoadNight;
                break;
            case "NightSky":
                _backgroundImage.sprite = backgroundNightSky;
                break;
            default:
                break;
        }
    }
    public override void BGMChange(string bgm)
    {
        switch (bgm)
        {
            case "Title":
                audioSource.clip = bgmTitle;
                audioSource.Play();
                break;
            case "Reach":
                audioSource.clip = bgmReach;
                audioSource.Play();
                break;
            case "Chapter":
                audioSource.clip = bgmChapter;
                audioSource.Play();
                break;
            case "Road":
                audioSource.clip = bgmRoad;
                audioSource.Play();
                break;
            case "RoadNight":
                audioSource.clip = bgmRoadNight;
                audioSource.Play();
                break;
            case "Encounter":
                audioSource.clip = bgmEncounter;
                audioSource.Play();
                break;
            case "Comedy":
                audioSource.clip = bgmComedy;
                audioSource.Play();
                break;
            case "Surprise":
                audioSource.clip = bgmSurprise;
                audioSource.Play();
                break;
            default:
                break;
        }
    }
    public override void Effect(string image)
    {
        switch (image)
        {
            case "WindEffectStart":
                effectsImage.sprite = windEffect;
                effectsRect.sizeDelta = new(3840, 1080);
                _coroutine = StartCoroutine(WindEffect());
                effectsAudio.Play();
                break;
            case "WindEffectStop":
                StopCoroutine(_coroutine);
                effectsImage.sprite = noneSprite;
                effectsImage.color = Color.white;
                effectsRect.sizeDelta = new(1920, 1080);
                _material.SetTextureOffset("_MainTex", Vector2.zero);
                effectsAudio.clip = null;
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
            //1�b�ň��
            float offset = Mathf.Repeat(timeCount, 1);
            _material.SetTextureOffset("_MainTex", new(offset, 0));
            yield return null;
            timeCount += Time.deltaTime;
        }
    }
    public override void SoundEffect(string se)
    {
        if (!skip)
        {
            switch (se)
            {
                case "Slide":
                    seSource.clip = seSlide;
                    seSource.Play();
                    break;
                case "Foot":
                    seSource.clip = seFoot;
                    seSource.Play();
                    break;
                default:
                    break;
            }
        }
    }
    public override void ChangeScene()
    {
        SceneManager.LoadScene("BattleScene3");
    }
}
