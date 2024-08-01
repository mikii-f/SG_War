using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSceneManager1 : BattleSceneManagerOrigin
{
    [SerializeField] private LowLevelEnemyManager lLEnemyManager;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Text tutorialText;
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
        tutorialPanel.SetActive(false);
        StartCoroutine(BattleStart());
    }
    private IEnumerator BattleStart()
    {
        yield return new WaitForSeconds(2);
        tutorialPanel.SetActive(true);
        tutorialText.text = "長期間戦闘行動を行っていなかったため、あなた方の能力は低下しているようです……。";
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
        lLEnemyManager.Pause = false;
    }

    public override void SceneLoad()
    {
        GameManager.instance.SceneName = "MainScene2_2";
        GameManager.instance.LineNumber = 0;
        GameManager.instance.Save();
        SceneManager.LoadScene("MainScene2_2");
    }
}