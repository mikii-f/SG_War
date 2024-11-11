using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExtraBattle1Manager : BattleSceneManagerOrigin
{
    [SerializeField] private ElManager elManager;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Text tutorialText;
    [SerializeField] private AudioClip bgmEl;

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
        tutorialText.text = "仮想空間なら今のエルも全力で戦うことができます。フィアと共に本気で特訓に臨みましょう。";
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        seSource.clip = seUIClick;
        seSource.Play();
        yield return null;
        tutorialPanel.SetActive(false);
        explanation.SetActive(true);
        yield return new WaitUntil(() => !explanation.activeSelf);
        StartCoroutine(VolumeFadeOut(2, audioSource));
        battleStartAndFinishText.text = "3";
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
        audioSource.clip = bgmEl;
        audioSource.volume = GameManager.instance.BgmVolume;
        audioSource.Play();
        sainManager.Pause = false;
        leaderManager.Pause = false;
        elManager.Pause = false;
    }

    public override void SceneLoad()
    {
        SceneManager.LoadScene("AfterClear");
    }
}
