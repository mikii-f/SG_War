using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectSceneManager : SystemManagerOrigin
{
    [SerializeField] private RectTransform normalSwitchRect;
    [SerializeField] private RectTransform hardSwitchRect;
    [SerializeField] private GameObject hardSwitchMask;
    [SerializeField] private RectTransform storySwitchRect;
    [SerializeField] private Text statusText;
    [SerializeField] private GameObject systemMessageObject;
    [SerializeField] private RectTransform yesSwitch;
    [SerializeField] private RectTransform noSwitch;
    [SerializeField] private GameObject developingMessage;
    [SerializeField] private Image black;
    [SerializeField] private TMP_Text countDown;
    private AudioSource audioSource;
    [SerializeField] private AudioClip seCountDown;
    private bool go = false;
    private bool hard = false;

    private void Start()
    {
        systemMessageObject.SetActive(false);
        developingMessage.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.instance.BgmVolume;
        seSource.volume = GameManager.instance.SeVolume;
        statusText.text = "体力\n" + GameManager.instance.SainHP.ToString() + "\n初期SG\n" + GameManager.instance.SainSG.ToString() + "\n攻撃力\n" + GameManager.instance.SainAttack.ToString() + "\n経験値\n" + GameManager.instance.EXP.ToString();
        if (GameManager.instance.EXP > 3000)
        {
            hardSwitchMask.SetActive(false);
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
    public void NormalSwitch()
    {
        if (!go)
        {
            StartCoroutine(ButtonAnim(normalSwitchRect));
            StartCoroutine(Delay(systemMessageObject, true));
            seSource.clip = seUIClick;
            seSource.Play();
            hard = false;
        }
    }
    public void HardSwitch()
    {
        if (!go && !developingMessage.activeSelf)
        {
            if (hardSwitchMask.activeSelf)
            {
                StartCoroutine(Developing());
                seSource.clip = seUIUnactive;
                seSource.Play();
            }
            else
            {
                StartCoroutine(ButtonAnim(hardSwitchRect));
                StartCoroutine(Delay(systemMessageObject, true));
                seSource.clip = seUIClick;
                seSource.Play();
                hard = true;
            }
        }
    }
    //今はノーマルステージのみに対応
    public void YesSwitch()
    {
        if (!go && !switchInterval)
        {
            StartCoroutine(ButtonAnim(yesSwitch));
            StartCoroutine(Delay(systemMessageObject, false));
            StartCoroutine(StartGame());
            go = true;
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }
    //ゲーム開始
    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.1f);
        countDown.text = "3";
        yield return new WaitForSeconds(1);
        countDown.text = "2";
        seSource.clip = seCountDown;
        seSource.Play();
        yield return new WaitForSeconds(1);
        countDown.text = "1";
        seSource.clip = seCountDown;
        seSource.Play();
        yield return new WaitForSeconds(1);
        if (hard)
        {
            SceneManager.LoadScene("3DGameScene2");
        }
        else
        {
            SceneManager.LoadScene("3DGameScene1");
        }
    }
    public void NoSwitch()
    {
        if (!go && !switchInterval)
        {
            StartCoroutine(SwitchInterval());
            StartCoroutine(ButtonAnim(noSwitch));
            StartCoroutine(Delay(systemMessageObject, false));
            seSource.clip = seUIBack;
            seSource.Play();
        }
    }

    private IEnumerator Developing()
    {
        developingMessage.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        developingMessage.SetActive(false);
    }
    public void StorySwitch()
    {
        if (!go)
        {
            go = true;
            StartCoroutine(ButtonAnim(storySwitchRect));
            StartCoroutine(GoToStory());
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }
    private IEnumerator GoToStory()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(VolumeFadeOut(1, audioSource));
        yield return StartCoroutine(FadeOut(1f, black));
        //本来起こらない状況と思われるが暫定措置
        if (GameManager.instance.SceneName == null)
        {
            SceneManager.LoadScene("TitleScene");
        }
        else
        {
            SceneManager.LoadScene(GameManager.instance.SceneName);
        }
    }
}
