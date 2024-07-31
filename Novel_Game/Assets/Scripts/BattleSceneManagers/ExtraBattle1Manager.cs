using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExtraBattle1Manager : BattleSceneManagerOrigin
{
    [SerializeField] private ElManager elManager;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Text tutorialText;

    // Start is called before the first frame update
    protected override void StartSet()
    {
        numberOfEnemy = new int[] { 1 };
        numberOfWave = 1;
        enemyComposition = new EnemyManagerOrigin[1][];
        enemyComposition[0] = new EnemyManagerOrigin[1] { elManager };
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
        tutorialText.text = "���z��ԂȂ獡�̃G�����S�͂Ő키���Ƃ��ł��܂��B�t�B�A�Ƌ��ɖ{�C�œ��P�ɗՂ݂܂��傤�B";
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
        elManager.Pause = false;
    }

    public override void SceneLoad()
    {
        SceneManager.LoadScene("AfterClear");
    }
}