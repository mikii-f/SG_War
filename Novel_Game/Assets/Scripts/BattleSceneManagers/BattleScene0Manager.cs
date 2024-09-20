using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSceneManager0 : BattleSceneManagerOrigin
{
    [SerializeField] private CommandManager commandManager;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Text tutorialText;
    [SerializeField] private GameObject enemyFrame;
    [SerializeField] private GameObject sainFrame1;
    [SerializeField] private GameObject sainFrame2;
    [SerializeField] private GameObject leaderFrame;
    [SerializeField] private GameObject systemFrame;
    // Start is called before the first frame update
    protected override void StartSet()
    {
        numberOfEnemy = new int[] { 1 };
        numberOfWave = 1;
        enemyComposition = new EnemyManagerOrigin[1][];
        enemyComposition[0] = new EnemyManagerOrigin[] { commandManager };
        deadEnemyComposition = new bool[1][];
        deadEnemyComposition[0] = new bool[1];
        numberOfArriveEnemy = numberOfEnemy[0];
        tutorialPanel.SetActive(false);
        enemyFrame.SetActive(false);
        sainFrame1.SetActive(false);
        sainFrame2.SetActive(false);
        leaderFrame.SetActive(false);
        systemFrame.SetActive(false);
        StartCoroutine(BattleStart());
    }
    private IEnumerator BattleStart()
    {
        yield return new WaitForSeconds(2);
        tutorialPanel.SetActive(true);
        tutorialText.text = "�`���[�g���A��";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "���ꂩ�烊�A���^�C�����̃o�g�����s���܂��B";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "�G�Ɩ����ɂ͂��ꂼ��U���܂ł̃C���^�[�o��������܂��B";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "�����̓J�E���g��0�ɂȂ�Ɛ퓬�X�L�����g�p�ł��܂��B�I�[�g�g�p�ɂ��邱�Ƃ��\�ł��B";
        sainFrame1.SetActive(true);
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "�J�E���g��0�̏�Ԃőҋ@���Ă���Ƃ��͓G����̃_���[�W��������50%�J�b�g���܂��B";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "�퓬�X�L���ɂ�SG(�X�L���Q�[�W)���񕜂�����̂Ə������̂�����܂��B";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        sainFrame1.SetActive(false);
        sainFrame2.SetActive(true);
        tutorialText.text = "SG��100�ɒB����ƕK�E�Z���g�p�ł��܂��B";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        sainFrame2.SetActive(false);
        enemyFrame.SetActive(true);
        tutorialText.text = "�G�͂��ꂼ��`���[�W�Q�[�W�������Ă��܂��B";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "�ʏ�U���̂��тɃQ�[�W���������A�ő�ɂȂ�Ƌ��͂ȍU�����s���܂��B";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        enemyFrame.SetActive(false);
        leaderFrame.SetActive(true);
        tutorialText.text = "�G�̍U���͒��e�܂Ŗ�1�b����܂��B���Ȃ��̓^�C�~���O�ǂ��K�[�h���邱�ƂŃ_���[�W���J�b�g�ł��܂��B";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "�܂��A���Ȃ��͖����ɑ̗́A�U���A���x�̎x�����s�����Ƃ��ł��܂��B";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        leaderFrame.SetActive(false);
        systemFrame.SetActive(true);
        tutorialText.text = "�S�ẴE�F�[�u�őS�Ă̓G��|���Ə����A������HP��0�ɂȂ�Ɣs�k�ł��B";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        systemFrame.SetActive(false);
        tutorialText.text = "���̉�ʂœG�����̔\�͂̏ڍׂ��m�F�ł��܂��B\n�ł͐퓬���J�n���܂��傤�B";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        yield return null;
        tutorialPanel.SetActive(false);
        explanation.SetActive(true);
        seSource.clip = seUIClick;
        seSource.Play();
        yield return new WaitUntil(() => !explanation.activeSelf);
        battleStartAndFinishText.text = "3";
        seSource.clip = seUIUnactive;
        seSource.Play();
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "2";
        seSource.clip = seUIUnactive;
        seSource.Play();
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "1";
        seSource.clip = seUIUnactive;
        seSource.Play();
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "Battle Start";
        seSource.clip = seChange;
        seSource.Play();
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "";
        sainManager.Pause = false;
        leaderManager.Pause = false;
        commandManager.Pause = false;
    }

    public override void SceneLoad()
    {
        GameManager.instance.SceneName = "MainScene1";
        GameManager.instance.LineNumber = 0;
        GameManager.instance.SainHP = 1000;
        GameManager.instance.SainAttack = 50;
        GameManager.instance.SainSG = 20;
        GameManager.instance.Save();
        SceneManager.LoadScene("MainScene1");
    }
}