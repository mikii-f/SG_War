using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExtraBattle2Manager : BattleSceneManagerOrigin
{
    [SerializeField] private LowLevelEnemyManager lLEnemyManager1;
    [SerializeField] private CarnivoreEnemyManager carnEnemyManager1;
    [SerializeField] private CarnivoreEnemyManager carnEnemyManager2;
    [SerializeField] private LowLevelEnemyManager lLEnemyManager2;
    [SerializeField] private ComplexEnemyManager compEnemyManager1;
    [SerializeField] private LowLevelEnemyManager lLEnemyManager3;
    [SerializeField] private CarnivoreEnemyManager carnEnemyManager3;
    [SerializeField] private CarnivoreEnemyManager carnEnemyManager4;
    [SerializeField] private LowLevelEnemyManager lLEnemyManager4;
    [SerializeField] private LowLevelEnemyManager lLEnemyManager5;
    [SerializeField] private ComplexEnemyManager compEnemyManager2;
    [SerializeField] private CarnivoreEnemyManager carnEnemyManager5;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Text tutorialText;
    [SerializeField] private AudioClip bgmBattle;
    [SerializeField] private Text remainingTimeText;
    private float remainingTime = 300;
    protected override void StartSet()
    {
        numberOfEnemy = new int[] { 1, 2, 3, 3, 3 };
        numberOfWave = 5;
        enemyComposition = new EnemyManagerOrigin[5][];
        enemyComposition[0] = new EnemyManagerOrigin[1] { lLEnemyManager1 };
        enemyComposition[1] = new EnemyManagerOrigin[2] { carnEnemyManager1, carnEnemyManager2 };
        enemyComposition[2] = new EnemyManagerOrigin[3] { lLEnemyManager2, compEnemyManager1, lLEnemyManager3 };
        enemyComposition[3] = new EnemyManagerOrigin[3] { carnEnemyManager3, carnEnemyManager4, lLEnemyManager4 };
        enemyComposition[4] = new EnemyManagerOrigin[3] { lLEnemyManager5, compEnemyManager2, carnEnemyManager5 };
        deadEnemyComposition = new bool[5][];
        deadEnemyComposition[0] = new bool[1];
        deadEnemyComposition[1] = new bool[2];
        deadEnemyComposition[2] = new bool[3];
        deadEnemyComposition[3] = new bool[3];
        deadEnemyComposition[4] = new bool[3];
        numberOfArriveEnemy = numberOfEnemy[0];
        tutorialPanel.SetActive(false);
        StartCoroutine(BattleStart());
    }

    private IEnumerator BattleStart()
    {
        yield return null;
        for (int i=1; i < 5; i++)
        {
            enemyComposition[i][0].AllObject = false;
            for (int j=1; j < numberOfEnemy[i]; j++)
            {
                enemyComposition[i][j].DisSelect();
                enemyComposition[i][j].AllObject = false;
            }
        }
        yield return new WaitForSeconds(2);
        tutorialPanel.SetActive(true);
        tutorialText.text = "かつてエルを失った戦いを繰り返さないよう、大量の敵を速やかに殲滅する訓練をしましょう。";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialText.text = "本戦闘には時間制限があります。5分以内に全ての敵を倒しきれなかった場合ゲームオーバーとなります。";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialText.text = "スキルを使用するタイミングや、敵を倒す順番を意識して挑みましょう。";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialPanel.SetActive(false);
        explanation.SetActive(true);
        yield return new WaitUntil(() => !explanation.activeSelf);
        StartCoroutine(VolumeFadeOut(2, audioSource));
        battleStartAndFinishText.text = "3";
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
        enemyComposition[0][0].Pause = false;
        StartCoroutine(CountDown());
    }
    private IEnumerator CountDown()
    {
        while(remainingTime > 0)
        {
            yield return null;
            if (!sainManager.Pause)
            {
                remainingTime = Mathf.Max(remainingTime - Time.deltaTime, 0);
                remainingTimeText.text = "残り時間:" + remainingTime.ToString("F2");
            }
        }
        StartCoroutine(GameOver());
    }
    public override void SceneLoad()
    {
        SceneManager.LoadScene("AfterClear");
    }
}
