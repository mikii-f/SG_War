using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImagesManager : ImagesManagerOrigin
{
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite el;
    [SerializeField] private Sprite backgroundMyRoom;
    [SerializeField] private Sprite backgroundRoad;
    [SerializeField] private Sprite backgroundCity;

    protected override void StartSet()
    {
        blackAllImage.color = new(0, 0, 0, 0.7f);
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
}
