using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndRoal : SystemManagerOrigin
{
    [SerializeField] private GameObject nextTitle;
    [SerializeField] private RectTransform endroalRect;
    [SerializeField] private Image white;
    [SerializeField] private RectTransform skipSwitchRect;
    private bool skip = false;

    void Start()
    {
        StartCoroutine(TitleToEndRoal());
    }
    private IEnumerator TitleToEndRoal()
    {
        yield return new WaitForSeconds(6);
        nextTitle.SetActive(false);
        yield return new WaitForSeconds(2);
        while (endroalRect.anchoredPosition.y < 2200)
        {
            Vector2 temp = endroalRect.anchoredPosition;
            temp.y += 210 * Time.deltaTime;
            endroalRect.anchoredPosition = temp;
            yield return null;
        }
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(FadeOut(3, white));
        GameManager.instance.SceneName = "AfterClear";
        GameManager.instance.Save();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("AfterClear");
    }
    public void Skip()
    {
        if (!skip)
        {
            skip = true;
            StartCoroutine(ButtonAnim(skipSwitchRect));
            StartCoroutine(SkipEndroal());
        }
    }
    private IEnumerator SkipEndroal()
    {
        yield return StartCoroutine(FadeOut(1, white));
        SceneManager.LoadScene("AfterClear");
    }
}
