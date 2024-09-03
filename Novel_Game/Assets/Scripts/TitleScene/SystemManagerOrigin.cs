using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class SystemManagerOrigin : MonoBehaviour
{
    protected bool switchInterval = false;  //メッセージを閉じるときなどにスイッチのアニメーションが見えてから閉じるようにするとその間にまだスイッチが押せてしまうため、それを防ぐ
    protected IEnumerator SwitchInterval()
    {
        switchInterval = true;
        yield return new WaitForSeconds(0.1f);
        switchInterval = false;
    }
    //ボタンのアニメーション
    protected IEnumerator ButtonAnim(RectTransform rect)
    {
        Vector2 temp = rect.localScale;
        rect.localScale = new(0.9f * temp.x, 0.9f * temp.y);
        yield return new WaitForSeconds(0.08f);
        rect.localScale = temp;
    }
    //ボタンのアニメーションを待ってから開閉
    protected IEnumerator Delay(GameObject panel, bool TorF)
    {
        yield return new WaitForSeconds(0.1f);
        panel.SetActive(TorF);
    }
    //ワイプなどの処理と比べて、少し粗い遷移の方が「それっぽい」気がするためyield return nullにしない
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
