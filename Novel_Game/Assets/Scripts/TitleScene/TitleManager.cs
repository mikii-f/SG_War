using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private RectTransform newGameSwitchRect;
    [SerializeField] private RectTransform continueGameSwitchRect;
    [SerializeField] private RectTransform wordsSwitchRect;
    [SerializeField] private RectTransform methodSwitchRect;
    [SerializeField] private RectTransform afterClearSwitch;
    [SerializeField] private GameObject words1;
    [SerializeField] private GameObject method;

    // Start is called before the first frame update
    void Start()
    {
        words1.SetActive(false);
        method.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGameSwitch()
    {
        StartCoroutine(ButtonAnim(newGameSwitchRect));
        StartCoroutine(NewGame());
    }
    private IEnumerator NewGame()
    {
        yield return new WaitForSeconds(0.15f);
        GameManager.instance.Initialize();
        SceneManager.LoadScene(GameManager.instance.SceneName);
    }
    public void ContinueGameSwitch()
    {
        StartCoroutine(ButtonAnim(continueGameSwitchRect));
    }
    public void WordsSwitch()
    {
        StartCoroutine(ButtonAnim(wordsSwitchRect));
        words1.SetActive(true);
    }
    public void MethodSwitch()
    {
        StartCoroutine(ButtonAnim(methodSwitchRect));
        method.SetActive(true);
    }
    public void AfterClearSwitch() 
    {
        StartCoroutine(ButtonAnim(afterClearSwitch));
    }
    public void Close()
    {
        words1.SetActive(false);
        method.SetActive(false);
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
