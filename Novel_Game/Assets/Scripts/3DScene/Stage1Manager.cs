using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Stage1Manager : StageManagerOrigin
{
    [SerializeField] TMP_Text goalText;
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text scoreText;
    private int score = 0;
    [SerializeField] Slider expSlider;
    [SerializeField] GameObject messagePanel;
    [SerializeField] Text messageText;

    private void Start()
    {
        resultPanel.SetActive(false);
        messagePanel.SetActive(false);
    }
    private void Update()
    {
        if (!clear)
        {
            time += Time.deltaTime;
            timeText.text = "Time: " + time.ToString("F2");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Goal());
        }
    }
    private IEnumerator Goal()
    {
        playerManager.Clear = true;
        clear = true;
        goalText.text = "GOAL";
        score = (int)(1000 * medalCount / 100 * 60 / time);
        yield return new WaitForSeconds(4);
        goalText.text = "";
        scoreText.text = "���_���l���F" + medalCount.ToString() + "��\n�^�C���F" + time.ToString("F2") + "s\n\n�X�R�A�F" + score.ToString() + "\n�݌vEXP�F" + (GameManager.instance.EXP + score).ToString();
        resultPanel.SetActive(true);
        expSlider.value = (float)(GameManager.instance.EXP + score) / 15000;
        //���x���A�b�v���̏���(���ɁA���񎞂̂݃X�g�[���[�̑J�ڐ��؂�ւ���)
        yield return new WaitForSeconds(2);
        if (GameManager.instance.EXP == 0)
        {
            GameManager.instance.EXP += score;
            GameManager.instance.LineNumber = 0;
            GameManager.instance.SceneName = "MainScene2_3";
            GameManager.instance.Save();
            messageText.text = "�X�L��2�E3���J������܂���\n�K�E�Z���J������܂���\n���[�_�[�X�L���u�̗͎x���v�u�U���x���v�u���x�x���v���J������܂���";
            messagePanel.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButton(0));
            messagePanel.SetActive(false);
            yield return new WaitForSeconds(1);
        }
        //exp5000
        //exp10000
        //exp15000
        else
        {
            GameManager.instance.EXP += score;
            GameManager.instance.Save();
        }
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        SceneManager.LoadScene("3DGameSelectScene");
    }
}
