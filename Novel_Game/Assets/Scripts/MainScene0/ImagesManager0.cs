using UnityEngine;
using UnityEngine.SceneManagement;

public class ImagesManager0 : ImagesManagerOrigin
{
    [SerializeField] private Sprite vier_battle;
    protected override void StartSet()
    {
        
    }

    public override void CharacterChange(int n)
    {
        switch (n)
        {
            case 0:
                _characterImage.sprite = noneSprite;
                break;
            case 21:
                _characterImage.sprite = vier_battle;
                break;
            default:
                break;
        }
    }
    public override void BackgroundChange(int n)
    {

    }
    public override void ChangeScene()
    {
        SceneManager.LoadScene("BattleScene0");
    }
}
