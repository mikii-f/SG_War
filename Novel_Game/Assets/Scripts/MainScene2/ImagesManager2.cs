using UnityEngine;
using UnityEngine.SceneManagement;

public class ImagesManager2 : ImagesManagerOrigin
{
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite vier3;
    [SerializeField] private Sprite vier4;
    [SerializeField] private Sprite vier5;
    [SerializeField] private Sprite vier7;
    [SerializeField] private Sprite vier8;
    [SerializeField] private Sprite vier_battle;
    [SerializeField] private Sprite vier_battle4;
    [SerializeField] private Sprite el_battle;
    [SerializeField] private Sprite ghost1;
    [SerializeField] private Sprite backgroundMyRoom;
    [SerializeField] private Sprite backgroundRoad;
    [SerializeField] private Sprite backgroundRoadNight;

    //この程度の処理ならスクリプト側から対応できるが、時折GetComponentなども使うため必要
    protected override void StartSet()
    {
        blackAllImage.color = Color.clear;
    }

    //立ち絵関係
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
            case "vier5":
                _characterImage.sprite = vier5;
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
            case "vier_battle4":
                _characterImage.sprite = vier_battle4;
                break;
            case "el_battle":
                _characterImage.sprite = el_battle;
                break;
            case "Ghost1":
                _characterImage.sprite = ghost1;
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
        //撤退時に直前へ戻れるように行数の初期化をしない
        SceneManager.LoadScene("BattleScene1");
    }
}