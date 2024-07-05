using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LeaderManager : MonoBehaviour
{
    [SerializeField] private GameObject sainManagerObject;
    private SainManager sainManager;
    [SerializeField] private GameObject HPAssist;
    [SerializeField] private GameObject attackAssist;
    [SerializeField] private GameObject speedAssist;
    [SerializeField] private GameObject guard;
    private RectTransform HPRect;
    private RectTransform attackRect;
    private RectTransform speedRect;
    private RectTransform guardRect;
    [SerializeField] private GameObject HPIntervalDisplay;
    [SerializeField] private GameObject attackIntervalDisplay;
    [SerializeField] private GameObject speedIntervalDisplay;
    [SerializeField] private GameObject guardIntervalDisplay;
    private Text HPIntervalText;
    private Text attackIntervalText;
    private Text speedIntervalText;
    private Text guardIntervalText;
    private float assistInterval = 60f;
    private float guardInterval = 4f;
    private float HPIntervalCount = 0f;
    private float attackIntervalCount = 0f;
    private float speedIntervalCount = 0f;
    private float guardIntervalCount = 0f;
    private bool pause = true;
    public bool Pause { set { pause = value; } }

    // Start is called before the first frame update
    void Start()
    {
        sainManager = sainManagerObject.GetComponent<SainManager>();
        HPRect = HPAssist.GetComponent<RectTransform>();
        attackRect = attackAssist.GetComponent<RectTransform>();
        speedRect = speedAssist.GetComponent<RectTransform>();
        guardRect = guard.GetComponent<RectTransform>();
        HPIntervalText = HPIntervalDisplay.GetComponentInChildren<Text>();
        attackIntervalText = attackIntervalDisplay.GetComponentInChildren<Text>();
        speedIntervalText = speedIntervalDisplay.GetComponentInChildren<Text>();
        guardIntervalText = guardIntervalDisplay.GetComponentInChildren<Text>();
        HPIntervalDisplay.SetActive(false);
        attackIntervalDisplay.SetActive(false);
        speedIntervalDisplay.SetActive(false);
        guardIntervalDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //各種アシストのキー入力
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GuardClick();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HPAssistClick();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AttackAssistClick();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SpeedAssistClick();
        }

        //各種アシストのインターバル管理
        if (HPIntervalCount > 0 && !pause)
        {
            HPIntervalCount = Mathf.Max(0, HPIntervalCount - Time.deltaTime);
            HPIntervalText.text = HPIntervalCount.ToString("F2");
        }
        else if (HPIntervalCount == 0)
        {
            HPIntervalDisplay.SetActive(false);
        }
        if (attackIntervalCount > 0 && !pause) {
            attackIntervalCount = Mathf.Max(0, attackIntervalCount - Time.deltaTime);
            attackIntervalText.text = attackIntervalCount.ToString("F2");
        }
        else if (attackIntervalCount == 0)
        {
            attackIntervalDisplay.SetActive(false);
        }
        if (speedIntervalCount > 0 && !pause)
        {
            speedIntervalCount = Mathf.Max(0,speedIntervalCount - Time.deltaTime);
            speedIntervalText.text = speedIntervalCount.ToString("F2");
        }
        else if (speedIntervalCount == 0)
        {
            speedIntervalDisplay.SetActive(false);
        }
        if (guardIntervalCount > 0 && !pause)
        {
            guardIntervalCount = Mathf.Max(0, guardIntervalCount - Time.deltaTime);
            guardIntervalText.text = guardIntervalCount.ToString("F2");
        }
        else if (guardIntervalCount == 0)
        {
            guardIntervalDisplay.SetActive(false);
        }
    }

    //各種アシストの選択判定
    public void HPAssistClick()
    {
        if (HPIntervalCount == 0 && !pause)
        {
            HPIntervalCount = assistInterval;
            StartCoroutine(ButtonAnim(HPRect));
            sainManager.ReceiveHPAssist();
            HPIntervalDisplay.SetActive(true);
        }
    }
    public void AttackAssistClick()
    {
        if (attackIntervalCount == 0 && !pause)
        {
            attackIntervalCount = assistInterval;
            StartCoroutine(ButtonAnim(attackRect));
            StartCoroutine(sainManager.ReceiveAttackAssist());
            attackIntervalDisplay.SetActive(true);
        }
    }
    public void SpeedAssistClick()
    {
        if (speedIntervalCount == 0 && !pause)
        {
            speedIntervalCount = assistInterval;
            StartCoroutine(ButtonAnim(speedRect));
            StartCoroutine(sainManager.ReceiveSpeedAssist());
            speedIntervalDisplay.SetActive(true);
        }
    }
    public void GuardClick()
    {
        if (guardIntervalCount == 0 && !pause)
        {
            guardIntervalCount = guardInterval;
            StartCoroutine(ButtonAnim(guardRect));
            StartCoroutine(sainManager.ReceiveGuard());
            guardIntervalDisplay.SetActive(true);
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
}
