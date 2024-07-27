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

    // Start is called before the first frame update
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
        tutorialText.text = "セインXI-エルは強敵です。";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        yield return null;
        tutorialText.text = "他のエネミーとは異なり、通常攻撃とチャージ技以外のスキルも所持しています。";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        yield return null;
        tutorialText.text = "また、チャージが最大になると必殺技準備の体勢に入ることがあります。";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        yield return null;
        tutorialText.text = "この状態ではシールドが展開され、味方の通常攻撃はダメージを与えられなくなります。";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        yield return null;
        tutorialText.text = "味方の必殺技を発動できれば、シールドを割るとともに必殺技の発動を阻止できます。";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        yield return null;
        tutorialText.text = "必殺技は回避・ガード等が不可能なため、エルのチャージゲージには特に注意して臨みましょう。";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        yield return null;
        tutorialPanel.SetActive(false);
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
        GameManager.instance.SceneName = "MainScene4_2";
        GameManager.instance.LineNumber = 0;
        GameManager.instance.Save();
        SceneManager.LoadScene("MainScene4_2");
    }
}
