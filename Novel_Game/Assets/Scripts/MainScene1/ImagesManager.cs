using UnityEngine;
using UnityEngine.SceneManagement;

public class ImagesManager : ImagesManagerOrigin
{
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite vier5;
    [SerializeField] private Sprite vier6;
    [SerializeField] private Sprite vier7;
    [SerializeField] private Sprite vier8;
    [SerializeField] private Sprite el_battle2;
    [SerializeField] private Sprite backgroundMyRoom;
    [SerializeField] private Sprite backgroundRoad;
    [SerializeField] private Sprite backgroundCity;
    [SerializeField] private Sprite backgroundSilhouette;

    protected override void StartSet()
    {
        
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
            case 5:
                _characterImage.sprite = vier5;
                break;
            case 6:
                _characterImage.sprite = vier6;
                break;
            case 7:
                _characterImage.sprite = vier7;
                break;
            case 8:
                _characterImage.sprite = vier8;
                break;
            case 72:
                _characterImage.sprite = el_battle2;
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
            case 3:
                _backgroundImage.sprite = backgroundCity;
                break;
            case 8:
                _backgroundImage.sprite = backgroundSilhouette;
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
        GameManager.instance.LineNumber = 0;
        SceneManager.LoadScene("MainScene2");
    }
}
