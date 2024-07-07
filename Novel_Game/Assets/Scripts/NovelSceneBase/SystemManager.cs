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
    [SerializeField] private GameObject skipMessageObject;
    [SerializeField] private RectTransform skipNoSwitchRect;
    [SerializeField] private RectTransform skipYesSwitchRect;
    [SerializeField] private GameObject logTextObject;
    private bool isMessageDisplay = false;
    private bool isFunctionAvailable = false;

    // Start is called before the first frame update
    void Start()
    {
        functions.SetActive(false);
        skipMessageObject.SetActive(false);
        menu.SetActive(false);
        logTextObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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
            //メッセージへの応答
            if (Input.GetKeyDown(KeyCode.Y))
            {
                if (skipMessageObject.activeSelf)
                {
                    SkipYesSwitch();
                }
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                if (skipMessageObject.activeSelf)
                {
                    SkipNoSwitch();
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
        skipMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    public void LogSwitch() 
    {
        StartCoroutine(ButtonAnim(logSwitchRect));
        logTextObject.SetActive(true);
        isMessageDisplay = true;
    }
    public void LogClose()
    {
        logTextObject.SetActive(false);
        isMessageDisplay = false;
    }
    public void GrowSwitch()
    {
        StartCoroutine(ButtonAnim(growSwitchRect));
    }
    public void SaveSwitch()
    {
        StartCoroutine(ButtonAnim(saveSwitchRect));
    }
    public void TitleSwitch() 
    {
        StartCoroutine(ButtonAnim(titleSwitchRect)  );
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
    public void SkipNoSwitch()
    {
        StartCoroutine(ButtonAnim(skipNoSwitchRect));
        skipMessageObject.SetActive(false);
        isMessageDisplay = false;
    }
    public void SkipYesSwitch()
    {
        StartCoroutine(ButtonAnim(skipYesSwitchRect));
        StartCoroutine(textManager.SceneSkip());
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
        skipMessageObject.SetActive(false);
        isMessageDisplay = false;
        isFunctionAvailable = false;
    }
}
