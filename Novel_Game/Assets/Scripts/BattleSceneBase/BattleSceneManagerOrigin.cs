using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BattleSceneManagerOrigin : MonoBehaviour
{
    [SerializeField] protected SainManager sainManager;
    [SerializeField] protected LeaderManager leaderManager;
    [SerializeField] protected BattleSystemManager battleSystemManager;
    [SerializeField] protected GameObject blackObject;
    protected RectTransform blackRect;
    protected Image blackImage;
    [SerializeField] protected GameObject explanation;
    [SerializeField] protected TMP_Text battleStartAndFinishText;
    [SerializeField] private GameObject specialSkillAnimation;
    [SerializeField] private Text enemyNumberText;
    [SerializeField] private Text waveNumberText;
    protected int[] numberOfEnemy;
    protected int numberOfWave;
    protected EnemyManagerOrigin[][] enemyComposition;
    protected bool[][] deadEnemyComposition;
    protected int numberOfArriveEnemy;
    protected int numberOfCurrentWave = 0;      //配列のインデックスとして使うことが多いため0から
    protected int selectedEnemy = 0;
    protected bool isSpecialAttack = false;
    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        blackRect = blackObject.GetComponent<RectTransform>();
        blackImage = blackObject.GetComponent<Image>();
        specialSkillAnimation.SetActive(false);
        explanation.SetActive(false);
        StartCoroutine(FadeIn(1, blackImage));
        StartSet();
    }
    //各シーンでの初期化処理
    protected abstract void StartSet();

    // Update is called once per frame
    void Update()
    {
        //同時押しなどによるバグをなくすためには全てif-elseで繋ぐべきなのか？その場合どれの優先度を高くする？別スクリプトは？
        //攻撃対象の選択
        if (Input.GetKeyDown(KeyCode.A))
        {
            EnemySelectA();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            EnemySelectD();
        }
    }
    //攻撃対象の選択(敵の数や倒された敵に柔軟に対応)
    private void EnemySelectA()
    {
        if (selectedEnemy != 0)
        {
            //一つ左に移動
            if (!deadEnemyComposition[numberOfCurrentWave][selectedEnemy - 1])
            {
                enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
                selectedEnemy--;
                enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
            }
            //二つ左(右端から左端、真ん中の敵が死んでいる場合)に移動
            else if (selectedEnemy == 2 && !deadEnemyComposition[numberOfCurrentWave][0])
            {
                enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
                selectedEnemy = 0;
                enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
            }
        }
    }
    private void EnemySelectD()
    {
        if (selectedEnemy != numberOfEnemy[numberOfCurrentWave]-1)
        {
            //一つ右に移動
            if (!deadEnemyComposition[numberOfCurrentWave][selectedEnemy + 1])
            {
                enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
                selectedEnemy++;
                enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
            }
            //二つ右に移動
            else if (selectedEnemy == 0 && !deadEnemyComposition[numberOfCurrentWave][numberOfEnemy[numberOfCurrentWave] - 1])
            {
                enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
                selectedEnemy = numberOfEnemy[numberOfCurrentWave] - 1;
                enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
            }
        }
    }
    //敵が倒れた時の自動切換え
    private void EnemySelect()
    {
        enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            if (!deadEnemyComposition[numberOfCurrentWave][i])
            {
                selectedEnemy = i;
                break;
            }
        }
        enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
    }

    //敵からの攻撃
    public void EnemyToSainAttack(int damage)
    {
        sainManager.ReceiveDamage(damage);
    }
    //敵への攻撃の着弾地点取得
    private float AttackPoint()
    {
        switch (numberOfEnemy[numberOfCurrentWave])
        {
            case 1:
                return 0;
            case 2:
                if (selectedEnemy == 0)
                {
                    return -300;
                }
                else
                {
                    return 300;
                }
            case 3:
                if (selectedEnemy == 0)
                {
                    return -500;
                }
                else if (selectedEnemy == 1)
                {
                    return 0;
                }
                else
                {
                    return 500;
                }
            default:
                return 0;
        }
    }
    //戦闘スキル1
    public IEnumerator SainSkill1(int damage, RectTransform attackRect, Image attackImage)
    {
        attackImage.color = Color.white;
        float diffX = AttackPoint();
        while (true)
        {
            Vector2 temp = attackRect.anchoredPosition;
            Vector2 temp2 = attackRect.localScale;
            //0.2秒で座標(x,0)へ
            temp.x += diffX * Time.deltaTime * 5;
            //0.2秒で1/2倍の大きさへ
            temp2.x -= 0.5f * Time.deltaTime * 5;
            temp2.y -= 0.5f * Time.deltaTime * 5;
            attackRect.anchoredPosition = temp;
            attackRect.localScale = temp2;
            if (temp2.x <= 0.5f)
            {
                break;
            }
            yield return null;
        }
        //着弾・消滅
        enemyComposition[numberOfCurrentWave][selectedEnemy].ReceiveDamage(damage);
        float waitTime = 0.05f;
        float fadeTime = 0.5f;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alphaChangeAmount)
        {
            Color newColor = attackImage.color;
            newColor.a = alpha / 255.0f;
            attackImage.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
        //初期化
        attackRect.anchoredPosition = new(0, 0);
        attackRect.localScale = new Vector2(1, 1);
    }
    //戦闘スキル2
    public IEnumerator SainSkill2(int damage, RectTransform attackRect, Image attackImage)
    {
        attackImage.color = Color.white;
        float diffX = AttackPoint();
        while (attackRect.localScale.x > 0.5f)
        {
            Vector2 temp = attackRect.anchoredPosition;
            Vector2 temp2 = attackRect.localScale;
            Vector3 temp3 = attackRect.localEulerAngles; 
            //0.2秒で座標(x,0)へ
            temp.x += diffX * Time.deltaTime * 5;
            //0.2秒で1/2倍の大きさへ
            temp2.x -= 0.5f * Time.deltaTime * 5;
            temp2.y -= 0.5f * Time.deltaTime * 5;
            //0.5秒で2回転
            temp3.z += 720 * Time.deltaTime * 5;
            attackRect.anchoredPosition = temp;
            attackRect.localScale = temp2;
            attackRect.localEulerAngles = temp3;
            yield return null;
        }
        //着弾・消滅
        enemyComposition[numberOfCurrentWave][selectedEnemy].ReceiveDamage(damage);
        enemyComposition[numberOfCurrentWave][selectedEnemy].ReceiveDelay();
        float waitTime = 0.05f;
        float fadeTime = 0.5f;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alphaChangeAmount)
        {
            Color newColor = attackImage.color;
            newColor.a = alpha / 255.0f;
            attackImage.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
        //初期化
        attackRect.anchoredPosition = new(0, 0);
        attackRect.localScale = new Vector2(1, 1);
    }

    //必殺技での攻撃
    public IEnumerator SainToAllAttack(int damage)
    {
        sainManager.Pause = true;
        leaderManager.Pause = true;
        isSpecialAttack = true;
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            enemyComposition[numberOfCurrentWave][i].Pause = true;
        }
        yield return new WaitForSeconds(2);
        specialSkillAnimation.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        specialSkillAnimation.SetActive(false);
        for (int i=0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            if (!deadEnemyComposition[numberOfCurrentWave][i])
            {
                enemyComposition[numberOfCurrentWave][i].ReceiveDamage(damage);
            }
        }
        yield return new WaitForSeconds(2);
        sainManager.Pause = false;
        leaderManager.Pause = false;
        isSpecialAttack = false;
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            enemyComposition[numberOfCurrentWave][i].Pause = false;
        }
    }
    //敵が死んだことの受け取り
    public void EnemyDied()
    {
        //相打ちだった時対策
        if (!isGameOver)
        {
            numberOfArriveEnemy--;
            ArriveEnemyCheck();
            enemyNumberText.text = "Enemy " + numberOfArriveEnemy.ToString() + "/" + numberOfEnemy[numberOfCurrentWave].ToString();
            if (numberOfArriveEnemy == 0 && numberOfCurrentWave + 1 == numberOfWave)
            {
                StartCoroutine(Win());
            }
            else if (numberOfArriveEnemy == 0)
            {
                StartCoroutine(GoToNextWave());
            }
            //敵を倒してからこの判定をするまでにラグがある→修正 その間にターゲットを移動させていたら選び直す必要はない
            else if (deadEnemyComposition[numberOfCurrentWave][selectedEnemy])
            {
                EnemySelect();
            }
        }
    }
    //生きている敵の管理
    private void ArriveEnemyCheck()
    {
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            deadEnemyComposition[numberOfCurrentWave][i] = enemyComposition[numberOfCurrentWave][i].Dead;
        }
    }
    //複合型による増殖(再生)処理
    public void ReviveEnemy()
    {
        for (int i = 0; i< numberOfEnemy[numberOfCurrentWave]; i++)
        {
            //死んだ下級異妖がいたら一体復活
            if (deadEnemyComposition[numberOfCurrentWave][i] && enemyComposition[numberOfCurrentWave][i].ID == 0)
            {
                enemyComposition[numberOfCurrentWave][i].Revive();
                numberOfArriveEnemy++;
                ArriveEnemyCheck();
                enemyNumberText.text = "Enemy " + numberOfArriveEnemy.ToString() + "/" + numberOfEnemy[numberOfCurrentWave].ToString();
                break;
            }
        }
    }

    //ウェーブ遷移
    private IEnumerator GoToNextWave()
    {
        //必殺技によって敵を全員倒した場合にPause処理が競合するのを避けるため
        yield return new WaitUntil(() => !isSpecialAttack);
        sainManager.Pause = true;
        leaderManager.Pause = true;
        battleSystemManager.MenuOff();
        yield return new WaitForSeconds(2);
        StartCoroutine(Wipe());
        yield return new WaitForSeconds(0.5f);//ワイプにより2秒間暗転
        numberOfCurrentWave++;
        selectedEnemy = 0;
        numberOfArriveEnemy = numberOfEnemy[numberOfCurrentWave];
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            enemyComposition[numberOfCurrentWave][i].AllObject = true;
        }
        enemyNumberText.text = "Enemy " + numberOfArriveEnemy.ToString() + "/" + numberOfEnemy[numberOfCurrentWave].ToString();
        waveNumberText.text = "Wave " + (numberOfCurrentWave+1).ToString() + "/" + numberOfWave.ToString();
        yield return new WaitForSeconds(4);
        battleSystemManager.MenuOn();
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            enemyComposition[numberOfCurrentWave][i].Pause = false;
        }
        sainManager.Pause = false;
        leaderManager.Pause = false;
    }

    //戦闘開始時・終了時のフェード
    protected IEnumerator FadeIn(float fadeTime, Image image)
    {
        float waitTime = 0.1f;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alphaChangeAmount)
        {
            Color newColor = image.color;
            newColor.a = alpha / 255.0f;
            image.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
    }
    protected IEnumerator FadeOut(float fadeTime, Image image)
    {
        float waitTime = 0.1f;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 0.0f; alpha <= 255.0f; alpha += alphaChangeAmount)
        {
            Color newColor = image.color;
            newColor.a = alpha / 255.0f;
            image.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
    }
    //ウェーブ遷移時のワイプ(0.5+2+0.5秒)
    private IEnumerator Wipe()
    {
        blackRect.anchoredPosition = new(-1920, 0);
        blackImage.color = Color.black;
        while (blackRect.anchoredPosition.x < 0)
        {
            yield return null;
            Vector2 pos = blackRect.anchoredPosition;
            pos.x = Mathf.Min(0, pos.x + 3840 * Time.deltaTime);
            blackRect.anchoredPosition = pos;
        }
        yield return new WaitForSeconds(2);
        while (blackRect.anchoredPosition.x < 1920)
        {
            yield return null;
            Vector2 pos = blackRect.anchoredPosition;
            pos.x = Mathf.Min(1920, pos.x + 3840 * Time.deltaTime);
            blackRect.anchoredPosition = pos;
        }
        blackImage.color = Color.clear;
        blackRect.anchoredPosition = new (0, 0);
    }

    //ゲームオーバー処理
    public IEnumerator GameOver()
    {
        isGameOver = true;
        sainManager.Pause = true;
        leaderManager.Pause = true;
        battleSystemManager.MenuOff();
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            enemyComposition[numberOfCurrentWave][i].Pause = true;
        }
        yield return new WaitForSeconds(1);
        battleSystemManager.GameOver();
    }
    //勝利&シーン遷移
    private IEnumerator Win()
    {
        //必殺技によって敵を全員倒した場合にPause処理が競合するのを避けるため
        yield return new WaitUntil(() => !isSpecialAttack);
        sainManager.Pause = true;
        leaderManager.Pause = true;
        battleSystemManager.MenuOff();
        yield return new WaitForSeconds(2);
        battleStartAndFinishText.text = "Battle Finish";
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(FadeOut(2, blackImage));
        SceneLoad();
    }
    //戦闘開始時の説明クローズ
    public void Close()
    {
        explanation.SetActive(false);
    }
    //各シーンにおけるロード&強制スキップ用
    public abstract void SceneLoad();
}