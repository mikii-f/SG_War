using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSceneManager1 : BattleSceneManagerOrigin
{
    [SerializeField] private GameObject lLEnemyManagerObject;
    private LowLevelEnemyManager lLEnemyManager;
    // Start is called before the first frame update
    protected override void StartSet()
    {
        lLEnemyManager = lLEnemyManagerObject.GetComponent<LowLevelEnemyManager>();
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
        yield return new WaitForSeconds(3);
        sainManager.Pause = false;
        leaderManager.Pause = false;
        lLEnemyManager.Pause = false;
    }

    public override void SceneLoad()
    { 
        SceneManager.LoadScene("MainScene2_2");
    }
}