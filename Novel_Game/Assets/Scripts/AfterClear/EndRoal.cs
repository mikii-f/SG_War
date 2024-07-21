using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndRoal : MonoBehaviour
{
    [SerializeField] GameObject nextTitle;
    [SerializeField] RectTransform endroalRect;
    [SerializeField] Image black;

    // Start is called before the first frame update
    void Start()
    {
        black.color = new(1, 1, 1, 0);
        StartCoroutine(TitleToEndRoal());
    }
    private IEnumerator TitleToEndRoal()
    {
        yield return new WaitForSeconds(6);
        nextTitle.SetActive(false);
        yield return new WaitForSeconds(2);
        while (endroalRect.anchoredPosition.y < 2100)
        {
            Vector2 temp = endroalRect.anchoredPosition;
            temp.y += 210 * Time.deltaTime;
            endroalRect.anchoredPosition = temp;
            yield return null;
        }
        yield return new WaitForSeconds(3);
        float waitTime = 0.1f;
        float alphaChangeAmount = 255.0f / (3 / waitTime);
        for (float alpha = 0.0f; alpha <= 255.0f; alpha += alphaChangeAmount)
        {
            Color newColor = black.color;
            newColor.a = alpha / 255.0f;
            black.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
        GameManager.instance.SceneName = "AfterClear";
        GameManager.instance.Save();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("AfterClear");
    }
}
