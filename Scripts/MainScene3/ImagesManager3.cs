using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImagesManager3 : ImagesManagerOrigin
{
    [SerializeField] private Image characterImage2;
    [SerializeField] private Image characterImage3;
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite vier2;
    [SerializeField] private Sprite vier3;
    [SerializeField] private Sprite vier4;
    [SerializeField] private Sprite vier5;
    [SerializeField] private Sprite vier8;
    [SerializeField] private Sprite vier_battle;
    [SerializeField] private Sprite vier_battle4;
    [SerializeField] private Sprite ghost1;
    [SerializeField] private Sprite ghost2;
    [SerializeField] private Sprite ghost3;
    [SerializeField] private Sprite backgroundMyRoom;
    [SerializeField] private Sprite backgroundRoad;
    [SerializeField] private Sprite backgroundRoadNight;

    protected override void StartSet()
    {
        
    }

    //�����G�֌W
    public override void CharacterChange(int n)
    {
        switch (n)
        {
            case 0:
                _characterImage.sprite = noneSprite;
                characterImage2.sprite = noneSprite;
                characterImage3.sprite = noneSprite;
                break;
            case 1:
                _characterImage.sprite = vier;
                break;
            case 2:
                _characterImage.sprite = vier2;
                break;
            case 3:
                _characterImage.sprite = vier3;
                break;
            case 4:
                _characterImage.sprite = vier4;
                break;
            case 5:
                _characterImage.sprite = vier5;
                break;
            case 8:
                _characterImage.sprite = vier8;
                break;
            case 21:
                _characterImage.sprite = vier_battle;
                break;
            case 24:
                _characterImage.sprite = vier_battle4;
                break;
            case 101:
                _characterImage.sprite = ghost1;
                break;
            case 111:
                _characterImage.sprite = ghost1;
                characterImage2.sprite = ghost2;
                characterImage3.sprite = ghost3;
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
            case 5:
                _backgroundImage.sprite = backgroundRoadNight;
                break;
            default:
                break;
        }
    }

    public override void Effect(int n)
    {

    }

    public override void ChangeScene()
    {
        //�s�������������Ȃ�
        SceneManager.LoadScene("BattleScene2");
    }
}