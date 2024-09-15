using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Stage0Manager : StageManagerOrigin
{
    [SerializeField] private GameObject method;
    [SerializeField] private RectTransform nextSwitchRect;
    [SerializeField] private TMP_Text countDown;
    [SerializeField] private GameObject enemy;
    private bool go = false;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !go)
        {
            method.SetActive(!method.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Next();
        }
    }
    
    public void Next()
    {
        if (!go)
        {
            go = true;
            method.SetActive(false);
            if (enemy != null)
            {
                enemy.SetActive(false);
            }
            StartCoroutine(ButtonAnim(nextSwitchRect));
            StartCoroutine(StartGame());
        }
    }
    //ÉQÅ[ÉÄäJén
    private IEnumerator StartGame()
    {
        countDown.text = "3";
        yield return new WaitForSeconds(1);
        countDown.text = "2";
        yield return new WaitForSeconds(1);
        countDown.text = "1";
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("3DGameScene1");
    }
}
