using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSceneManager : MonoBehaviour
{
    [SerializeField] private RectTransform NormalSwitchRect;
    [SerializeField] private RectTransform HardSwitchRect;
    [SerializeField] private RectTransform StorySwitchRect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NormalSwitch()
    {
        StartCoroutine(ButtonAnim(NormalSwitchRect));
        StartCoroutine(NormalStage());
    }
    private IEnumerator NormalStage()
    {
        yield return new WaitForSeconds(0.15f);
        SceneManager.LoadScene("3DGameScene1");
    }
    public void HardSwitch()
    {
        StartCoroutine(ButtonAnim(HardSwitchRect));
    }
    public void StorySwitch()
    {
        StartCoroutine(ButtonAnim(StorySwitchRect));
        StartCoroutine(GoToStory());
    }
    private IEnumerator GoToStory()
    {
        yield return new WaitForSeconds(0.15f);
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

    //ボタンのアニメーション
    private IEnumerator ButtonAnim(RectTransform rect)
    {
        Vector2 temp = rect.localScale;
        rect.localScale = new(0.9f * temp.x, 0.9f * temp.y);
        yield return new WaitForSeconds(0.1f);
        rect.localScale = temp;
    }
}
