using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Stage0Manager : StageManagerOrigin
{
    [SerializeField] private GameObject method;
    [SerializeField] private RectTransform nextSwitchRect;
    [SerializeField] private TMP_Text countDown;
    [SerializeField] private GameObject enemy;
    private bool go = false;
    [SerializeField] private AudioClip seCountDown;
    [SerializeField] private GameObject tutorialVideo;
    [SerializeField] private RectTransform videoSwitchRect;
    [SerializeField] private VideoPlayer videoPlayer;

    private void Start()
    {
        seSource.volume = GameManager.instance.SeVolume;
        tutorialVideo.SetActive(false);
    }

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

    public void VideoSwitch()
    {
        if (!go && !tutorialVideo.activeSelf)
        {
            StartCoroutine(ButtonAnim(videoSwitchRect));
            tutorialVideo.SetActive(true);
            method.SetActive(false);
            videoPlayer.time = 0;
            videoPlayer.Play();
            StartCoroutine(CloseVideo());
        }
    }
    private IEnumerator CloseVideo()
    {
        yield return new WaitForSeconds((float)videoPlayer.clip.length);
        tutorialVideo.SetActive(false);
    }
    
    public void Next()
    {
        if (!go)
        {
            go = true;
            method.SetActive(false);
            tutorialVideo.SetActive(false);
            if (enemy != null)
            {
                enemy.SetActive(false);
            }
            StartCoroutine(ButtonAnim(nextSwitchRect));
            StartCoroutine(StartGame());
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }
    //ÉQÅ[ÉÄäJén
    private IEnumerator StartGame()
    {
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
        SceneManager.LoadScene("3DGameScene1");
    }
}
