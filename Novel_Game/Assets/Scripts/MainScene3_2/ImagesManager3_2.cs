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
    private AudioSource effectsAudio;
    [SerializeField] private Sprite swordEffect;
    [SerializeField] private AudioClip bgmHome;
    [SerializeField] private AudioClip bgmRoadNight;
    [SerializeField] private AudioClip bgmEncounter;
    [SerializeField] private AudioClip bgmThinking;
    [SerializeField] private AudioClip bgmVision;
    [SerializeField] private AudioClip seBright;
    [SerializeField] private AudioClip seRoar;
    [SerializeField] private AudioClip seChange;
    [SerializeField] private AudioClip seSwitchOff;

    protected override void StartSet()
    {
        effectsImage = effectsObject.GetComponent<Image>();
        effectsRect = effectsObject.GetComponent<RectTransform>();
        effectsAudio = effectsObject.GetComponent<AudioSource>();
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
    public override void BGMChange(string bgm)
    {
        switch (bgm)
        {
            case "Home":
                audioSource.clip = bgmHome;
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
            case "Thinking":
                audioSource.clip = bgmThinking;
                audioSource.Play();
                break;
            case "Vision":
                audioSource.clip = bgmVision;
                audioSource.Play();
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
        effectsAudio.Play();
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
    public override void SoundEffect(string se)
    {
        if (!skip)
        {
            switch (se)
            {
                case "Bright":
                    seSource.clip = seBright;
                    seSource.Play();
                    break;
                case "Roar":
                    seSource.clip = seRoar;
                    seSource.Play();
                    break;
                case "Change":
                    seSource.clip = seChange;
                    seSource.Play();
                    break;
                case "SwitchOff":
                    seSource.clip = seSwitchOff;
                    seSource.Play();
                    break;
                default:
                    break;
            }
        }
    }
    public override void ChangeScene()
    {
        GameManager.instance.LineNumber = 0;
        SceneManager.LoadScene("MainScene4");
    }
}
