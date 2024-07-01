using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagesManager2_2 : ImagesManagerOrigin
{
    private TextManager textManager;
    [SerializeField] private Sprite vier;
    [SerializeField] private Sprite ghost1;
    [SerializeField] private Sprite backgroundMyRoom;
    [SerializeField] private Sprite backgroundRoad;

    protected override void StartSet()
    {
        textManager = tManager.GetComponent<TextManager>();
        textPanel.SetActive(false);
        chapterTitle.SetActive(false);
    }

    //立ち絵関係
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
            case 11:
                _characterImage.sprite = ghost1;
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

    //テキストへのアニメーション終了通知
    protected override void AnimationFinished(float waitTime)
    {
        StartCoroutine(textManager.AnimationFinished(waitTime));
    }
}
