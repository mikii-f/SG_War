using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class SystemManagerOrigin : MonoBehaviour
{
    protected bool switchInterval = false;  //���b�Z�[�W�����Ƃ��ȂǂɃX�C�b�`�̃A�j���[�V�����������Ă������悤�ɂ���Ƃ��̊Ԃɂ܂��X�C�b�`�������Ă��܂����߁A�����h��
    protected IEnumerator SwitchInterval()
    {
        switchInterval = true;
        yield return new WaitForSeconds(0.1f);
        switchInterval = false;
    }
    //�{�^���̃A�j���[�V����
    protected IEnumerator ButtonAnim(RectTransform rect)
    {
        Vector2 temp = rect.localScale;
        rect.localScale = new(0.9f * temp.x, 0.9f * temp.y);
        yield return new WaitForSeconds(0.08f);
        rect.localScale = temp;
    }
    //�{�^���̃A�j���[�V������҂��Ă���J��
    protected IEnumerator Delay(GameObject panel, bool TorF)
    {
        yield return new WaitForSeconds(0.1f);
        panel.SetActive(TorF);
    }
    protected IEnumerator FadeOut(float fadeTime, Image image)
    {
        float waitTime = 0.1f;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 0.0f; alpha <= 255.0f; alpha += alphaChangeAmount)
        {
            Color newColor = image.color;
            newColor.a = alpha / 255.0f;
            image.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
    }
    protected IEnumerator FadeIn(float fadeTime, Image image)
    {
        float waitTime = 0.1f;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alphaChangeAmount)
        {
            Color newColor = image.color;
            newColor.a = alpha / 255.0f;
            image.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
