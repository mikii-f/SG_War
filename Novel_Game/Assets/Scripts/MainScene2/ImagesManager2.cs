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

    //�����G�֌W
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
    //�w�i�؂�ւ�
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
        //�P�ގ��ɒ��O�֖߂��悤�ɍs���̏����������Ȃ�
        SceneManager.LoadScene("BattleScene1");
    }
}