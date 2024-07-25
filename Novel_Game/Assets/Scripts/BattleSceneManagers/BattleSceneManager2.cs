using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSceneManager2 : BattleSceneManagerOrigin
{
    [SerializeField] private LowLevelEnemyManager lLEnemyManager1;
    [SerializeField] private ComplexEnemyManager compEnemyManager;
    [SerializeField] private LowLevelEnemyManager lLEnemyManager2;
    [SerializeField] private CarnivoreEnemyManager carnEnemyManager1;
    [SerializeField] private LowLevelEnemyManager lLEnemyManager3;
    [SerializeField] private CarnivoreEnemyManager carnEnemyManager2;

    // Start is called before the first frame update
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
        explanation.SetActive(true);
        yield return new WaitUntil(() => !explanation.activeSelf);
        battleStartAndFinishText.text = "3";
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "2";
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "1";
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "Battle Start";
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "";
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
