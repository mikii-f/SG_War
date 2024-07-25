using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSceneManager0 : BattleSceneManagerOrigin
{
    [SerializeField] private LowLevelEnemyManager lLEnemyManager;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Text tutorialText;
    [SerializeField] private GameObject enemyFrame;
    [SerializeField] private GameObject sainFrame1;
    [SerializeField] private GameObject sainFrame2;
    [SerializeField] private GameObject leaderFrame;
    [SerializeField] private GameObject systemFrame;
    // Start is called before the first frame update
    protected override void StartSet()
    {
        numberOfEnemy = new int[] { 1 };
        numberOfWave = 1;
        enemyComposition = new EnemyManagerOrigin[1][];
        enemyComposition[0] = new EnemyManagerOrigin[] { lLEnemyManager };
        deadEnemyComposition = new bool[1][];
        deadEnemyComposition[0] = new bool[1];
        numberOfArriveEnemy = numberOfEnemy[0];
        tutorialPanel.SetActive(false);
        enemyFrame.SetActive(false);
        sainFrame1.SetActive(false);
        sainFrame2.SetActive(false);
        leaderFrame.SetActive(false);
        systemFrame.SetActive(false);
        StartCoroutine(BattleStart());
    }
    private IEnumerator BattleStart()
    {
        yield return new WaitForSeconds(2);
        tutorialPanel.SetActive(true);
        tutorialText.text = "チュートリアル";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "これからリアルタイム制のバトルが行われます。";
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "敵と味方にはそれぞれ攻撃までのインターバルがあります。";
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "味方はカウントが0になると戦闘スキルが使用できます。オート使用にすることも可能です。";
        sainFrame1.SetActive(true);
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "戦闘スキルにはSG(スキルゲージ)を回復するものと消費するものがあります。";
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        sainFrame1.SetActive(false);
        sainFrame2.SetActive(true);
        tutorialText.text = "SGが100に達すると必殺技を使用できます。";
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        sainFrame2.SetActive(false);
        enemyFrame.SetActive(true);
        tutorialText.text = "敵はそれぞれチャージゲージを持っています。";
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "通常攻撃のたびにゲージが増加し、最大になると強力な攻撃を行います。";
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        enemyFrame.SetActive(false);
        leaderFrame.SetActive(true);
        tutorialText.text = "敵の攻撃は着弾まで約1秒あります。あなたはタイミング良くガードすることでダメージをカットできます。";
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "また、あなたは味方に体力、攻撃、速度の支援を行うことができます。";
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        leaderFrame.SetActive(false);
        tutorialText.text = "今回敵は1体ですが、敵が複数体存在するときはA/Dキーで攻撃対象を選択することができます。";
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        systemFrame.SetActive(true);
        tutorialText.text = "全てのウェーブで全ての敵を倒すと勝利、味方のHPが0になると敗北です。";
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        systemFrame.SetActive(false);
        tutorialText.text = "次の画面で敵味方の能力の詳細を確認できます。\nでは戦闘を開始しましょう。";
        yield return null;
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
        GameManager.instance.SceneName = "MainScene1";
        GameManager.instance.LineNumber = 0;
        GameManager.instance.SainHP = 1000;
        GameManager.instance.SainAttack = 50;
        GameManager.instance.SainSG = 20;
        GameManager.instance.Save();
        SceneManager.LoadScene("MainScene1");
    }
}