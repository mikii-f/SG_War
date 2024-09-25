using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Stage1Manager : StageManagerOrigin
{
    [SerializeField] private Image black;
    [SerializeField] private TMP_Text goalText;
    [SerializeField] private GameObject transparentWall;
    private MeshRenderer wallMeshRenderer;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] protected Text scoreText;
    protected int score = 0;
    [SerializeField] private Slider expSlider;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private Text messageText;
    [SerializeField] private GameObject function;
    [SerializeField] private Text functionText2;
    [SerializeField] private GameObject function2;
    [SerializeField] private Text functionMessageText;
    private int functionNumber = 0;
    private AudioSource audioSource;
    [SerializeField] private AudioClip seCymbal;

    private void Start()
    {
        wallMeshRenderer = transparentWall.GetComponent<MeshRenderer>();
        wallMeshRenderer.enabled = false;
        transparentWall.SetActive(false);
        resultPanel.SetActive(false);
        messagePanel.SetActive(false);
        function.SetActive(false);
        function2.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.instance.BgmVolume;
        seSource.volume = GameManager.instance.SeVolume;
        if (GameManager.instance.EXP == 0)
        {
            functionText2.text = "";
        }
    }
    private void Update()
    {
        if (!clear)
        {
            time += Time.deltaTime;
            timeText.text = "Time: " + time.ToString("F2");
            //メニュー表示非表示
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (!function.activeSelf)
                {
                    function.SetActive(true);
                    seSource.clip = seUIClick;
                    seSource.Play();
                }
                else if (!function2.activeSelf)
                {
                    function.SetActive(false);
                    seSource.clip = seUIBack;
                    seSource.Play();
                }
            }
            //各機能
            if (Input.GetKeyDown(KeyCode.Alpha1) && function.activeSelf && !function2.activeSelf)
            {
                function2.SetActive(true);
                functionMessageText.text = "再挑戦しますか？";
                functionNumber = 1;
                seSource.clip = seUIClick;
                seSource.Play();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && function.activeSelf && !function2.activeSelf)
            {
                if (GameManager.instance.EXP != 0)
                {
                    function2.SetActive(true);
                    functionMessageText.text = "ステージセレクトに戻りますか？";
                    functionNumber = 2;
                    seSource.clip = seUIClick;
                    seSource.Play();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0) && function.activeSelf && !function2.activeSelf)
            {
                function2.SetActive(true);
                functionMessageText.text = "(ゲームを評価してくださる方向けの機能です)\nスキップしてスコア3000を獲得しますか？";
                functionNumber = 0;
                seSource.clip = seUIClick;
                seSource.Play();
            }
            //YesNo選択
            if (function2.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    seSource.clip = seUIClick;
                    seSource.Play();
                    switch (functionNumber)
                    {
                        case 1:
                            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                            break;
                        case 2:
                            SceneManager.LoadScene("3DGameSelectScene");
                            break;
                        case 0:
                            Skip();
                            break;
                    }

                }
                else if (Input.GetKeyDown(KeyCode.N))
                {
                    function2.SetActive(false);
                    seSource.clip = seUIBack;
                    seSource.Play();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !transparentWall.activeSelf)
        {
            StartCoroutine(Goal());
            transparentWall.SetActive(true);
        }
    }
    private IEnumerator Goal()
    {
        playerManager.Clear = true;
        clear = true;
        function.SetActive(false);
        function2.SetActive(false);
        goalText.text = "GOAL";
        seSource.clip = seCymbal;
        seSource.Play();
        yield return new WaitForSeconds(4);
        goalText.text = "";
        ScoreText();
        StartCoroutine(GoNext());
    }
    protected virtual void ScoreText()
    {
        score = Mathf.Max((int)(2000 * medalCount / 100 * 60 / time), 1);
        scoreText.text = "メダル獲得：" + medalCount.ToString() + "枚\n\nタイム：" + time.ToString("F2") + "s\n\n\nスコア：" + score.ToString() + "\n累計EXP：" + (GameManager.instance.EXP + score).ToString();
    }
    private void Skip()
    {
        playerManager.Clear = true;
        clear = true;
        score = 3000;
        function.SetActive(false);
        function2.SetActive(false);
        scoreText.text = "メダル獲得： 枚\n\nタイム： s\n\n\nスコア：" + score.ToString() + "\n累計EXP：" + (GameManager.instance.EXP + score).ToString();
        StartCoroutine(GoNext());
    }
    private IEnumerator GoNext()
    {
        resultPanel.SetActive(true);
        expSlider.value = (float)(GameManager.instance.EXP + score) / 15000;
        //レベルアップ時の処理(特に、初回時のみストーリーの遷移先を切り替える)
        yield return new WaitForSeconds(2);
        if (GameManager.instance.EXP == 0)
        {
            seSource.clip = seCymbal;
            seSource.Play();
            GameManager.instance.EXP += score;
            GameManager.instance.LineNumber = 0;
            GameManager.instance.SceneName = "MainScene2_3";
            GameManager.instance.Save();
            messageText.text = "スキル2・3が開放されました\n必殺技が開放されました\nリーダースキル「体力支援」「攻撃支援」「速度支援」が開放されました";
            messagePanel.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space));
            messagePanel.SetActive(false);
            seSource.clip = seUIClick;
            seSource.Play();
            yield return new WaitForSeconds(1);
        }
        else if (GameManager.instance.EXP < 5000)
        {
            GameManager.instance.EXP += score;
            if (GameManager.instance.EXP >= 5000)
            {
                seSource.clip = seCymbal;
                seSource.Play();
                GameManager.instance.SainHP = 1400;
                GameManager.instance.SainAttack = 90;
                GameManager.instance.SainSG = 30;
                GameManager.instance.Save();
                messageText.text = "体力：1000→1400\n攻撃力：50→90\n初期SG：20→30";
                messagePanel.SetActive(true);
                yield return new WaitUntil(() => Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space));
                messagePanel.SetActive(false);
                seSource.clip = seUIClick;
                seSource.Play();
                yield return new WaitForSeconds(1);
            }
            else
            {
                GameManager.instance.Save();
            }
        }
        else if (GameManager.instance.EXP < 10000)
        {
            GameManager.instance.EXP += score;
            if (GameManager.instance.EXP >= 10000)
            {
                seSource.clip = seCymbal;
                seSource.Play();
                GameManager.instance.SainHP = 1700;
                GameManager.instance.SainAttack = 120;
                GameManager.instance.Save();
                messageText.text = "体力：1400→1700\n攻撃力：90→120";
                messagePanel.SetActive(true);
                yield return new WaitUntil(() => Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space));
                messagePanel.SetActive(false);
                seSource.clip = seUIClick;
                seSource.Play();
                yield return new WaitForSeconds(1);
            }
            else
            {
                GameManager.instance.Save();
            }
        }
        else if (GameManager.instance.EXP < 15000)
        {
            GameManager.instance.EXP += score;
            if (GameManager.instance.EXP >= 15000)
            {
                seSource.clip = seCymbal;
                seSource.Play();
                GameManager.instance.SainHP = 2000;
                GameManager.instance.SainAttack = 150;
                GameManager.instance.SainSG = 40;
                GameManager.instance.Save();
                messageText.text = "体力：1700→2000\n攻撃力：120→150\n初期SG：30→40";
                messagePanel.SetActive(true);
                yield return new WaitUntil(() => Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space));
                messagePanel.SetActive(false);
                seSource.clip = seUIClick;
                seSource.Play();
                yield return new WaitForSeconds(1);
            }
            else
            {
                GameManager.instance.Save();
            }
        }
        else
        {
            GameManager.instance.EXP += score;
            GameManager.instance.Save();
        }
        yield return new WaitUntil(() => Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space));
        StartCoroutine(VolumeFadeOut(1, audioSource));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return StartCoroutine(FadeOut(1, black));
        SceneManager.LoadScene("3DGameSelectScene");
    }
}
