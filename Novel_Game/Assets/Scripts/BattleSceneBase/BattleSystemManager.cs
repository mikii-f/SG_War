using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSystemManager : MonoBehaviour
{
    [SerializeField] private BattleSceneManagerOrigin bSManager;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject gameOverMessageObject;
    public bool GameOverMessageObject { set { gameOverMessageObject.SetActive(value); } }
    [SerializeField] private RectTransform gOYesSwitchRect;
    [SerializeField] private RectTransform gONoSwitchRect;
    [SerializeField] private GameObject functions;
    [SerializeField] private RectTransform restartSwitchRect;
    [SerializeField] private GameObject restartMessageObject;
    [SerializeField] private RectTransform restartYesSwitchRect;
    [SerializeField] private RectTransform restartNoSwitchRect;
    [SerializeField] private RectTransform retreatSwitchRect;
    [SerializeField] private GameObject retreatMessageObject;
    [SerializeField] private RectTransform retreatYesSwitchRect;
    [SerializeField] private RectTransform retreatNoSwitchRect;
    [SerializeField] private RectTransform debugSwitchRect;
    [SerializeField] private GameObject debugMessageObject;
    [SerializeField] private RectTransform debugYesSwitchRect;
    [SerializeField] private RectTransform debugNoSwitchRect;
    private bool isMessageDisplay = false;
    private bool isFunctionAvailable = true;

    // Start is called before the first frame update
    void Start()
    {
        gameOverMessageObject.SetActive(false);
        functions.SetActive(false);
        restartMessageObject.SetActive(false);
        retreatMessageObject.SetActive(false);
        debugMessageObject.SetActive(false);
    }

    // Update is called once per frame
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
            if (functions.activeSelf && !isMessageDisplay)
            {
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
                if (gameOverMessageObject.activeSelf)
                {
                    GORestartSwitch();
                }
                else if (restartMessageObject.activeSelf)
                {
                    RestartYesSwitch();
                }
                else if (retreatMessageObject.activeSelf)
                {
                    RetreatYesSwitch();
                }
                else if (debugMessageObject.activeSelf)
                {
                    DebugYesSwitch();
                }
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                if (gameOverMessageObject.activeSelf)
                {
                    GOGrowSwitch();
                }
                else if (restartMessageObject.activeSelf)
                {
                    RestartNoSwitch();
                }
                else if (retreatMessageObject.activeSelf)
                {
                    RetreatNoSwitch();
                }
                else if (debugMessageObject.activeSelf)
                {
                    DebugNoSwitch();
                }
            }
        }
    }

    //再挑戦ボタン(ゲームオーバー時)
    public void GORestartSwitch()
    {
        StartCoroutine(ButtonAnim(gOYesSwitchRect));
        StartCoroutine(ReloadScene());
    }
    //育成に向かう(ゲームオーバー時)
    public void GOGrowSwitch()
    {
        StartCoroutine(ButtonAnim(gONoSwitchRect));
    }
    //メニュー開閉
    public void MenuSwitch()
    {
        functions.SetActive(true);
    }
    public void FunctionsClose()
    {
        functions.SetActive(false);
    }
    //再挑戦(メニューから選択)
    public void RestartMenuSwitch()
    {
        StartCoroutine(ButtonAnim(restartSwitchRect));
        restartMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    public void RestartYesSwitch()
    {
        StartCoroutine(ButtonAnim(restartYesSwitchRect));
        StartCoroutine(ReloadScene());
    }
    public void RestartNoSwitch()
    {
        StartCoroutine(ButtonAnim((restartNoSwitchRect)));
        StartCoroutine(Back(restartMessageObject));
    }
    //撤退(メニューから選択)
    public void RetreatMenuSwitch()
    {
        StartCoroutine(ButtonAnim(retreatSwitchRect));
        retreatMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    public void RetreatYesSwitch()
    {
        StartCoroutine(ButtonAnim(retreatYesSwitchRect));
        //直前の場面に戻る処理
    }
    public void RetreatNoSwitch()
    {
        StartCoroutine(ButtonAnim((retreatNoSwitchRect)));
        StartCoroutine(Back(retreatMessageObject));
    }
    //デバッグ用スキップ
    public void DebugMenuSwitch()
    {
        StartCoroutine(ButtonAnim(debugSwitchRect));
        debugMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    public void DebugYesSwitch()
    {
        StartCoroutine(ButtonAnim(debugYesSwitchRect));
        StartCoroutine(SceneSkip());
        debugMessageObject.SetActive(false);
    }
    public void DebugNoSwitch()
    {
        StartCoroutine(ButtonAnim((debugNoSwitchRect)));
        StartCoroutine(Back(debugMessageObject));
    }

    //少し待って再読み込み
    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(0.15f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //少し待って閉じる
    private IEnumerator Back(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.15f);
        gameObject.SetActive(false);
        isMessageDisplay = false;
    }
    //少し待ってスキップ
    private IEnumerator SceneSkip()
    {
        yield return new WaitForSeconds(0.15f);
        bSManager.SceneLoad();
    }

    //ボタンのアニメーション
    private IEnumerator ButtonAnim(RectTransform rect)
    {
        Vector2 temp = rect.localScale;
        rect.localScale = new(0.9f * temp.x, 0.9f * temp.y);
        yield return new WaitForSeconds(0.1f);
        rect.localScale = temp;
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
        restartMessageObject.SetActive(false);
        retreatMessageObject.SetActive(false);
        debugMessageObject.SetActive(false);
        isMessageDisplay = false;
        isFunctionAvailable = false;
    }
}
