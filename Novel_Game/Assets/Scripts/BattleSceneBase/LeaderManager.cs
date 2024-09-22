using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderManager : SystemManagerOrigin
{
    [SerializeField] private SainManager sainManager;
    [SerializeField] private GameObject autoObject;
    private RectTransform autoRect;
    private Image autoImage;
    [SerializeField] private RectTransform HPRect;
    [SerializeField] private RectTransform attackRect;
    [SerializeField] private RectTransform speedRect;
    [SerializeField] private RectTransform guardRect;
    [SerializeField] private GameObject HPIntervalDisplay;
    [SerializeField] private GameObject attackIntervalDisplay;
    [SerializeField] private GameObject speedIntervalDisplay;
    [SerializeField] private GameObject guardIntervalDisplay;
    private Text HPIntervalText;
    private Text attackIntervalText;
    private Text speedIntervalText;
    private Text guardIntervalText;
    private const float assistInterval = 60f;
    private const float guardInterval = 4f;
    private float HPIntervalCount = 0f;
    private float attackIntervalCount = 0f;
    private float speedIntervalCount = 0f;
    private float guardIntervalCount = 0f;
    private bool pause = true;
    public bool Pause { set { pause = value; } }

    // Start is called before the first frame update
    void Start()
    {
        autoRect = autoObject.GetComponent<RectTransform>();
        autoImage = autoObject.GetComponent<Image>();
        HPIntervalText = HPIntervalDisplay.GetComponentInChildren<Text>();
        attackIntervalText = attackIntervalDisplay.GetComponentInChildren<Text>();
        speedIntervalText = speedIntervalDisplay.GetComponentInChildren<Text>();
        guardIntervalText = guardIntervalDisplay.GetComponentInChildren<Text>();
        HPIntervalDisplay.SetActive(false);
        attackIntervalDisplay.SetActive(false);
        speedIntervalDisplay.SetActive(false);
        guardIntervalDisplay.SetActive(false);
        //1戦目はガードしかできない
        if (SceneManager.GetActiveScene().name == "BattleScene1")
        {
            HPIntervalDisplay.SetActive(true);
            attackIntervalDisplay.SetActive(true);
            speedIntervalDisplay.SetActive(true);
            HPIntervalCount = 99.99f;
            attackIntervalCount = 99.99f;
            speedIntervalCount = 99.99f;
            HPIntervalText.text = HPIntervalCount.ToString("F2");
            attackIntervalText.text = attackIntervalCount.ToString("F2");
            speedIntervalText.text = speedIntervalCount.ToString("F2");
        }
        seSource = GetComponent<AudioSource>();
        seSource.volume = GameManager.instance.SeVolume;
    }

    // Update is called once per frame
    void Update()
    {
        //各種アシスト及びオート機能のキー入力
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
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AutoClick();
        }

        //各種アシストのインターバル管理(1戦目はガードのみ)
        if (SceneManager.GetActiveScene().name != "BattleScene1")
        {
            if (HPIntervalCount > 0 && !pause)
            {
                HPIntervalCount = Mathf.Max(0, HPIntervalCount - Time.deltaTime);
                HPIntervalText.text = HPIntervalCount.ToString("F2");
            }
            else if (HPIntervalCount == 0)
            {
                HPIntervalDisplay.SetActive(false);
            }
            if (attackIntervalCount > 0 && !pause)
            {
                attackIntervalCount = Mathf.Max(0, attackIntervalCount - Time.deltaTime);
                attackIntervalText.text = attackIntervalCount.ToString("F2");
            }
            else if (attackIntervalCount == 0)
            {
                attackIntervalDisplay.SetActive(false);
            }
            if (speedIntervalCount > 0 && !pause)
            {
                speedIntervalCount = Mathf.Max(0, speedIntervalCount - Time.deltaTime);
                speedIntervalText.text = speedIntervalCount.ToString("F2");
            }
            else if (speedIntervalCount == 0)
            {
                speedIntervalDisplay.SetActive(false);
            }
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
            seSource.clip = seUIClick;
            seSource.Play();
        }
        else if (!pause)
        {
            seSource.clip = seUIUnactive;
            seSource.Play();
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
            seSource.clip = seUIClick;
            seSource.Play();
        }
        else if (!pause)
        {
            seSource.clip = seUIUnactive;
            seSource.Play();
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
            seSource.clip = seUIClick;
            seSource.Play();
        }
        else if (!pause)
        {
            seSource.clip = seUIUnactive;
            seSource.Play();
        }
    }
    public void GuardClick()
    {
        //if (guardIntervalCount == 0 && !pause && !sainManager.IsCannotGuard)
        if (guardIntervalCount == 0 && !pause)
        {
            guardIntervalCount = guardInterval;
            StartCoroutine(ButtonAnim(guardRect));
            StartCoroutine(sainManager.ReceiveGuard());
            guardIntervalDisplay.SetActive(true);
            seSource.clip = seUIClick;
            seSource.Play();
        }
        else if (!pause)
        {
            seSource.clip = seUIUnactive;
            seSource.Play();
        }
    }
    public void AutoClick()
    {
        if (!pause)
        {
            StartCoroutine(ButtonAnim(autoRect));
            seSource.clip = seUIClick;
            seSource.Play();
            if (!sainManager.Auto)
            {
                sainManager.Auto = true;
                autoImage.color = Color.white;
            }
            else
            {
                sainManager.Auto = false;
                autoImage.color = new(100f / 255, 100f / 255, 100f / 255, 1f);
            }
        }
    }
}