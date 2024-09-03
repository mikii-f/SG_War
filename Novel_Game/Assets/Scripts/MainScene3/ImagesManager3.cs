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

    //óßÇøäGä÷åW
    public override void CharacterChange(string image)
    {
        switch (image)
        {
            case "transparent":
                _characterImage.sprite = noneSprite;
                characterImage2.sprite = noneSprite;
                characterImage3.sprite = noneSprite;
                break;
            case "vier":
                _characterImage.sprite = vier;
                break;
            case "vier2":
                _characterImage.sprite = vier2;
                break;
            case "vier3":
                _characterImage.sprite = vier3;
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
            case "vier_battle4":
                _characterImage.sprite = vier_battle4;
                break;
            case "Ghost1":
                _characterImage.sprite = ghost1;
                break;
            case "Enemys":
                _characterImage.sprite = ghost1;
                characterImage2.sprite = ghost2;
                characterImage3.sprite = ghost3;
                break;
            default:
                break;
        }
    }

    //îwåiêÿÇËë÷Ç¶
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
            case "RoadNight":
                _backgroundImage.sprite = backgroundRoadNight;
                break;
            default:
                break;
        }
    }

    public override void Effect(string image)
    {

    }

    public override void ChangeScene()
    {
        //çsêîÇèâä˙âªÇµÇ»Ç¢
        SceneManager.LoadScene("BattleScene2");
    }
}
