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
    [SerializeField] private GameObject systemMessageObject;
    [SerializeField] private Text systemMessage;
    [SerializeField] private RectTransform yesSwitch;
    [SerializeField] private RectTransform noSwitch;
    [SerializeField] private Image black;
    private int messageNumber;

    // Start is called before the first frame update
    void Start()
    {
        systemMessageObject.SetActive(false);
    }

    // Update is called once per frame
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
        StartCoroutine(Delay(systemMessageObject));
    }
    public void WordsSwitch()
    {
        StartCoroutine(ButtonAnim(wordsSwitchRect));
    }
    public void GrowSwitchRect()
    {
        StartCoroutine(ButtonAnim(growSwitchRect));
        systemMessage.text = "育成に向かいますか？";
        messageNumber = 1;
        StartCoroutine(Delay(systemMessageObject));
    }
    public void BonusSwitch()
    {
        StartCoroutine(ButtonAnim(bonusSwitchRect));
    }
    public void PlusAlphaSwitch()
    {
        StartCoroutine(ButtonAnim(plusAlphaSwitch));
    }
    public void YesSwitch()
    {
        StartCoroutine(ButtonAnim(yesSwitch));
        switch (messageNumber)
        {
            case 0:
                StartCoroutine(GoBackTitle());
                break;
            case 1:
                StartCoroutine(GoToGrow());
                break;
        }
    }
    public void NoSwitch()
    {
        StartCoroutine(ButtonAnim(noSwitch));
        StartCoroutine(Delay(systemMessageObject));
    }
    //タイトルへ
    private IEnumerator GoBackTitle()
    {
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeOut(1, black));
        SceneManager.LoadScene("TitleScene");
    }
    //育成へ
    private IEnumerator GoToGrow()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("3DGameSelectScene");
    }
}
