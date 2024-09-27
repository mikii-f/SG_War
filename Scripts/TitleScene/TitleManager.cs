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
    [SerializeField] private GameObject saveDataPanel;
    [SerializeField] private RectTransform data1SwitchRect;
    [SerializeField] private RectTransform data2SwitchRect;
    [SerializeField] private RectTransform data3SwitchRect;
    [SerializeField] private Text data1Text;
    [SerializeField] private Text data2Text;
    [SerializeField] private Text data3Text;
    private bool newGame = true;

    void Start()
    {
        words1.SetActive(false);
        method.SetActive(false);
        systemMessageObject.SetActive(false);
        saveDataPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.instance.BgmVolume;
        seSource.volume = GameManager.instance.SeVolume;
        StartCoroutine(FadeIn(0.5f, black));
        if (GameManager.instance.SaveData)
        {
            continueSwitchMask.SetActive(false);
        }
        if (PlayerPrefs.GetString("progress") != "")
        {
            data1Text.text = PlayerPrefs.GetString("progress");
        }
        else
        {
            data1Text.text = "データなし";
        }
        if (PlayerPrefs.GetString("progress2") != "")
        {
            data2Text.text = PlayerPrefs.GetString("progress2");
        }
        else
        {
            data2Text.text = "データなし";
        }
        if (PlayerPrefs.GetString("progress3") != "")
        {
            data3Text.text = PlayerPrefs.GetString("progress3");
        }
        else
        {
            data3Text.text = "データなし";
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
        if (!switchInterval && !isGoNext)
        {
            StartCoroutine(SwitchInterval());
            StartCoroutine(ButtonAnim(newGameSwitchRect));
            StartCoroutine(Delay(saveDataPanel, true));
            newGame = true;
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }
    public void YesSwitch()
    {
        if (!isGoNext && !switchInterval)
        {
            isGoNext = true;
            StartCoroutine(ButtonAnim(yesSwitch));
            StartCoroutine(NewGame());
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }
    public void NoSwitch()
    {
        if (!isGoNext && !switchInterval)
        {
            StartCoroutine(SwitchInterval());
            StartCoroutine(ButtonAnim(noSwitch));
            StartCoroutine(Delay(systemMessageObject, false));
            seSource.clip = seUIBack;
            seSource.Play();
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
        if (!isGoNext && !continueSwitchMask.activeSelf && !switchInterval)
        {
            StartCoroutine(SwitchInterval());
            StartCoroutine(ButtonAnim(continueGameSwitchRect));
            StartCoroutine(Delay(saveDataPanel, true));
            newGame = false;
            seSource.clip = seUIClick;
            seSource.Play();
        }
        else if (continueSwitchMask.activeSelf)
        {
            seSource.clip = seUIUnactive;
            seSource.Play();
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
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }
    public void MethodSwitch()
    {
        if (!isGoNext)
        {
            StartCoroutine(ButtonAnim(methodSwitchRect));
            StartCoroutine(Delay(method, true));
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }
    public void Data1Switch()
    {
        if (!isGoNext && !switchInterval)
        {
            if (newGame)
            {
                StartCoroutine(SwitchInterval());
                StartCoroutine(ButtonAnim(data1SwitchRect));
                GameManager.instance.SaveDataNumber = 1;
                seSource.clip = seUIClick;
                seSource.Play();
                StartCoroutine(Delay(systemMessageObject, true));
            }
            else
            {
                if (data1Text.text != "データなし")
                {
                    StartCoroutine(ButtonAnim(data1SwitchRect));
                    GameManager.instance.SaveDataNumber = 1;
                    seSource.clip = seUIClick;
                    seSource.Play();
                    isGoNext = true;
                    StartCoroutine(ContinueGame());
                }
                else
                {
                    seSource.clip = seUIUnactive;
                    seSource.Play();
                }
            }
        }
    }
    public void Data2Switch()
    {
        if (!isGoNext && !switchInterval)
        {
            if (newGame)
            {
                StartCoroutine(SwitchInterval());
                StartCoroutine(ButtonAnim(data2SwitchRect));
                GameManager.instance.SaveDataNumber = 2;
                seSource.clip = seUIClick;
                seSource.Play();
                StartCoroutine(Delay(systemMessageObject, true));
            }
            else
            {
                if (data2Text.text != "データなし")
                {
                    StartCoroutine(ButtonAnim(data2SwitchRect));
                    GameManager.instance.SaveDataNumber = 2;
                    seSource.clip = seUIClick;
                    seSource.Play();
                    isGoNext = true;
                    StartCoroutine(ContinueGame());
                }
                else
                {
                    seSource.clip = seUIUnactive;
                    seSource.Play();
                }
            }
        }
    }
    public void Data3Switch()
    {
        if (!isGoNext && !switchInterval)
        {
            if (newGame)
            {
                StartCoroutine(SwitchInterval());
                StartCoroutine(ButtonAnim(data3SwitchRect));
                GameManager.instance.SaveDataNumber = 3;
                seSource.clip = seUIClick;
                seSource.Play();
                StartCoroutine(Delay(systemMessageObject, true));
            }
            else
            {
                if (data3Text.text != "データなし")
                {
                    StartCoroutine(ButtonAnim(data3SwitchRect));
                    GameManager.instance.SaveDataNumber = 3;
                    seSource.clip = seUIClick;
                    seSource.Play();
                    isGoNext = true;
                    StartCoroutine(ContinueGame());
                }
                else
                {
                    seSource.clip = seUIUnactive;
                    seSource.Play();
                }
            }
        }
    }
    public void Close()
    {
        words1.SetActive(false);
        method.SetActive(false);
        saveDataPanel.SetActive(false);
        seSource.clip = seUIBack;
        seSource.Play();
    }
}
