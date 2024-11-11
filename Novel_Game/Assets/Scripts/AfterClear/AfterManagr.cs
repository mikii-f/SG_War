using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AfterManagr : SystemManagerOrigin
{
    [SerializeField] private RectTransform titleSwitchRect;
    [SerializeField] private RectTransform wordsSwitchRect;
    [SerializeField] private RectTransform growSwitchRect;
    [SerializeField] private RectTransform bonusSwitchRect;
    [SerializeField] private RectTransform plusAlphaSwitch;
    [SerializeField] private GameObject words2;
    [SerializeField] private GameObject bonus;
    [SerializeField] private GameObject battle;
    [SerializeField] private RectTransform battle1SwitchRect;
    [SerializeField] private RectTransform battle2SwitchRect;
    [SerializeField] private RectTransform battle3SwitchRect;
    [SerializeField] private RectTransform backSwitchRect;
    [SerializeField] private GameObject systemMessageObject;
    [SerializeField] private Text systemMessage;
    [SerializeField] private RectTransform yesSwitch;
    [SerializeField] private RectTransform noSwitch;
    [SerializeField] private Image black;
    private int messageNumber;
    private bool isGoNext = false;

    void Start()
    {
        systemMessageObject.SetActive(false);
        words2.SetActive(false);
        bonus.SetActive(false);
        battle.SetActive(false);
        black.color = Color.white;
        seSource.volume = GameManager.instance.SeVolume;
        StartCoroutine(FadeIn(0.5f, black));
    }

    void Update()
    {
        if (systemMessageObject.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                YesSwitch();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                NoSwitch();
            }
        }
    }

    public void TitleSwitch()
    {
        StartCoroutine(ButtonAnim(titleSwitchRect));
        systemMessage.text = "タイトルに戻りますか？";
        messageNumber = 0;
        StartCoroutine(Delay(systemMessageObject, true));
        seSource.clip = seUIClick;
        seSource.Play();
    }
    public void WordsSwitch()
    {
        StartCoroutine(ButtonAnim(wordsSwitchRect));
        StartCoroutine(Delay(words2, true));
        seSource.clip = seUIClick;
        seSource.Play();
    }
    public void GrowSwitchRect()
    {
        StartCoroutine(ButtonAnim(growSwitchRect));
        systemMessage.text = "育成に向かいますか？";
        messageNumber = 1;
        StartCoroutine(Delay(systemMessageObject, true));
        seSource.clip = seUIClick;
        seSource.Play();
    }
    public void BonusSwitch()
    {
        StartCoroutine(ButtonAnim(bonusSwitchRect));
        StartCoroutine(Delay(bonus, true));
        seSource.clip = seUIClick;
        seSource.Play();
    }
    public void PlusAlphaSwitch()
    {
        StartCoroutine(ButtonAnim(plusAlphaSwitch));
        StartCoroutine(Delay(battle, true));
        seSource.clip = seUIClick;
        seSource.Play();
    }
    public void YesSwitch()
    {
        if (!isGoNext && !switchInterval)
        {
            StartCoroutine(ButtonAnim(yesSwitch));
            seSource.clip = seUIClick;
            seSource.Play();
            switch (messageNumber)
            {
                case 0:
                    isGoNext = true;
                    StartCoroutine(GoBackTitle());
                    break;
                case 1:
                    isGoNext = true;
                    StartCoroutine(GoToGrow());
                    break;
            }
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
    //タイトルへ
    private IEnumerator GoBackTitle()
    {
        black.color = new(0, 0, 0, 0);
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeOut(1, black));
        SceneManager.LoadScene("TitleScene");
    }
    //育成へ
    private IEnumerator GoToGrow()
    {
        black.color = new(0, 0, 0, 0);
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeOut(0.5f, black));
        SceneManager.LoadScene("3DGameSelectScene");
    }
    //バトル1へ
    public void Battle1Switch()
    {
        if (!isGoNext)
        {
            isGoNext = true;
            StartCoroutine(ButtonAnim(battle1SwitchRect));
            StartCoroutine(GoToBattle1());
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }
    private IEnumerator GoToBattle1()
    {
        black.color = new(0, 0, 0, 0);
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeOut(0.5f, black));
        SceneManager.LoadScene("ExtraBattle1");
    }
    //バトル2へ
    public void Battle2Switch()
    {
        if (!isGoNext)
        {
            isGoNext = true;
            StartCoroutine(ButtonAnim(battle2SwitchRect));
            StartCoroutine(GoToBattle2());
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }
    private IEnumerator GoToBattle2()
    {
        black.color = new(0, 0, 0, 0);
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeOut(0.5f, black));
        SceneManager.LoadScene("ExtraBattle2");
    }
    //バトル3へ
    public void Battle3Switch()
    {
        if (!isGoNext)
        {
            isGoNext = true;
            StartCoroutine(ButtonAnim(battle3SwitchRect));
            StartCoroutine(GoToBattle3());
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }
    private IEnumerator GoToBattle3()
    {
        black.color = new(0, 0, 0, 0);
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeOut(0.5f, black));
        SceneManager.LoadScene("ExtraBattle3");
    }
    //バトル一覧を閉じる
    public void BattleClose()
    {
        StartCoroutine(ButtonAnim(backSwitchRect));
        StartCoroutine(Delay(battle, false));
        seSource.clip = seUIBack;
        seSource.Play();
    }
    public void Close()
    {
        words2.SetActive(false);
        bonus.SetActive(false);
        battle.SetActive(false);
        seSource.clip = seUIBack;
        seSource.Play();
    }
}
