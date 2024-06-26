using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImagesManager : ImagesManagerOrigin
{
    private TextManager textManager;
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite el;
    [SerializeField] private Sprite backgroundImage1;         //�C���������Ƃ��ɕ�����₷�����̂ɕύX
    [SerializeField] private Sprite backgroundImage2;
    [SerializeField] private Sprite backgroundImage3;
    [SerializeField] private Sprite backgroundImage4;

    // Start is called before the first frame update
    protected override void StartSet()
    {
        textManager = tManager.GetComponent<TextManager>();
        blackOverImage.color = new(0, 0, 0, 0.7f);
        blackUnderImage.color = new(0, 0, 0, 0.7f);
    }

    //���w�i�������J��
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

    //���w�i���J����ƂƂ��Ɍ��ɕ�܂ꏙ�X�ɖ߂�
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

    //�����G�֌W
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

    //�w�i�؂�ւ�
    public override void BackgroundChange(int n)
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
            case 4:
                _backgroundImage.sprite = backgroundImage4;
                break;
            default:
                break;
        }
    }

    //�e�L�X�g�ւ̃A�j���[�V�����I���ʒm
    protected override void AnimationFinished(float waitTime)
    {
        StartCoroutine(textManager.AnimationFinished(waitTime));
    }
}
