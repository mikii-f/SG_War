using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSceneManager3 : BattleSceneManagerOrigin
{
    [SerializeField] private LowLevelEnemyManager lLEnemyManager;
    [SerializeField]private ComplexEnemyManager compEnemyManager;
    [SerializeField] private CarnivoreEnemyManager carnEnemyManager;
    [SerializeField] private ElManager elManager;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Text tutorialText;
    [SerializeField] private AudioClip bgmEl;

    protected override void StartSet()
    {
        numberOfEnemy = new int[] { 3, 1 };
        numberOfWave = 2;
        enemyComposition = new EnemyManagerOrigin[2][];
        enemyComposition[0] = new EnemyManagerOrigin[3] { lLEnemyManager, compEnemyManager, carnEnemyManager };
        enemyComposition[1] = new EnemyManagerOrigin[1] {  elManager };
        deadEnemyComposition = new bool[2][];
        deadEnemyComposition[0] = new bool[3];
        deadEnemyComposition[1] = new bool[1];
        numberOfArriveEnemy = numberOfEnemy[0];
        tutorialPanel.SetActive(false);
        StartCoroutine(BattleStart());
        StartCoroutine(BGMChange());
    }
    private IEnumerator BattleStart()
    {
        yield return null;
        enemyComposition[0][1].DisSelect();
        enemyComposition[0][2].DisSelect();
        for (int i = 0; i < numberOfEnemy[1]; i++)
        {
            enemyComposition[1][i].AllObject = false;
        }
        yield return new WaitForSeconds(2);
        tutorialPanel.SetActive(true);
        tutorialText.text = "�Z�C��XI-�G���͋��G�ł��B";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialText.text = "���̃G�l�~�[�Ƃ͈قȂ�A�ʏ�U���ƃ`���[�W�Z�ȊO�̃X�L�����������Ă��܂��B";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialText.text = "�܂��A�`���[�W���ő�ɂȂ�ƕK�E�Z�����̑̐��ɓ��邱�Ƃ�����܂��B";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialText.text = "���̏�Ԃł̓V�[���h���W�J����A�����̒ʏ�U���̓_���[�W��^�����Ȃ��Ȃ�܂��B";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialText.text = "�����̕K�E�Z�𔭓��ł���΁A�V�[���h������ƂƂ��ɕK�E�Z�̔�����j�~�ł��܂��B";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialText.text = "�K�E�Z�͉���E�K�[�h�����s�\�Ȃ��߁A�G���̃`���[�W�Q�[�W�ɂ͓��ɒ��ӂ��ėՂ݂܂��傤�B";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialPanel.SetActive(false);
        explanation.SetActive(true);
        yield return new WaitUntil(() => !explanation.activeSelf);
        battleStartAndFinishText.text = "3";
        seSource.clip = seUIUnactive;
        seSource.Play();
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "2";
        seSource.clip = seCountDown;
        seSource.Play();
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "1";
        seSource.clip = seCountDown;
        seSource.Play();
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "Battle Start";
        seSource.clip = seWhistle;
        seSource.Play();
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "";
        sainManager.Pause = false;
        leaderManager.Pause = false;
        for (int i = 0; i < 3; i++)
        {
            enemyComposition[0][i].Pause = false;
        }
    }

    private IEnumerator BGMChange()
    {
        yield return new WaitUntil(() => numberOfCurrentWave == 1);
        audioSource.clip = bgmEl;
        audioSource.Play();
    }

    public override void SceneLoad()
    {
        GameManager.instance.SceneName = "MainScene4_2";
        GameManager.instance.LineNumber = 0;
        GameManager.instance.Save();
        SceneManager.LoadScene("MainScene4_2");
    }
}
