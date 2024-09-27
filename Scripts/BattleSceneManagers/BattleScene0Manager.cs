using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSceneManager0 : BattleSceneManagerOrigin
{
    [SerializeField] private CommandManager commandManager;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Text tutorialText;
    [SerializeField] private GameObject enemyFrame;
    [SerializeField] private GameObject sainFrame1;
    [SerializeField] private GameObject sainFrame2;
    [SerializeField] private GameObject leaderFrame;
    [SerializeField] private GameObject systemFrame;
    [SerializeField] private GameObject tutorialPanel2;
    [SerializeField] private Text tutorialText2;

    protected override void StartSet()
    {
        numberOfEnemy = new int[] { 1 };
        numberOfWave = 1;
        enemyComposition = new EnemyManagerOrigin[1][];
        enemyComposition[0] = new EnemyManagerOrigin[] { commandManager };
        deadEnemyComposition = new bool[1][];
        deadEnemyComposition[0] = new bool[1];
        numberOfArriveEnemy = numberOfEnemy[0];
        tutorialPanel.SetActive(false);
        enemyFrame.SetActive(false);
        sainFrame1.SetActive(false);
        sainFrame2.SetActive(false);
        leaderFrame.SetActive(false);
        systemFrame.SetActive(false);
        tutorialPanel2.SetActive(false);
        StartCoroutine(BattleStart());
    }
    private IEnumerator BattleStart()
    {
        yield return new WaitForSeconds(2);
        tutorialPanel.SetActive(true);
        tutorialText.text = "チュートリアル";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "これからリアルタイム制のバトルが行われます。";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "敵と味方にはそれぞれ攻撃までのインターバルがあります。";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "味方はカウントが0になると戦闘スキルが使用できます。オート使用にすることも可能です。";
        sainFrame1.SetActive(true);
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "カウントが0の状態で待機しているときは敵からのダメージを自動で50%カットします。";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "戦闘スキルにはSG(スキルゲージ)を回復するものと消費するものがあります。";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        sainFrame1.SetActive(false);
        sainFrame2.SetActive(true);
        tutorialText.text = "SGが100に達すると必殺技を使用できます。";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        sainFrame2.SetActive(false);
        enemyFrame.SetActive(true);
        tutorialText.text = "敵はそれぞれチャージゲージを持っています。";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "通常攻撃のたびにゲージが増加し、最大になると強力な攻撃を行います。";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        enemyFrame.SetActive(false);
        leaderFrame.SetActive(true);
        tutorialText.text = "敵の攻撃は着弾まで約1秒あります。あなたはタイミング良くガードすることでダメージをカットできます。";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        tutorialText.text = "また、あなたは味方に体力、攻撃、速度の支援を行うことができます。";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        leaderFrame.SetActive(false);
        systemFrame.SetActive(true);
        tutorialText.text = "全てのウェーブで全ての敵を倒すと勝利、味方のHPが0になると敗北です。";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        systemFrame.SetActive(false);
        tutorialText.text = "次の画面で敵味方の能力の詳細を確認できます。\nでは戦闘を開始しましょう。";
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        yield return null;
        tutorialPanel.SetActive(false);
        explanation.SetActive(true);
        seSource.clip = seUIClick;
        seSource.Play();
        yield return new WaitUntil(() => !explanation.activeSelf);
        battleStartAndFinishText.text = "3";
        seSource.clip = seUIUnactive;
        seSource.Play();
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "2";
        seSource.clip = seCountDown;
        seSource.Play();
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "1";
        seSource.clip = seCountDown;
        seSource.Play();
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "Battle Start";
        seSource.clip = seWhistle;
        seSource.Play();
        yield return new WaitForSeconds(1);
        battleStartAndFinishText.text = "";
        sainManager.Pause = false;
        leaderManager.Pause = false;
        commandManager.Pause = false;
        tutorialPanel2.SetActive(true);
        StartCoroutine(Tutorial());
    }

    private IEnumerator Tutorial()
    {
        while (true)
        {
            tutorialText2.text = "本戦闘のみ、戦い方について追加のアシストを行います。(この説明はループします)";
            yield return new WaitForSeconds(6);
            tutorialText2.text = "初めに、戦闘に慣れるまでは戦闘スキルの使用をオートにすることをおすすめします。";
            yield return new WaitForSeconds(6);
            tutorialText2.text = "オートでは、SGが50溜まるとスキル3を発動し、SGがある限りスキル2を使用します。";
            yield return new WaitForSeconds(6);
            tutorialText2.text = "スキル3は攻撃力を上昇させるとともに攻撃間隔も短縮できる強力なスキルなので、積極的に活用しましょう。";
            yield return new WaitForSeconds(6);
            tutorialText2.text = "スキル3の発動中にリーダースキルの攻撃・速度支援を重ねれば、更に強力な攻撃を更に高速に行えます。";
            yield return new WaitForSeconds(6);
            tutorialText2.text = "スキル2は敵の攻撃間隔を延長させる効果も持つため、HPの低い敵であれば一方的に倒し切ることも可能でしょう。";
            yield return new WaitForSeconds(6);
            tutorialText2.text = "ただし、オートでは必殺技を発動できません。SGを100まで溜めたいときは、手動で戦闘スキルを選択しましょう。";
            yield return new WaitForSeconds(6);
            tutorialText2.text = "アシストは以上となります。初めから全てをこなす必要はありません。少しずつ慣れていきましょう。";
            yield return new WaitForSeconds(6);
        }
    }

    public override void SceneLoad()
    {
        GameManager.instance.SceneName = "MainScene1";
        GameManager.instance.LineNumber = 0;
        GameManager.instance.SainHP = 1000;
        GameManager.instance.SainAttack = 50;
        GameManager.instance.SainSG = 20;
        GameManager.instance.Progress = "1章";
        GameManager.instance.Save();
        SceneManager.LoadScene("MainScene1");
    }
}