using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImagesManager3 : ImagesManagerOrigin
{
    [SerializeField] private Image characterImage2;
    [SerializeField] private Image characterImage3;
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite vier_battle;
    [SerializeField] private Sprite ghost1;
    [SerializeField] private Sprite ghost2;
    [SerializeField] private Sprite ghost3;
    [SerializeField] private Sprite backgroundMyRoom;
    [SerializeField] private Sprite backgroundRoad;
    [SerializeField] private Sprite backgroundRoadNight;

    protected override void StartSet()
    {
        blackAllImage.color = Color.clear;
    }

    //óßÇøäGä÷åW
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
            case 21:
                _characterImage.sprite = vier_battle;
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
            case 5:
                _backgroundImage.sprite = backgroundRoadNight;
                break;
            default:
                break;
        }
    }
    public override void ChangeScene()
    {
        //çsêîÇèâä˙âªÇµÇ»Ç¢
        SceneManager.LoadScene("BattleScene2");
    }
}
