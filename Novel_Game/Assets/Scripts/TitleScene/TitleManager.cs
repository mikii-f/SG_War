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
    private AudioSource audioSource;
    private bool isGoNext = false;

    void Start()
    {
        words1.SetActive(false);
        method.SetActive(false);
        systemMessageObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.instance.BgmVolume;
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
        StartCoroutine(Delay(systemMessageObject, true));
    }
    public void YesSwitch()
    {
        if (!isGoNext && !switchInterval)
        {
            isGoNext = true;
            StartCoroutine(ButtonAnim(yesSwitch));
            StartCoroutine(NewGame());
        }
    }
    public void NoSwitch()
    {
        if (!isGoNext && !switchInterval)
        {
            StartCoroutine(SwitchInterval());
            StartCoroutine(ButtonAnim(noSwitch));
            StartCoroutine(Delay(systemMessageObject, false));
        }
    }
    private IEnumerator NewGame()
    {
        StartCoroutine(VolumeFadeOut(1, audioSource));
        yield return StartCoroutine(FadeOut(2, black));
        GameManager.instance.Initialize();
        SceneManager.LoadScene(GameManager.instance.SceneName);
    }
    public void ContinueGameSwitch()
    {
        if (!isGoNext)
        {
            isGoNext = true;
            StartCoroutine(ButtonAnim(continueGameSwitchRect));
            StartCoroutine(ContinueGame());
        }
    }
    private IEnumerator ContinueGame()
    {
        StartCoroutine(VolumeFadeOut(1, audioSource));
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeOut(1, black));
        GameManager.instance.Set();
        SceneManager.LoadScene(GameManager.instance.SceneName);
    }
    public void WordsSwitch()
    {
        if (!isGoNext)
        {
            StartCoroutine(ButtonAnim(wordsSwitchRect));
            StartCoroutine(Delay(words1, true));
        }
    }
    public void MethodSwitch()
    {
        if (!isGoNext)
        {
            StartCoroutine(ButtonAnim(methodSwitchRect));
            StartCoroutine(Delay(method, true));
        }
    }
    public void Close()
    {
        words1.SetActive(false);
        method.SetActive(false);
    }
}
