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

    public override void CharacterChange(string image)
    {
        switch (image)
        {
            case "transparent":
                _characterImage.sprite = noneSprite;
                break;
            case "vier_battle":
                _characterImage.sprite = vier_battle;
                break;
            case "vier_battle3":
                _characterImage.sprite = vier_battle3;
                break;
            case "vier_battle4":
                _characterImage.sprite = vier_battle4;
                break;
            case "Command":
                _characterImage.sprite = command;
                break;
            default:
                break;
        }
    }
    public override void BackgroundChange(string image)
    {

    }
    public override void BGMChange(string bgm)
    {
        
    }
    public override void Effect(string image)
    {

    }
    public override void ChangeScene()
    {
        SceneManager.LoadScene("BattleScene0");
    }
}
