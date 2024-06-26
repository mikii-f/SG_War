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

    //立ち絵関係
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
    //背景切り替え
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
    //テキストへのアニメーション終了通知
    protected override void AnimationFinished(float waitTime)
    {
        StartCoroutine(textManager.AnimationFinished(waitTime));
    }
}
