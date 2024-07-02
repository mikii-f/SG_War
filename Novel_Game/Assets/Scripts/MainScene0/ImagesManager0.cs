using UnityEngine;
using UnityEngine.SceneManagement;

public class ImagesManager0 : ImagesManagerOrigin
{
    private TextManager0 textManager;
    protected override void StartSet()
    {
        textManager = tManager.GetComponent<TextManager0>();
        white.SetActive(false);
        blackOver.SetActive(false);
        blackUnder.SetActive(false);
    }

    public override void CharacterChange(int n)
    {

    }
    public override void BackgroundChange(int n)
    {

    }
    protected override void AnimationFinished(float waitTime)
    {
        StartCoroutine(textManager.AnimationFinished(waitTime));
    }
}
