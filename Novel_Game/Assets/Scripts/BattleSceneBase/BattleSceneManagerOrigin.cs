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
    [SerializeField] protected TMP_Text battleStartAndFinishText;
    [SerializeField] private GameObject specialSkillAnimation;
    [SerializeField] private Text enemyNumberText;
    [SerializeField] private Text waveNumberText;
    protected int[] numberOfEnemy;
    protected int numberOfWave;
    protected EnemyManagerOrigin[][] enemyComposition;
    protected bool[][] deadEnemyComposition;
    protected int numberOfArriveEnemy;
    protected int numberOfCurrentWave = 0;      //”z—ñ‚ÌƒCƒ“ƒfƒbƒNƒX‚Æ‚µ‚Äg‚¤‚±‚Æ‚ª‘½‚¢‚½‚ß0‚©‚ç
    protected int selectedEnemy = 0;
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

    // Start is called before the first frame update
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
    //ŠeƒV[ƒ“‚Å‚Ì‰Šú‰»ˆ—
    protected abstract void StartSet();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && explanation.activeSelf)
        {
            explanation.SetActive(false);
        }
        //UŒ‚‘ÎÛ‚Ì‘I‘ğ
        if (Input.GetKeyDown(KeyCode.A))
        {
            EnemySelectA();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            EnemySelectD();
        }
    }
    //UŒ‚‘ÎÛ‚Ì‘I‘ğ(“G‚Ì”‚â“|‚³‚ê‚½“G‚É_“î‚É‘Î‰)
    private void EnemySelectA()
    {
        if (selectedEnemy != 0)
        {
            //ˆê‚Â¶‚ÉˆÚ“®
            if (!deadEnemyComposition[numberOfCurrentWave][selectedEnemy - 1])
            {
                enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
                selectedEnemy--;
                enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
                seSource.clip = seUIClick;
                seSource.Play();
            }
            //“ñ‚Â¶(‰E’[‚©‚ç¶’[A^‚ñ’†‚Ì“G‚ª€‚ñ‚Å‚¢‚éê‡)‚ÉˆÚ“®
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
            //ˆê‚Â‰E‚ÉˆÚ“®
            if (!deadEnemyComposition[numberOfCurrentWave][selectedEnemy + 1])
            {
                enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
                selectedEnemy++;
                enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
                seSource.clip = seUIClick;
                seSource.Play();
            }
            //“ñ‚Â‰E‚ÉˆÚ“®
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
    //“G‚ª“|‚ê‚½‚Ì©“®ØŠ·‚¦
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

    //“G‚©‚ç‚ÌUŒ‚
    public void EnemyToSainAttack(int damage)
    {
        sainManager.ReceiveDamage(damage);
    }
    //“G‚©‚ç‚Ì•KE‹Z(ƒK[ƒhE‰ñ”ğ•s‰Â)
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
        seSource.Play();                        //•¡”‚ÌSE‚ğ“¯‚É–Â‚ç‚·‚½‚ß
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
        //0.25•b‚Å400ˆÚ“®
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
    //“G‚Ö‚ÌUŒ‚‚Ì’…’e’n“_æ“¾
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
    //í“¬ƒXƒLƒ‹1
    public IEnumerator SainSkill1(int damage, RectTransform attackRect, Image attackImage)
    {
        seSource.clip = seSword;
        seSource.Play();
        attackImage.color = Color.white;
        float diffX = AttackPoint();
        int selected = selectedEnemy;       //UŒ‚‚ğ‘I‘ğ‚µ‚½“_‚Å‚ÌUŒ‚‘ÎÛ‚ğ•Û
        while (true)
        {
            Vector2 temp = attackRect.anchoredPosition;
            Vector2 temp2 = attackRect.localScale;
            //0.2•b‚ÅÀ•W(x,0)‚Ö
            temp.x += diffX * Time.deltaTime * 5;
            //0.2•b‚Å1/2”{‚Ì‘å‚«‚³‚Ö
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
        //’…’eEÁ–Å
        enemyComposition[numberOfCurrentWave][selected].ReceiveDamage(damage);
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeIn(0.5f, attackImage));
        //‰Šú‰»
        attackRect.anchoredPosition = new(0, 0);
        attackRect.localScale = new Vector2(1, 1);
    }
    //í“¬ƒXƒLƒ‹2
    public IEnumerator SainSkill2(int damage, RectTransform attackRect, Image attackImage)
    {
        seSource.clip = seWind;
        seSource.Play();
        attackImage.color = Color.white;
        float diffX = AttackPoint();
        int selected = selectedEnemy;       //UŒ‚‚ğ‘I‘ğ‚µ‚½“_‚Å‚ÌUŒ‚‘ÎÛ‚ğ•Û
        while (attackRect.localScale.x > 0.5f)
        {
            Vector2 temp = attackRect.anchoredPosition;
            Vector2 temp2 = attackRect.localScale;
            Vector3 temp3 = attackRect.localEulerAngles; 
            //0.2•b‚ÅÀ•W(x,0)‚Ö
            temp.x += diffX * Time.deltaTime * 5;
            //0.2•b‚Å1/2”{‚Ì‘å‚«‚³‚Ö
            temp2.x -= 0.5f * Time.deltaTime * 5;
            temp2.y -= 0.5f * Time.deltaTime * 5;
            //0.5•b‚Å2‰ñ“]
            temp3.z += 720 * Time.deltaTime * 5;
            attackRect.anchoredPosition = temp;
            attackRect.localScale = temp2;
            attackRect.localEulerAngles = temp3;
            yield return null;
        }
        //’…’eEÁ–Å
        enemyComposition[numberOfCurrentWave][selected].ReceiveDamage(damage);
        enemyComposition[numberOfCurrentWave][selected].ReceiveDelay();
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeIn(0.5f, attackImage));
        //‰Šú‰»
        attackRect.anchoredPosition = new(0, 0);
        attackRect.localScale = new Vector2(1, 1);
    }

    //•KE‹Z‚Å‚ÌUŒ‚
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
                //‹­“G‚ÌƒoƒŠƒA”j‰ó
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
    //“G‚ª€‚ñ‚¾‚±‚Æ‚Ìó‚¯æ‚è
    public void EnemyDied()
    {
        //‘Š‘Å‚¿‚¾‚Á‚½‘Îô
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
            //“G‚ğ“|‚µ‚Ä‚©‚ç‚±‚Ì”»’è‚ğ‚·‚é‚Ü‚Å‚Éƒ‰ƒO‚ª‚ ‚é¨C³ ‚»‚ÌŠÔ‚Éƒ^[ƒQƒbƒg‚ğˆÚ“®‚³‚¹‚Ä‚¢‚½‚ç‘I‚Ñ’¼‚·•K—v‚Í‚È‚¢
            else if (deadEnemyComposition[numberOfCurrentWave][selectedEnemy])
            {
                EnemySelect();
            }
        }
    }
    //¶‚«‚Ä‚¢‚é“G‚ÌŠÇ—
    private void ArriveEnemyCheck()
    {
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            deadEnemyComposition[numberOfCurrentWave][i] = enemyComposition[numberOfCurrentWave][i].Dead;
        }
    }
    //•¡‡Œ^‚É‚æ‚é‘B(Ä¶)ˆ—
    public void ReviveEnemy()
    {
        for (int i = 0; i< numberOfEnemy[numberOfCurrentWave]; i++)
        {
            //€‚ñ‚¾‰º‹‰ˆÙ—d‚ª‚¢‚½‚çˆê‘Ì•œŠˆ
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

    //ƒEƒF[ƒu‘JˆÚ
    private IEnumerator GoToNextWave()
    {
        //•KE‹Z‚É‚æ‚Á‚Ä“G‚ğ‘Sˆõ“|‚µ‚½ê‡‚ÉPauseˆ—‚ª‹£‡‚·‚é‚Ì‚ğ”ğ‚¯‚é‚½‚ß
        yield return new WaitUntil(() => !isSpecialAttack);
        sainManager.Pause = true;
        leaderManager.Pause = true;
        battleSystemManager.MenuOff();
        yield return new WaitForSeconds(2);
        StartCoroutine(Wipe());
        yield return new WaitForSeconds(0.5f);//ƒƒCƒv‚É‚æ‚è2•bŠÔˆÃ“]
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

    //ƒEƒF[ƒu‘JˆÚ‚ÌƒƒCƒv(0.5+2+0.5•b)
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

    //ƒQ[ƒ€ƒI[ƒo[ˆ—
    public IEnumerator GameOver()
    {
        //•KE‹Z‚É‚æ‚Á‚Ä“|‚³‚ê‚½ê‡‚ÉPauseˆ—‚ª‹£‡‚·‚é‚Ì‚ğ”ğ‚¯‚é‚½‚ß
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
    //Ÿ—˜&ƒV[ƒ“‘JˆÚ
    private IEnumerator Win()
    {
        //•KE‹Z‚É‚æ‚Á‚Ä“G‚ğ‘Sˆõ“|‚µ‚½ê‡‚ÉPauseˆ—‚ª‹£‡‚·‚é‚Ì‚ğ”ğ‚¯‚é‚½‚ß
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
    //í“¬ŠJn‚Ìà–¾ƒNƒ[ƒY
    public void Close()
    {
        explanation.SetActive(false);
    }
    //ŠeƒV[ƒ“‚É‚¨‚¯‚éƒ[ƒh&‹­§ƒXƒLƒbƒv—p
    public abstract void SceneLoad();
}