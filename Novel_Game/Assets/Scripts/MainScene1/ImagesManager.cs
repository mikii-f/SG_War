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
            case "vier5":
                _characterImage.sprite = vier5;
                break;
            case "vier6":
                _characterImage.sprite = vier6;
                break;
            case "vier7":
                _characterImage.sprite = vier7;
                break;
            case "vier8":
                _characterImage.sprite = vier8;
                break;
            case "el_battle2":
                _characterImage.sprite = el_battle2;
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
            case "City":
                _backgroundImage.sprite = backgroundCity;
                break;
            case "Silhouette":
                _backgroundImage.sprite = backgroundSilhouette;
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
        GameManager.instance.LineNumber = 0;
        SceneManager.LoadScene("MainScene2");
    }
}
