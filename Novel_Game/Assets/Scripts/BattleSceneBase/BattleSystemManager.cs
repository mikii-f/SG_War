using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSystemManager : MonoBehaviour
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
    private int messageNumber;
    private bool isMessageDisplay = false;
    private bool isFunctionAvailable = true;

    // Start is called before the first frame update
    void Start()
    {
        functions.SetActive(false);
        systemMessageObject.SetActive(false);
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
        functions.SetActive(true);
    }
    public void FunctionsClose()
    {
        functions.SetActive(false);
    }
    //ゲームオーバー(バトルシーンから呼び出し)
    public void GameOver()
    {
        systemMessage.text = "GAME OVER\n再挑戦しますか？";
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
        systemMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    //撤退(メニューから選択)
    public void RetreatMenuSwitch()
    {
        StartCoroutine(ButtonAnim(retreatSwitchRect));
        systemMessage.text = "撤退しますか？\n(直前の場面に移り育成が行えます)";
        yesText.text = "撤退(Y)";
        noText.text = "戻る(N)";
        messageNumber = 2;
        systemMessageObject.SetActive(true);
        isMessageDisplay = true;
    }
    //デバッグ用スキップ
    public void DebugMenuSwitch()
    {
        StartCoroutine(ButtonAnim(debugSwitchRect));
        systemMessage.text = "戦闘をスキップしますか？\n(ゲームを評価してくださる方向けの機能です！！)";
        yesText.text = "スキップ(Y)";
        noText.text = "戻る(N)";
        messageNumber = 3;
        systemMessageObject.SetActive(true);
        isMessageDisplay = true;
    }

    //Yesボタンを押したときの機能
    public void YesSwitch()
    {
        StartCoroutine(ButtonAnim(yesSwitch));
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
                systemMessageObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    //Noボタンを押したときの機能
    public void NoSwitch()
    {
        StartCoroutine(ButtonAnim(noSwitch));
        switch (messageNumber)
        {
            //ゲームオーバーから育成へ
            case 0:
                StartCoroutine(GoBackStory());
                break;
            //戻る
            default:
                StartCoroutine(Back(systemMessageObject));
                break;
        }
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
    //少し待って直前の場面へ
    private IEnumerator GoBackStory()
    {
        yield return new WaitForSeconds(0.15f);
        if (GameManager.instance.SceneName == null)
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene(GameManager.instance.SceneName);
        }
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
        systemMessageObject.SetActive(false);
        isMessageDisplay = false;
        isFunctionAvailable = false;
    }
}
