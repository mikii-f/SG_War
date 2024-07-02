using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleSceneManager1 : MonoBehaviour
{
    [SerializeField] private GameObject sainManagerObject;
    [SerializeField] private GameObject leaderManagerObject;
    [SerializeField] private GameObject lLEnemyManagerObject;
    private SainManager sainManager;
    private LeaderManager leaderManager;
    private LowLevelEnemyManager lLEnemyManager;
    [SerializeField] GameObject enemyNumberObject;
    private Text enemyNumberText;
    private const int numberOfEnemy = 1;
    private int numberOfArriveEnemy = 1;
    // Start is called before the first frame update
    void Start()
    {
        sainManager = sainManagerObject.GetComponent<SainManager>();
        leaderManager = leaderManagerObject.GetComponent<LeaderManager>();
        lLEnemyManager = lLEnemyManagerObject.GetComponent<LowLevelEnemyManager>();
        enemyNumberText = enemyNumberObject.GetComponent<Text>();
    }


    //敵への攻撃(敵が複数体いるシーンではターゲット管理用の変数を用意)
    public void SainToEnemyAttack(int damage)
    {
        lLEnemyManager.ReceiveDamage(damage);
    }
    //敵からの攻撃
    public void EnemyToSainAttack(int damage)
    {
        sainManager.ReceiveDamage(damage);
    }

    //敵が死んだことの受け取り
    public void EnemyDied()
    {
        numberOfArriveEnemy--;
        enemyNumberText.text = "Enemy " + numberOfArriveEnemy.ToString() + "/" + numberOfEnemy.ToString();
        if (numberOfArriveEnemy == 0)
        {
            StartCoroutine(Win());
        }
    }

    //ガードの展開
    public void Guard()
    {
        StartCoroutine(sainManager.ReceiveGuard());
    }
    //各種アシスト
    public void Assist(int n)
    {
        switch (n)
        {
            case 0:
                sainManager.ReceiveHPAssist();
                break;
            case 1:
                StartCoroutine(sainManager.ReceiveAttackAssist());
                break;
            case 2:
                StartCoroutine(sainManager.ReceiveSpeedAssist());
                break;
            default:
                break;
        }
    }


    //バトル勝利
    private IEnumerator Win()
    {
        sainManager.Pause = true;
        leaderManager.Pause = true;
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainScene2_2");
    }
}
