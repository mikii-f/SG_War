using UnityEngine;
using UnityEngine.SceneManagement;

public class ImagesManager0 : ImagesManagerOrigin
{
    [SerializeField] private Sprite vier_battle;
    [SerializeField] private Sprite vier_battle3;
    [SerializeField] private Sprite vier_battle4;
    [SerializeField] private Sprite command;
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
            case 23:
                _characterImage.sprite = vier_battle3;
                break;
            case 24:
                _characterImage.sprite = vier_battle4;
                break;
            case 106:
                _characterImage.sprite = command;
                break;
            default:
                break;
        }
    }
    public override void BackgroundChange(int n)
    {

    }

    public override void Effect(int n)
    {

    }
    public override void ChangeScene()
    {
        SceneManager.LoadScene("BattleScene0");
    }
}
