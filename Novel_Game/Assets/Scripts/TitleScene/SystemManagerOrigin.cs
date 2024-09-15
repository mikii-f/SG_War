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
    //���C�v�Ȃǂ̏����Ɣ�ׂāA�����e���J�ڂ̕����u������ۂ��v�C�����邽��yield return null�ɂ��Ȃ�
    protected IEnumerator FadeOut(float fadeTime, Image image)
    {
        Color temp = image.color;
        temp.a = 0;
        image.color = temp;
        while (image.color.a < 1)
        {
            yield return new WaitForSeconds(0.1f);
            temp = image.color;
            temp.a = Mathf.Min(1, temp.a + 0.1f / fadeTime);
            image.color = temp;
        }
    }
    protected IEnumerator FadeIn(float fadeTime, Image image)
    {
        Color temp = image.color;
        temp.a = 1;
        image.color = temp;
        while (image.color.a > 0)
        {
            yield return new WaitForSeconds(0.1f);
            temp = image.color;
            temp.a = Mathf.Max(0, temp.a - 0.1f / fadeTime);
            image.color = temp;
        }
    }
}
