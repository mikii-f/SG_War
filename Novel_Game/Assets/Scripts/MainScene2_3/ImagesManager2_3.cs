using UnityEngine;
using UnityEngine.SceneManagement;

public class ImagesManager2_3 : ImagesManagerOrigin
{
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite vier3;
    [SerializeField] private Sprite vier5;
    [SerializeField] private Sprite vier8;
    [SerializeField] private Sprite ghost1;
    [SerializeField] private Sprite backgroundMyRoom;
    [SerializeField] private Sprite backgroundRoad;

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
            case "vier3":
                _characterImage.sprite = vier3;
                break;
            case "vier5":
                _characterImage.sprite = vier5;
                break;
            case "vier8":
                _characterImage.sprite = vier8;
                break;
            case "Ghost1":
                _characterImage.sprite = ghost1;
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
        SceneManager.LoadScene("MainScene3");
    }
}
