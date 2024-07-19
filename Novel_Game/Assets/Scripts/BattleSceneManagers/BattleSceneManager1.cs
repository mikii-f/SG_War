using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSceneManager1 : BattleSceneManagerOrigin
{
    [SerializeField] private LowLevelEnemyManager lLEnemyManager;
    // Start is called before the first frame update
    protected override void StartSet()
    {
        numberOfEnemy = new int[]{1};
        numberOfWave = 1;
        enemyComposition = new EnemyManagerOrigin[1][];
        enemyComposition[0] = new EnemyManagerOrigin[] { lLEnemyManager };
        deadEnemyComposition = new bool[1][];
        deadEnemyComposition[0] = new bool[1];
        numberOfArriveEnemy = numberOfEnemy[0];
        StartCoroutine(BattleStart());
    }
    private IEnumerator BattleStart()
    {
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
        lLEnemyManager.Pause = false;
    }

    public override void SceneLoad()
    { 
        GameManager.instance.LineNumber = 0;
        SceneManager.LoadScene("MainScene2_2");
    }
}