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
            case 3:
                _characterImage.sprite = vier3;
                break;
            case 5:
                _characterImage.sprite = vier5;
                break;
            case 8:
                _characterImage.sprite = vier8;
                break;
            case 101:
                _characterImage.sprite = ghost1;
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
        SceneManager.LoadScene("MainScene3");
    }
}
