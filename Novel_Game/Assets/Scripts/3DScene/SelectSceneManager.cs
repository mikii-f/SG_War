using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectSceneManager : SystemManagerOrigin
{
    [SerializeField] private RectTransform NormalSwitchRect;
    [SerializeField] private RectTransform HardSwitchRect;
    [SerializeField] private RectTransform StorySwitchRect;
    [SerializeField] private Text statusText;
    [SerializeField] private GameObject systemMessageObject;
    [SerializeField] private RectTransform yesSwitch;
    [SerializeField] private RectTransform noSwitch;
    [SerializeField] private GameObject developingMessage;
    [SerializeField] private Image black;
    [SerializeField] private TMP_Text countDown;
    private bool go = false;

    private void Start()
    {
        systemMessageObject.SetActive(false);
        developingMessage.SetActive(false);
        statusText.text = "�̗�\n" + GameManager.instance.SainHP.ToString() + "\n����SG\n" + GameManager.instance.SainSG.ToString() + "\n�U����\n" + GameManager.instance.SainAttack.ToString() + "\n�o���l\n" + GameManager.instance.EXP.ToString();
    }

    private void Update()
    {
        //���b�Z�[�W�ւ̉���
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
            StartCoroutine(ButtonAnim(NormalSwitchRect));
            StartCoroutine(Delay(systemMessageObject));
        }
    }
    //���̓m�[�}���X�e�[�W�݂̂ɑΉ�
    public void YesSwitch()
    {
        StartCoroutine(ButtonAnim(yesSwitch));
        StartCoroutine(Delay(systemMessageObject));
        StartCoroutine(StartGame());
        go = true;
    }
    //�Q�[���J�n
    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.1f);
        countDown.text = "3";
        yield return new WaitForSeconds(1);
        countDown.text = "2";
        yield return new WaitForSeconds(1);
        countDown.text = "1";
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("3DGameScene1");
    }
    public void NoSwitch()
    {
        StartCoroutine(Delay(systemMessageObject));
    }
    public void HardSwitch()
    {
        if (!go && !developingMessage.activeSelf)
        {
            StartCoroutine(ButtonAnim(HardSwitchRect));
            StartCoroutine(Developing());
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
            StartCoroutine(ButtonAnim(StorySwitchRect));
            StartCoroutine(GoToStory());
        }
    }
    private IEnumerator GoToStory()
    {
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeOut(1f, black));
        //�{���N����Ȃ��󋵂Ǝv���邪�b��[�u
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
