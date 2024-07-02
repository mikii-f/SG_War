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


    //�G�ւ̍U��(�G�������̂���V�[���ł̓^�[�Q�b�g�Ǘ��p�̕ϐ���p��)
    public void SainToEnemyAttack(int damage)
    {
        lLEnemyManager.ReceiveDamage(damage);
    }
    //�G����̍U��
    public void EnemyToSainAttack(int damage)
    {
        sainManager.ReceiveDamage(damage);
    }

    //�G�����񂾂��Ƃ̎󂯎��
    public void EnemyDied()
    {
        numberOfArriveEnemy--;
        enemyNumberText.text = "Enemy " + numberOfArriveEnemy.ToString() + "/" + numberOfEnemy.ToString();
        if (numberOfArriveEnemy == 0)
        {
            StartCoroutine(Win());
        }
    }

    //�K�[�h�̓W�J
    public void Guard()
    {
        StartCoroutine(sainManager.ReceiveGuard());
    }
    //�e��A�V�X�g
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


    //�o�g������
    private IEnumerator Win()
    {
        sainManager.Pause = true;
        leaderManager.Pause = true;
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("MainScene2_2");
    }
}
