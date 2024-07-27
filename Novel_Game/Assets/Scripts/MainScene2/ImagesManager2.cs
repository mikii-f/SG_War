using UnityEngine;
using UnityEngine.SceneManagement;

public class ImagesManager2 : ImagesManagerOrigin
{
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite vier_battle;
    [SerializeField] private Sprite ghost1;
    [SerializeField] private Sprite backgroundMyRoom;
    [SerializeField] private Sprite backgroundRoad;

    protected override void StartSet()
    {
        blackAllImage.color = Color.clear;
    }

    //—§‚¿ŠGŠÖŒW
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
            case 21:
                _characterImage.sprite = vier_battle;
                break;
            case 101:
                _characterImage.sprite = ghost1;
                break;
            default:
                break;
        }
    }
    //”wŒiØ‚è‘Ö‚¦
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
            default:
                break;
        }
    }
    public override void ChangeScene()
    {
        //“P‘Ş‚É’¼‘O‚Ö–ß‚ê‚é‚æ‚¤‚És”‚Ì‰Šú‰»‚ğ‚µ‚È‚¢
        SceneManager.LoadScene("BattleScene1");
    }
}