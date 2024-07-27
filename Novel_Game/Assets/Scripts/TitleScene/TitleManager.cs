using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : SystemManagerOrigin
{
    [SerializeField] private RectTransform newGameSwitchRect;
    [SerializeField] private RectTransform continueGameSwitchRect;
    [SerializeField] private RectTransform wordsSwitchRect;
    [SerializeField] private RectTransform methodSwitchRect;
    [SerializeField] private GameObject words1;
    [SerializeField] private GameObject method;
    [SerializeField] private GameObject systemMessageObject;
    [SerializeField] private RectTransform yesSwitch;
    [SerializeField] private RectTransform noSwitch;
    [SerializeField] private Image black;
    [SerializeField] private GameObject continueSwitchMask;

    void Start()
    {
        words1.SetActive(false);
        method.SetActive(false);
        systemMessageObject.SetActive(false);
        StartCoroutine(FadeIn(0.5f, black));
        if (GameManager.instance.SaveData)
        {
            continueSwitchMask.SetActive(false);
        }
    }

    private void Update()
    {
        //メッセージへの応答
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (systemMessageObject.activeSelf)
            {
                YesSwitch();
            }
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            if (systemMessageObject.activeSelf)
            {
                NoSwitch();
            }
        }
    }

    public void NewGameSwitch()
    {
        StartCoroutine(ButtonAnim(newGameSwitchRect));
        StartCoroutine(Delay(systemMessageObject));
    }
    public void YesSwitch()
    {
        StartCoroutine(ButtonAnim(yesSwitch));
        StartCoroutine(NewGame());
    }
    public void NoSwitch()
    {
        StartCoroutine(ButtonAnim(noSwitch));
        StartCoroutine(Delay(systemMessageObject));
    }
    private IEnumerator NewGame()
    {
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeOut(2, black));
        GameManager.instance.Initialize();
        SceneManager.LoadScene(GameManager.instance.SceneName);
    }
    public void ContinueGameSwitch()
    {
        StartCoroutine(ButtonAnim(continueGameSwitchRect));
        StartCoroutine(ContinueGame());
    }
    private IEnumerator ContinueGame()
    {
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeOut(2, black));
        GameManager.instance.Set();
        SceneManager.LoadScene(GameManager.instance.SceneName);
    }
    public void WordsSwitch()
    {
        StartCoroutine(ButtonAnim(wordsSwitchRect));
        StartCoroutine(Delay(words1));
    }
    public void MethodSwitch()
    {
        StartCoroutine(ButtonAnim(methodSwitchRect));
        StartCoroutine(Delay(method));
    }
    public void Close()
    {
        words1.SetActive(false);
        method.SetActive(false);
    }
}
