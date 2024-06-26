using UnityEngine;

public class ImagesManager2 : ImagesManagerOrigin
{
    private TextManager2 textManager;

    // Start is called before the first frame update
    protected override void StartSet()
    {
        textManager = tManager.GetComponent<TextManager2>();
        textPanel.SetActive(false);
    }

    //�����G�֌W
    public override void CharacterChange(int n)
    {
        switch (n)
        {
            case 0:
                _characterImage.sprite = noneSprite;
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
            default:
                break;
        }
    }
    //�e�L�X�g�ւ̃A�j���[�V�����I���ʒm
    protected override void AnimationFinished(float waitTime)
    {
        StartCoroutine(textManager.AnimationFinished(waitTime));
    }
}
