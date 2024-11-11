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
    [SerializeField] private RectTransform configSwitchRect;
    [SerializeField] private GameObject words1;
    [SerializeField] private GameObject method;
    [SerializeField] private GameObject config;
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
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;
    [SerializeField] private Slider autoSlider;
    private bool newGame = true;

    void Start()
    {
        words1.SetActive(false);
        method.SetActive(false);
        config.SetActive(false);
        systemMessageObject.SetActive(false);
        saveDataPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(FadeIn(0.5f, black));
        SaveDataCheck();
        bgmSlider.onValueChanged.AddListener(OnBGMSliderValueChanged);
        seSlider.onValueChanged.AddListener(OnSESliderValueChanged);
        autoSlider.onValueChanged.AddListener(OnAutoSliderValueChanged);
        audioSource.volume = GameManager.instance.BgmVolume;
        seSource.volume = GameManager.instance.SeVolume;
    }
    private void SaveDataCheck()
    {
        //一つ以上セーブデータがあるか
        if (GameManager.instance.SaveData)
        {
            continueSwitchMask.SetActive(false);
        }
        //データ1～3の進行度を確認
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
        //コンフィグの設定(float値のためイコール判定は避ける)
        if (GameManager.instance.BgmVolume < 0.01f)
        {
            bgmSlider.value = 0;
        }
        else if (GameManager.instance.BgmVolume < 0.13f)
        {
            bgmSlider.value = 1;
        }
        else if (GameManager.instance.BgmVolume < 0.26f)
        {
            bgmSlider.value = 2;
        }
        else if (GameManager.instance.BgmVolume < 0.38f)
        {
            bgmSlider.value = 3;
        }
        else
        {
            bgmSlider.value = 4;
        }
        if (GameManager.instance.SeVolume < 0.01f)
        {
            seSlider.value = 0;
        }
        else if (GameManager.instance.SeVolume < 0.13f)
        {
            seSlider.value = 1;
        }
        else if (GameManager.instance.SeVolume < 0.26f)
        {
            seSlider.value = 2;
        }
        else if (GameManager.instance.SeVolume < 0.38f)
        {
            seSlider.value = 3;
        }
        else
        {
            seSlider.value = 4;
        }
        if (GameManager.instance.AutoSpeed < 0.6f)
        {
            autoSlider.value = 0;
        }
        else if (GameManager.instance.AutoSpeed < 1.1f)
        {
            autoSlider.value = 1;
        }
        else if (GameManager.instance.AutoSpeed < 2.1f)
        {
            autoSlider.value = 2;
        }
        else if (GameManager.instance.AutoSpeed < 3.1f)
        {
            autoSlider.value = 3;
        }
        else
        {
            autoSlider.value = 4;
        }
    }
    //スライダー操作時に発生させるイベント
    public void OnBGMSliderValueChanged(float value)
    {
        switch (bgmSlider.value)
        {
            case 0:
                audioSource.volume = 0f;
                break;
            case 1:
                audioSource.volume = 0.125f;
                break;
            case 2:
                audioSource.volume = 0.25f;
                break;
            case 3:
                audioSource.volume = 0.375f;
                break;
            case 4:
                audioSource.volume = 0.5f;
                break;
        }
        GameManager.instance.BgmVolume = audioSource.volume;
        seSource.clip = seUIClick;
        seSource.Play();
    }
    public void OnSESliderValueChanged(float value)
    {
        switch (seSlider.value)
        {
            case 0:
                seSource.volume = 0f;
                break;
            case 1:
                seSource.volume = 0.125f;
                break;
            case 2:
                seSource.volume = 0.25f;
                break;
            case 3:
                seSource.volume = 0.375f;
                break;
            case 4:
                seSource.volume = 0.5f;
                break;
        }
        GameManager.instance.SeVolume = seSource.volume;
        seSource.clip = seUIClick;
        seSource.Play();
    }
    public void OnAutoSliderValueChanged(float value)
    {
        switch (autoSlider.value)
        {
            case 0:
                GameManager.instance.AutoSpeed = 0.5f;
                break;
            case 1:
                GameManager.instance.AutoSpeed = 1f;
                break;
            case 2:
                GameManager.instance.AutoSpeed = 2f;
                break;
            case 3:
                GameManager.instance.AutoSpeed = 3f;
                break;
            case 4:
                GameManager.instance.AutoSpeed = 4f;
                break;
        }
        seSource.clip = seUIClick;
        seSource.Play();
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
    public void ConfigSwitch()
    {
        if (!isGoNext)
        {
            StartCoroutine(ButtonAnim(configSwitchRect));
            StartCoroutine(Delay(config, true));
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
        config.SetActive(false);
        GameManager.instance.SetConfig();
        saveDataPanel.SetActive(false);
        seSource.clip = seUIBack;
        seSource.Play();
    }
}
