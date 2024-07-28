using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImagesManager4_2 : ImagesManagerOrigin
{
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite vier_battle;
    [SerializeField] private Sprite el;
    [SerializeField] private Sprite el_battle;
    [SerializeField] private Sprite el_enemy;
    [SerializeField] private Sprite ghost1;
    [SerializeField] private Sprite command;
    [SerializeField] private Sprite backgroundMyRoom;
    [SerializeField] private Sprite backgroundRoad;
    [SerializeField] private Sprite backgroundRooftop;
    [SerializeField] private Sprite backgroundRooftop2;
    [SerializeField] private Image backgroundImage2;

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
            case 21:
                _characterImage.sprite = vier_battle;
                break;
            case 51:
                _characterImage.sprite = el;
                break;
            case 71:
                _characterImage.sprite = el_battle;
                break;
            case 91:
                _characterImage.sprite = el_enemy;
                break;
            case 101:
                _characterImage.sprite = ghost1;
                break;
            case 106:
                _characterImage.sprite = command;
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
            case 4:
                _backgroundImage.sprite = backgroundRooftop;
                break;
            case 7:
                StartCoroutine(FadeOut(3.0f, backgroundImage2));
                backgroundImage2.sprite = backgroundRooftop2;
                break;
            default:
                break;
        }
    }
    public override void ChangeScene()
    {
        GameManager.instance.LineNumber = 0;
        SceneManager.LoadScene("EndRoal");
    }
}
