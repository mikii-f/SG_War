using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSceneManager2 : BattleSceneManagerOrigin
{
    [SerializeField] private LowLevelEnemyManager lLEnemyManager1;
    [SerializeField] private ComplexEnemyManager compEnemyManager;
    [SerializeField] private LowLevelEnemyManager lLEnemyManager2;
    [SerializeField] private CarnivoreEnemyManager carnEnemyManager1;
    [SerializeField] private LowLevelEnemyManager lLEnemyManager3;
    [SerializeField] private CarnivoreEnemyManager carnEnemyManager2;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Text tutorialText;
    [SerializeField] private AudioClip bgmBattle;

    protected override void StartSet()
    {
        numberOfEnemy = new int[] {3, 3};
        numberOfWave = 2;
        enemyComposition = new EnemyManagerOrigin[2][];
        enemyComposition[0] = new EnemyManagerOrigin[3] { lLEnemyManager1, compEnemyManager, lLEnemyManager2 };
        enemyComposition[1] = new EnemyManagerOrigin[3] { carnEnemyManager1, lLEnemyManager3, carnEnemyManager2 };
        deadEnemyComposition = new bool[2][];
        deadEnemyComposition[0] = new bool[3];
        deadEnemyComposition[1] = new bool[3];
        numberOfArriveEnemy = numberOfEnemy[0];
        tutorialPanel.SetActive(false);
        StartCoroutine(BattleStart());
    }
    private IEnumerator BattleStart()
    {
        yield return null;
        enemyComposition[0][1].DisSelect();
        enemyComposition[0][2].DisSelect();
        enemyComposition[1][1].DisSelect();
        enemyComposition[1][2].DisSelect();
        for (int i = 0; i < 3; i++)
        {
            enemyComposition[1][i].AllObject = false;
        }
        yield return new WaitForSeconds(2);
        tutorialPanel.SetActive(true);
        tutorialText.text = "����͕����̂̓G�Ƃ̐퓬�ƂȂ�܂��B";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialText.text = "�G�̏ڍׂ��m�F���A�D�悵�ē|���ׂ��G�����ɂ߂܂��傤�B";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialText.text = "A/D�L�[�ɂ��G�^�[�Q�b�g��؂�ւ��邱�Ƃ��ł��܂��B";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialText.text = "�܂��A�S�Ă̍U�����K�[�h����͍̂���ł��B���͂ȍU�����d�_�I�ɃK�[�h����悤�ɂ��܂��傤�B";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialText.text = "�퓬�X�L��3���g�p���ĉ�𗦂��グ��A�K�E�Z�ň�|����Ƃ������헪���L���ł��B";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialPanel.SetActive(false);
        explanation.SetActive(true);
        yield return new WaitUntil(() => !explanation.activeSelf);
        StartCoroutine(VolumeFadeOut(2, audioSource));
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
        audioSource.clip = bgmBattle;
        audioSource.volume = GameManager.instance.BgmVolume;
        audioSource.Play();
        sainManager.Pause = false;
        leaderManager.Pause = false;
        for (int i = 0; i < 3; i++)
        {
            enemyComposition[0][i].Pause = false;
        }
    }

    public override void SceneLoad()
    {
        GameManager.instance.SceneName = "MainScene3_2";
        GameManager.instance.LineNumber = 0;
        GameManager.instance.Save();
        SceneManager.LoadScene("MainScene3_2");
    }
}
