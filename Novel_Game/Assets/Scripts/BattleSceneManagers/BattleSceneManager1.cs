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

    //�o�g������
    protected override IEnumerator Win()
    {
        //�K�E�Z�ɂ���ēG��S���|�����ꍇ��Pause��������������̂�����邽��(���̃V�[���ł͕s�v�ƂȂ�\��)
        yield return new WaitUntil(() => !isSpecialAttack);
        sainManager.Pause = true;
        leaderManager.Pause = true;
        yield return new WaitForSeconds(3);
        yield return StartCoroutine(FadeOut(2, blackImage));
        SceneManager.LoadScene("MainScene2_2");
    }
}