using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BattleSceneManagerOrigin : SystemManagerOrigin
{
    [SerializeField] protected SainManager sainManager;
    [SerializeField] protected LeaderManager leaderManager;
    [SerializeField] protected BattleSystemManager battleSystemManager;
    [SerializeField] protected GameObject blackObject;
    protected RectTransform blackRect;
    protected Image blackImage;
    [SerializeField] protected GameObject explanation;
    [SerializeField] private Text expMessage;
    [SerializeField] private RectTransform expSwitchRect;
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
    private bool isSainAttack = false;
    protected bool isSpecialAttack = false;
    private bool isEnemySpecialAttack = false;
    private bool isGameOver = false;
    [SerializeField] private GameObject specialAttackPanel;
    private RectTransform specialAttackPanelRect;
    private Image specialAttackPanelImage;
    protected AudioSource audioSource;
    [SerializeField] protected AudioClip seCountDown;
    [SerializeField] protected AudioClip seWhistle;
    [SerializeField] private AudioClip seSword;
    [SerializeField] private AudioClip seWind;
    [SerializeField] private AudioClip seSpecialOn;
    [SerializeField] private AudioClip seSpecialFinish;
    [SerializeField] private AudioClip seSpecialDamage;
    [SerializeField] private AudioClip seCymbal;

    void Start()
    {
        blackRect = blackObject.GetComponent<RectTransform>();
        blackImage = blackObject.GetComponent<Image>();
        specialAttackPanelRect = specialAttackPanel.GetComponent<RectTransform>();
        specialAttackPanelImage = specialAttackPanel.GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = GameManager.instance.BgmVolume;
        seSource.volume = GameManager.instance.SeVolume;
        specialAttackPanelImage.color = new(1, 1, 1, 0);
        specialSkillAnimation.SetActive(false);
        explanation.SetActive(false);
        StartCoroutine(FadeIn(1, blackImage));
        StartSet();
    }
    //各シーンでの初期化処理
    protected abstract void StartSet();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && explanation.activeSelf)
        {
            Close();
        }
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
                seSource.clip = seUIClick;
                seSource.Play();
            }
            //二つ左(右端から左端、真ん中の敵が死んでいる場合)に移動
            else if (selectedEnemy == 2 && !deadEnemyComposition[numberOfCurrentWave][0])
            {
                enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
                selectedEnemy = 0;
                enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
                seSource.clip = seUIClick;
                seSource.Play();
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
                seSource.clip = seUIClick;
                seSource.Play();
            }
            //二つ右に移動
            else if (selectedEnemy == 0 && !deadEnemyComposition[numberOfCurrentWave][numberOfEnemy[numberOfCurrentWave] - 1])
            {
                enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
                selectedEnemy = numberOfEnemy[numberOfCurrentWave] - 1;
                enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
                seSource.clip = seUIClick;
                seSource.Play();
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
    //敵の画像(厳密には別オブジェクト)をクリックしたときの対象切り替え
    public void EnemySelect0()
    {
        Select(0);
    }
    public void EnemySelect1()
    {
        Select(1);
    }
    public void EnemySelect2()
    {
        Select(2);
    }
    private void Select(int n)
    {
        if (selectedEnemy != n && !deadEnemyComposition[numberOfCurrentWave][n])
        {
            enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
            selectedEnemy = n;
            enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }

    //敵からの攻撃
    public void EnemyToSainAttack(int damage)
    {
        sainManager.ReceiveDamage(damage);
    }
    //敵からの必殺技(ガード・回避不可)
    public IEnumerator EnemySpecialAttack(int damage)
    {
        sainManager.Pause = true;
        leaderManager.Pause = true;
        isEnemySpecialAttack = true;
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            enemyComposition[numberOfCurrentWave][i].Pause = true;
        }
        yield return new WaitForSeconds(2);
        //specialSkillAnimation.SetActive(true);
        yield return new WaitForSeconds(1.4f);
        seSource.clip = seSpecialFinish;
        seSource.Play();                        //複数のSEを同時に鳴らすため
        yield return new WaitForSeconds(1.2f);
        //specialSkillAnimation.SetActive(false);
        seSource.clip = null;
        sainManager.ReceiveSpecialDamage(damage);
        yield return new WaitForSeconds(2);
        sainManager.Pause = false;
        leaderManager.Pause = false;
        isEnemySpecialAttack = false;
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            enemyComposition[numberOfCurrentWave][i].Pause = false;
        }
    }
    public IEnumerator SpecialAttackName(Sprite sprite)
    {
        seSource.clip = seSpecialOn;
        seSource.Play();
        specialAttackPanelImage.sprite = sprite;
        StartCoroutine(FadeOut(0.25f, specialAttackPanelImage));
        specialAttackPanelRect.anchoredPosition = new(-400, 0);
        //0.25秒で400移動
        while (specialAttackPanelRect.anchoredPosition.x < 0)
        {
            yield return null;
            Vector2 pos = specialAttackPanelRect.anchoredPosition;
            pos.x = Mathf.Min(pos.x + 1600 * Time.deltaTime, 0);
            specialAttackPanelRect.anchoredPosition = pos;
        }
        specialAttackPanelImage.color = Color.white;
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeIn(0.25f, specialAttackPanelImage));
        while (specialAttackPanelRect.anchoredPosition.x < 400)
        {
            yield return null;
            Vector2 pos = specialAttackPanelRect.anchoredPosition;
            pos.x = Mathf.Min(pos.x + 1600 * Time.deltaTime, 400);
            specialAttackPanelRect.anchoredPosition = pos;
        }
        specialAttackPanelImage.color = new(1, 1, 1, 0);
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
        isSainAttack = true;
        seSource.clip = seSword;
        seSource.Play();
        attackImage.color = Color.white;
        float diffX = AttackPoint();
        int selected = selectedEnemy;       //攻撃を選択した時点での攻撃対象を保持
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
        enemyComposition[numberOfCurrentWave][selected].ReceiveDamage(damage);
        isSainAttack = false;
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeIn(0.5f, attackImage));
        //初期化
        attackRect.anchoredPosition = new(0, 0);
        attackRect.localScale = new Vector2(1, 1);
    }
    //戦闘スキル2
    public IEnumerator SainSkill2(int damage, RectTransform attackRect, Image attackImage)
    {
        isSainAttack = true;
        seSource.clip = seWind;
        seSource.Play();
        attackImage.color = Color.white;
        float diffX = AttackPoint();
        int selected = selectedEnemy;       //攻撃を選択した時点での攻撃対象を保持
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
        enemyComposition[numberOfCurrentWave][selected].ReceiveDamage(damage);
        enemyComposition[numberOfCurrentWave][selected].ReceiveDelay();
        isSainAttack = false;
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeIn(0.5f, attackImage));
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
        StartCoroutine(SpecialSE());
        yield return new WaitForSeconds(2.5f);
        seSource.clip = seSpecialDamage;
        seSource.Play();
        specialSkillAnimation.SetActive(false);
        for (int i=0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            if (!deadEnemyComposition[numberOfCurrentWave][i])
            {
                //強敵のバリア破壊
                if (enemyComposition[numberOfCurrentWave][i].ID == 4)
                {
                    enemyComposition[numberOfCurrentWave][i].ShieldBreak();
                }
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
    private IEnumerator SpecialSE()
    {
        seSource.clip = seWind;
        seSource.Play();
        yield return new WaitForSeconds(0.8f);
        for (int i=0; i<4; i++)
        {
            seSource.clip = seSword;
            seSource.Play();
            yield return new WaitForSeconds(0.15f);
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
        //必殺技によって倒された場合にPause処理が競合するのを避けるため
        yield return new WaitUntil(() => !isEnemySpecialAttack);
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
        seSource.clip = seCymbal;
        seSource.Play();
        yield return new WaitForSeconds(2);
        StartCoroutine(VolumeFadeOut(2, audioSource));
        yield return StartCoroutine(FadeOut(2, blackImage));
        SceneLoad();
    }
    //戦闘開始時の説明クローズ
    public void Close()
    {
        explanation.SetActive(false);
        battleSystemManager.IsMessageDisplay = false;
        seSource.clip = seUIUnactive;
        seSource.Play();
    }
    //ポーズ(メニュー表示)が可能か(敵も味方も攻撃中でないか)
    public bool IsPausePossible()
    {
        bool possible = true;
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            if (enemyComposition[numberOfCurrentWave][i].IsAttack)
            {
                possible = false;
                break;
            }
        }
        return possible && !isSainAttack && !isSpecialAttack && !isEnemySpecialAttack && !sainManager.Pause;
    }
    public void Pause()
    {
        sainManager.Pause = true;
        leaderManager.Pause = true;
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            enemyComposition[numberOfCurrentWave][i].Pause = true;
        }
    }
    public void Restart()
    {
        sainManager.Pause = false;
        leaderManager.Pause = false;
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            enemyComposition[numberOfCurrentWave][i].Pause = false;
        }
    }
    //説明の再表示
    public void Explanation()
    {
        StartCoroutine(ButtonAnim(expSwitchRect));
        seSource.clip = seUIClick;
        seSource.Play();
        expMessage.text = "枠外をクリック、またはスペースキーで閉じる";
        explanation.SetActive(true);
        battleSystemManager.IsMessageDisplay = true;
    }
    //各シーンにおけるロード&強制スキップ用
    public abstract void SceneLoad();
}