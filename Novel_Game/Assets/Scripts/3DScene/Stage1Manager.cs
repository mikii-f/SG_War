using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Stage1Manager : StageManagerOrigin
{
    [SerializeField] TMP_Text goalText;
    [SerializeField] GameObject transparentWall;
    private MeshRenderer wallMeshRenderer;
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text scoreText;
    private int score = 0;
    [SerializeField] Slider expSlider;
    [SerializeField] GameObject messagePanel;
    [SerializeField] Text messageText;
    [SerializeField] GameObject function;
    [SerializeField] Text functionText2;
    [SerializeField] GameObject function2;
    [SerializeField] Text functionMessageText;
    private int functionNumber = 0;

    private void Start()
    {
        wallMeshRenderer = transparentWall.GetComponent<MeshRenderer>();
        wallMeshRenderer.enabled = false;
        transparentWall.SetActive(false);
        resultPanel.SetActive(false);
        messagePanel.SetActive(false);
        function.SetActive(false);
        function2.SetActive(false);
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
        }
        //���j���[�\����\��
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!function.activeSelf)
            {
                function.SetActive(true);
            }
            else if (!function2.activeSelf)
            {
                function.SetActive(false);
            }
        }
        //�e�@�\
        if (Input.GetKeyDown(KeyCode.Alpha1) && function.activeSelf && !function2.activeSelf)
        {
            function2.SetActive(true);
            functionMessageText.text = "�Ē��킵�܂����H";
            functionNumber = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && function.activeSelf && !function2.activeSelf)
        {
            if (GameManager.instance.EXP != 0)
            {
                function2.SetActive(true);
                functionMessageText.text = "�X�e�[�W�Z���N�g�ɖ߂�܂���";
                functionNumber = 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0) && function.activeSelf && !function2.activeSelf)
        {
            function2.SetActive(true);
            functionMessageText.text = "(�Q�[����]�����Ă�������������̋@�\�ł�)\n�X�L�b�v���ăX�R�A3000���l�����܂����H";
            functionNumber = 0;
        }
        //YesNo�I��
        if (function2.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
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
        goalText.text = "GOAL";
        score = Mathf.Max((int)(2000 * medalCount / 100 * 60 / time), 1);
        yield return new WaitForSeconds(4);
        goalText.text = "";
        scoreText.text = "���_���l���F" + medalCount.ToString() + "��\n\n�^�C���F" + time.ToString("F2") + "s\n\n\n�X�R�A�F" + score.ToString() + "\n�݌vEXP�F" + (GameManager.instance.EXP + score).ToString();
        StartCoroutine(GoNext());
    }
    private void Skip()
    {
        playerManager.Clear = true;
        clear = true;
        score = 3000;
        function.SetActive(false);
        function2.SetActive(false);
        scoreText.text = "���_���l���F ��\n\n�^�C���F s\n\n\n�X�R�A�F" + score.ToString() + "\n�݌vEXP�F" + (GameManager.instance.EXP + score).ToString();
        StartCoroutine(GoNext());
    }
    private IEnumerator GoNext()
    {
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
        else if (GameManager.instance.EXP < 5000)
        {
            GameManager.instance.EXP += score;
            if (GameManager.instance.EXP >= 5000)
            {
                GameManager.instance.SainHP = 1400;
                GameManager.instance.SainAttack = 90;
                GameManager.instance.SainSG = 30;
                GameManager.instance.Save();
                messageText.text = "�̗́F1000��1400\n�U���́F50��90\n����SG�F20��30";
                messagePanel.SetActive(true);
                yield return new WaitUntil(() => Input.GetMouseButton(0));
                messagePanel.SetActive(false);
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
                GameManager.instance.SainHP = 1700;
                GameManager.instance.SainAttack = 120;
                GameManager.instance.Save();
                messageText.text = "�̗́F1400��1700\n�U���́F90��120";
                messagePanel.SetActive(true);
                yield return new WaitUntil(() => Input.GetMouseButton(0));
                messagePanel.SetActive(false);
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
                GameManager.instance.SainHP = 2000;
                GameManager.instance.SainAttack = 150;
                GameManager.instance.SainSG = 40;
                GameManager.instance.Save();
                messageText.text = "�̗́F1700��2000\n�U���́F120��150\n����SG�F30��40";
                messagePanel.SetActive(true);
                yield return new WaitUntil(() => Input.GetMouseButton(0));
                messagePanel.SetActive(false);
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
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        SceneManager.LoadScene("3DGameSelectScene");
    }
}
