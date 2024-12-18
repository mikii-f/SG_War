using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSystemManager : SystemManagerOrigin
{
    [SerializeField] private BattleSceneManagerOrigin bSManager;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject functions;
    [SerializeField] private RectTransform restartSwitchRect;
    [SerializeField] private RectTransform retreatSwitchRect;
    [SerializeField] private RectTransform debugSwitchRect;
    [SerializeField] private GameObject systemMessageObject;
    [SerializeField] private Text systemMessage;
    [SerializeField] private RectTransform yesSwitch;
    [SerializeField] private RectTransform noSwitch;
    [SerializeField] private Text yesText;
    [SerializeField] private Text noText;
    [SerializeField] private GameObject growMask1;
    [SerializeField] private GameObject growMask2;
    [SerializeField] private Image black;
    private int messageNumber;
    private bool isMessageDisplay = false;
    public bool IsMessageDisplay { set { isMessageDisplay = value; } }
    private bool isFunctionAvailable = true;
    private bool isSelected = false;

    void Start()
    {
        functions.SetActive(false);
        systemMessageObject.SetActive(false);
        seSource = GetComponent<AudioSource>();
        seSource.volume = GameManager.instance.SeVolume;
        //1回目の育成を行うまでは育成を選択できない
        if (GameManager.instance.EXP != 0)
        {
            growMask1.SetActive(false);
        }
        growMask2.SetActive(false);
    }

    void Update()
    {
        if (isFunctionAvailable)
        {
            //メニューオンオフ
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (!functions.activeSelf)
                {
                    MenuSwitch();
                }
                else if (!isMessageDisplay)
                {
                    FunctionsClose();
                }
            }
            //各ファンクション(ファンクションが開いていて、かつメッセージが表示されていないとき)
            else if (functions.activeSelf && !isMessageDisplay)
            {
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    bSManager.Explanation();
                }
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    RestartMenuSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    RetreatMenuSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    DebugMenuSwitch();
                }
            }
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
    }

    //メニュー開閉
    public void MenuSwitch()
    {
        if (bSManager.IsPausePossible())
        {
            functions.SetActive(true);
            bSManager.Pause();
            seSource.clip = seUIClick;
            seSource.Play();
        }
        else
        {
            seSource.clip = seUIUnactive;
            seSource.Play();
        }
    }
    public void FunctionsClose()
    {
        functions.SetActive(false);
        bSManager.Restart();
        seSource.clip = seUIBack;
        seSource.Play();
    }
    //ゲームオーバー(バトルシーンから呼び出し)
    public void GameOver()
    {
        if (GameManager.instance.EXP == 0)
        {
            growMask2.SetActive(true);
            systemMessage.text = "GAME OVER\n再挑戦しますか？";
        }
        else
        {
            systemMessage.text = "GAME OVER\n再挑戦しますか？\n直前の場面に戻り育成しますか？";
        }
        isFunctionAvailable = true;
        yesText.text = "再挑戦(Y)";
        noText.text = "育成へ(N)";
        messageNumber = 0;
        systemMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    //再挑戦(メニューから選択)
    public void RestartMenuSwitch()
    {
        StartCoroutine(ButtonAnim(restartSwitchRect));
        systemMessage.text = "戦闘を始めからやり直しますか？";
        yesText.text = "再挑戦(Y)";
        noText.text = "戻る(N)";
        messageNumber = 1;
        StartCoroutine(Delay(systemMessageObject, true));
        isMessageDisplay = true;
        seSource.clip = seUIClick;
        seSource.Play();
    }
    //撤退(メニューから選択)
    public void RetreatMenuSwitch()
    {
        if (!growMask1.activeSelf)
        {
            StartCoroutine(ButtonAnim(retreatSwitchRect));
            systemMessage.text = "撤退しますか？\n(直前の場面に移り育成が行えます)";
            yesText.text = "撤退(Y)";
            noText.text = "戻る(N)";
            messageNumber = 2;
            StartCoroutine(Delay(systemMessageObject, true));
            isMessageDisplay = true;
            seSource.clip = seUIClick;
            seSource.Play();
        }
        else
        {
            seSource.clip = seUIUnactive;
            seSource.Play();
        }
    }
    //デバッグ用スキップ
    public void DebugMenuSwitch()
    {
        StartCoroutine(ButtonAnim(debugSwitchRect));
        systemMessage.text = "戦闘をスキップしますか？\n(ゲームを評価してくださる方向けの機能です！！)";
        yesText.text = "スキップ(Y)";
        noText.text = "戻る(N)";
        messageNumber = 3;
        StartCoroutine(Delay(systemMessageObject, true));
        isMessageDisplay = true;
        seSource.clip = seUIClick;
        seSource.Play();
    }

    //Yesボタンを押したときの機能
    public void YesSwitch()
    {
        if (!isSelected && !switchInterval)
        {
            isSelected = true;
            StartCoroutine(ButtonAnim(yesSwitch));
            seSource.clip = seUIClick;
            seSource.Play();
            switch (messageNumber)
            {
                //ゲームオーバーからの再挑戦
                case 0:
                    StartCoroutine(ReloadScene());
                    break;
                //メニューからの再挑戦
                case 1:
                    StartCoroutine(ReloadScene());
                    break;
                //撤退し直前の場面に戻る
                case 2:
                    StartCoroutine(GoBackStory());
                    break;
                //デバッグ用スキップ
                case 3:
                    StartCoroutine(SceneSkip());
                    break;
                default:
                    break;
            }
        }
    }
    //Noボタンを押したときの機能
    public void NoSwitch()
    {
        if (!isSelected && !switchInterval)
        {
            StartCoroutine(SwitchInterval());
            seSource.clip = seUIBack;
            seSource.Play();
            switch (messageNumber)
            {
                //ゲームオーバーから育成へ
                case 0:
                    if (!growMask2.activeSelf)
                    {
                        StartCoroutine(ButtonAnim(noSwitch));
                        StartCoroutine(GoBackStory());
                        isSelected = true;
                    }
                    else
                    {
                        seSource.clip = seUIUnactive;
                        seSource.Play();
                    }
                    break;
                //戻る
                default:
                    StartCoroutine(ButtonAnim(noSwitch));
                    StartCoroutine(Delay(systemMessageObject, false));
                    isMessageDisplay = false;
                    break;
            }
        }
    }

    //少し待って再読み込み
    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //少し待ってスキップ
    private IEnumerator SceneSkip()
    {
        yield return new WaitForSeconds(0.1f);
        bSManager.SceneLoad();
    }
    //少し待って直前の場面へ
    private IEnumerator GoBackStory()
    {
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeOut(1, black));
        if (GameManager.instance.SceneName == null)
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene(GameManager.instance.SceneName);
        }
    }

    public void MenuOn()
    {
        menu.SetActive(true);
        isFunctionAvailable = true;
    }
    public void MenuOff()
    {
        menu.SetActive(false);
        functions.SetActive(false);
        systemMessageObject.SetActive(false);
        isMessageDisplay = false;
        isFunctionAvailable = false;
    }
}
