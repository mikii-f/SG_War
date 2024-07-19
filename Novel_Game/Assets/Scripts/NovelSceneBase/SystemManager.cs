using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SystemManager : MonoBehaviour
{
    [SerializeField] private TextManagerOrigin textManager;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject functions;
    [SerializeField] private RectTransform speedUpSwitchRect;
    [SerializeField] private Image speedUpSwitchImage;
    [SerializeField] private Text speedUpSwitchText;
    [SerializeField] private RectTransform skipSwitchRect;
    [SerializeField] private RectTransform logSwitchRect;
    [SerializeField] private RectTransform growSwitchRect;
    [SerializeField] private RectTransform saveSwitchRect;
    [SerializeField] private RectTransform titleSwitchRect;
    [SerializeField] private GameObject logTextObject;
    [SerializeField] private GameObject systemMessageObject;
    [SerializeField] private Text systemMessage;
    [SerializeField] private RectTransform yesSwitch;
    [SerializeField] private RectTransform noSwitch;
    [SerializeField] private GameObject saveSuccessed;
    private int messageNumber;
    private bool isMessageDisplay = false;
    private bool isFunctionAvailable = false;

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        functions.SetActive(false);
        logTextObject.SetActive(false);
        systemMessageObject.SetActive(false);
        saveSuccessed.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //アニメーション中にセーブが行えても問題ないのかどうか

        //キー入力によるシステム操作(テキスト側から禁止されていないとき)(UI操作は自然とできなくなっているはず)
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
            if (functions.activeSelf && !isMessageDisplay)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    SpeedUpSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    SkipSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    LogSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    GrowSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    SaveSwitch();
                }
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    TitleSwitch();
                }
            }
            //ログを閉じる
            if (logTextObject.activeSelf && Input.GetKeyDown(KeyCode.Alpha3))
            {
                LogClose();
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

    public void MenuSwitch()
    {
        functions.SetActive(true);
        textManager.FunctionsOpen = true;
    }
    public void SpeedUpSwitch()
    {
        StartCoroutine(ButtonAnim(speedUpSwitchRect));
        if (!textManager.IsSpeedUp)
        {
            textManager.IsSpeedUp = true;
            speedUpSwitchImage.color = Color.white;
            speedUpSwitchText.color = Color.black;
        }
        else
        {
            textManager.IsSpeedUp = false;
            speedUpSwitchImage.color = new(0, 0, 40f / 255f, 1);
            speedUpSwitchText.color = Color.white;
        }
    }
    public void SkipSwitch() 
    {
        StartCoroutine(ButtonAnim(skipSwitchRect));
        systemMessage.text = "このシーンをスキップしますか？";
        messageNumber = 0;
        systemMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    public void LogSwitch() 
    {
        StartCoroutine(ButtonAnim(logSwitchRect));
        StartCoroutine(LogOnOff());
    }
    public void LogClose()
    {
        StartCoroutine(LogOnOff());
    }
    //開けてすぐ閉じてしまうとかがないように1フレーム空ける
    private IEnumerator LogOnOff()
    {
        yield return null;
        logTextObject.SetActive(!logTextObject.activeSelf);
        isMessageDisplay = !isMessageDisplay;
    }
    public void GrowSwitch()
    {
        StartCoroutine(ButtonAnim(growSwitchRect));
        systemMessage.text = "育成に向かいますか？";
        messageNumber = 1;
        systemMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    public void SaveSwitch()
    {
        StartCoroutine(ButtonAnim(saveSwitchRect));
        systemMessage.text = "進行度を保存しますか？";
        messageNumber = 2;
        systemMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    public void TitleSwitch() 
    {
        StartCoroutine(ButtonAnim(titleSwitchRect));
        systemMessage.text = "タイトルに戻りますか？\n(自動でセーブされます)";
        messageNumber = 3;
        systemMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    public void FunctionsClose()
    {
        functions.SetActive(false);
        StartCoroutine(CloseInterval());
    }
    private IEnumerator CloseInterval() //閉じた時点ではテキストが進まないようにする
    {
        yield return null;
        textManager.FunctionsOpen = false;
    }
    public void YesSwitch()
    {
        StartCoroutine(ButtonAnim(yesSwitch));
        switch (messageNumber)
        {
            //スキップ
            case 0:
                StartCoroutine(textManager.SceneSkip());
                break;
            //育成へ
            case 1:
                StartCoroutine(textManager.GoToGrow());
                break;
            //セーブ
            case 2:
                textManager.Save();
                StartCoroutine(SaveSuccessed());
                break;
            //タイトルへ
            case 3:
                StartCoroutine(textManager.GoBackTitle());
                break;
            default:
                break;
        }

    }
    public void NoSwitch()
    {
        StartCoroutine(ButtonAnim(noSwitch));
        systemMessageObject.SetActive(false);
        isMessageDisplay = false;
    }
    //セーブ成功を伝えるメッセージ
    private IEnumerator SaveSuccessed()
    {
        yield return new WaitForSeconds(0.15f);
        systemMessageObject.SetActive(false);
        isMessageDisplay= false;
        saveSuccessed.SetActive(true);
        yield return new WaitForSeconds(3);
        saveSuccessed.SetActive(false);
    }

    //ボタンのアニメーション
    private IEnumerator ButtonAnim(RectTransform rect)
    {
        Vector2 temp = rect.localScale;
        rect.localScale = new(0.9f * temp.x, 0.9f * temp.y);
        yield return new WaitForSeconds(0.1f);
        rect.localScale = temp;
    }

    //テキスト側からメニューの表示非表示
    public void MenuOn()
    {
        menu.SetActive(true);
        isFunctionAvailable = true;
    }
    public void MenuOff()
    {
        menu.SetActive(false);
        functions.SetActive(false);
        logTextObject.SetActive(false);
        systemMessageObject.SetActive(false);
        isMessageDisplay = false;
        isFunctionAvailable = false;
    }
}
