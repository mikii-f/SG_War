using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class SainManager : SystemManagerOrigin
{
    [SerializeField] private BattleSceneManagerOrigin bSManager;
    [SerializeField] private RectTransform myRect;
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private TMP_Text SGText;
    [SerializeField] private Slider HPslider;
    [SerializeField] private Slider SGslider;
    private int maxHP = 1000;
    private const int maxSG = 120;
    private int currentHP = 1000;
    private int currentSG = 20;
    private int attack = 50;
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private GameObject specialAttack;
    [SerializeField] private RectTransform bS1Rect;
    [SerializeField] private RectTransform bS2Rect;
    [SerializeField] private RectTransform bS3Rect;
    private RectTransform sARect;
    private Image sAImage;
    [SerializeField] private GameObject mask;
    [SerializeField] private GameObject mask2;
    [SerializeField] private GameObject mask3;
    [SerializeField] private GameObject attack1Effect;
    private RectTransform attack1Rect;
    private Image attack1Image;
    [SerializeField] private GameObject attack2Effect;
    private RectTransform attack2Rect;
    private Image attack2Image;
    [SerializeField] private GameObject buffEffect;
    [SerializeField] private GameObject healEffect;
    [SerializeField] private GameObject guardEffect;
    [SerializeField] private Text intervalText;
    private const float interval = 4f;
    private float intervalCount = 0;
    [SerializeField] private GameObject buffDebuffFolder;
    private int buffDebuffNumber;
    private Image[] buffAndDebuffs;
    private int[] buffIndex;
    [SerializeField] private Sprite noneSprite;
    [SerializeField] private Sprite attackBuffIcon;
    [SerializeField] private Sprite speedBuffIcon;
    [SerializeField] private Sprite avoidBuffIcon;
    //[SerializeField] private Sprite cannotGuardIcon;
    [SerializeField] private GameObject commentPanel;
    private RectTransform commentPanelRect;
    [SerializeField] private Text comment;
    [SerializeField] private Sprite specialAttackNameSprite;
    private bool isGuard = false;
    //private bool isCannotGuard = false;
    //public bool IsCannotGuard { get { return isCannotGuard; } }   ガード不可はデメリットとして大きすぎるのでいったん外す
    private bool isSkill3 = false;
    private bool isInvincible = false;
    //整数値で係数を扱うことで簡単化(floatは色々とめんどい)(/10の処理に問題がないよう気をつける)
    private int attackFactor = 10;
    private int speedFactor = 10;
    private int avoidFactor = 0;
    private float buffTimer = 0;
    private bool pause = true;
    public bool Pause { set { pause = value; } }
    private bool auto = false;
    public bool Auto { set { auto = value; } get { return auto; } }
    private Coroutine commentCoroutine;
    private Coroutine damageCoroutine;

    void Start()
    {
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        maxHP = Mathf.Max(GameManager.instance.SainHP, 1000);       //開発中におけるGameManagerの値が設定されていない状況用
        attack = Mathf.Max(GameManager.instance.SainAttack, 50);
        currentSG = Mathf.Max(GameManager.instance.SainSG, 20);
        currentHP = maxHP;
        HPslider.value = 1;
        SGslider.value = (float)currentSG / maxSG;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        SGText.text = currentSG.ToString() + "/" + maxSG.ToString();
        sARect = specialAttack.GetComponent<RectTransform>();
        sAImage = specialAttack.GetComponent<Image>();
        attack1Rect = attack1Effect.GetComponent<RectTransform>();
        attack1Image = attack1Effect.GetComponent<Image>();
        attack1Image.color = new(1, 1, 1, 0);
        attack2Rect = attack2Effect.GetComponent<RectTransform>();
        attack2Image = attack2Effect.GetComponent<Image>();
        attack2Image.color = new(1, 1, 1, 0);
        buffDebuffNumber = buffDebuffFolder.transform.childCount;
        buffAndDebuffs = new Image[buffDebuffNumber];
        buffIndex = new int[buffDebuffNumber];
        for (int i = 0; i < buffDebuffNumber; i++)
        {
            buffAndDebuffs[i] = buffDebuffFolder.transform.GetChild(i).GetComponent<Image>();
            buffIndex[i] = -1;
        }
        commentPanelRect = commentPanel.GetComponent<RectTransform>();
        commentPanel.SetActive(false);
        sAImage.color = new(0.4f, 0.4f, 0.4f, 1);
        mask.SetActive(false);
        mask2.SetActive(false);
        mask3.SetActive(false);
        //1戦目はスキルゲージが増えない
        if (SceneManager.GetActiveScene().name == "BattleScene1")
        {
            mask2.SetActive(true);
            mask3.SetActive(true);
            currentSG = 0;
            SGText.text = currentSG.ToString() + "/" + maxSG.ToString();
            SGslider.value = 0;
        }
        buffEffect.SetActive(false);
        healEffect.SetActive(false);
        guardEffect.SetActive(false);
    }

    void Update()
    {
        if (!pause)
        {
            //バフの持続時間計測用(流石にfloatの表現範囲超える可能性は考えなくていいはず……)
            buffTimer += Time.deltaTime;
            //インターバル管理
            if (intervalCount > 0)
            {
                intervalCount = Mathf.Max(0, intervalCount - Time.deltaTime * speedFactor / 10);
                intervalText.text = intervalCount.ToString("F2");
            }
            //攻撃可能状態なら暗転用マスクを解除
            else if (mask.activeSelf)
            {
                mask.SetActive(false);
            }
            //戦闘スキルおよび必殺の選択
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                BattleSkill1Click();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                BattleSkill2Click();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                BattleSkill3Click();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                SpecialAttackClick();
            }
            //オート時(プレイヤーも操作はできる)(多分競合はしないはず……)
            if (auto)
            {
                if (intervalCount == 0)
                {
                    //SG>=50でS3を使用
                    if (currentSG >= 50 && !isSkill3)
                    {
                        BattleSkill3Click();
                    }
                    //S3使用中はスキル2を優先して使用
                    else if (isSkill3 && currentSG >= 10)
                    {
                        BattleSkill2Click();
                    }
                    else
                    {
                        BattleSkill1Click();
                    }
                }
            }
        }
    }

    public void BattleSkill1Click()
    {
        //インターバルが終わっているかの確認
        if (intervalCount == 0　 && !pause)
        {
            intervalCount = interval;
            StartCoroutine(ButtonAnim(bS1Rect));
            mask.SetActive(true);
            //通常攻撃(攻撃力の100% SG回復10)
            StartCoroutine(bSManager.SainSkill1(attack*attackFactor/10, attack1Rect, attack1Image));
            //1戦目は通常攻撃しかできない
            if (SceneManager.GetActiveScene().name != "BattleScene1")
            {
                currentSG = Mathf.Min(maxSG, currentSG + 10);
            }
            SGCheck();
        }
    }
    public void BattleSkill2Click()
    {
        //インターバルの確認＆SG>=10
        if (intervalCount == 0 && currentSG >= 10 && !pause)
        {
            intervalCount = interval;
            StartCoroutine(ButtonAnim(bS2Rect));
            mask.SetActive(true);
            //強攻撃(攻撃力の200% SG消費10 攻撃態勢に入る前の敵の行動遅延)
            StartCoroutine(bSManager.SainSkill2(2*attack*attackFactor/10, attack2Rect, attack2Image));
            currentSG -= 10;
            SGCheck();
        }
    }
    //クリックの受け取りはコルーチンにできないっぽい
    public void BattleSkill3Click()
    {
        //インターバルの確認＆SG>=20＆スキル3使用中でない
        if (intervalCount == 0 && currentSG >= 20 && !pause && !isSkill3)
        {
            StartCoroutine(BattleSkill3());
        }
    }
    private IEnumerator BattleSkill3()
    {
        //即座に再行動可能
        StartCoroutine(ButtonAnim(bS3Rect));
        mask.SetActive(true);
        StartCoroutine(EffectOnandOff(buffEffect));
        //バフ状況の管理
        int index = -1;
        for (int i=0; i<buffDebuffNumber; i++)
        {
            if (buffAndDebuffs[i].sprite == noneSprite)
            {
                buffAndDebuffs[i].sprite = speedBuffIcon;
                buffAndDebuffs[i + 1].sprite = avoidBuffIcon;
                buffAndDebuffs[i + 2].sprite = attackBuffIcon;
                //buffAndDebuffs[i + 3].sprite = cannotGuardIcon;
                for (int j=0; j<buffDebuffNumber; j++)
                {
                    if (buffIndex[j] == -1)
                    {
                        index = j;
                        buffIndex[j] = i;
                        break;
                    }
                }
                break;
            }
        }
        //自己強化(SG消費20 インターバル半減 回避率30%上昇 攻撃力上昇) 重ね掛け不可
        currentSG -= 20;
        speedFactor += 10;
        avoidFactor += 3;
        attackFactor += 4;
        isSkill3 = true;
        SGCheck();
        float tempTimer = buffTimer;
        yield return new WaitUntil(() => buffTimer - tempTimer >= 10);
        //バフ状況の管理
        BuffIndexCheck(index, 3);
        speedFactor -= 10;
        avoidFactor -= 3;
        attackFactor -= 4;
        isSkill3 = false;
        if (currentSG >= 20)
        {
            mask3.SetActive(false);
        }
    }

    public void SpecialAttackClick()
    {
        //SG>=100ならいつでも
        if (currentSG >= 100 && !pause)
        {
            StartCoroutine(ButtonAnim(sARect));
            StartCoroutine(Invincible(1));
            StartCoroutine(bSManager.SpecialAttackName(specialAttackNameSprite));
            //敵全体に攻撃力500%(SG消費100 時間を止めて専用演出 必殺持ちの敵のガードを割る)
            StartCoroutine(bSManager.SainToAllAttack(5*attack * attackFactor / 10));
            currentSG -= 100;
            SGCheck();
        }
    }
    
    //主に必殺技発動直後のための無敵時間
    private IEnumerator Invincible(float invincibleTime)
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    //SG値によってマスク切り替え
    private void SGCheck()
    {
        SGslider.value = (float)currentSG / maxSG;
        SGText.text = currentSG.ToString() + "/" + maxSG.ToString();
        if (currentSG >= 100)
        {
            sAImage.color = Color.white;
        }
        else
        {
            sAImage.color = new(0.4f, 0.4f, 0.4f, 1);
        }
        if (currentSG >= 20 && !isSkill3)
        {
            mask3.SetActive(false);
        }
        else
        {
            mask3.SetActive(true);
        }
        if (currentSG >= 10)
        {
            mask2.SetActive(false);
        }
        else
        {
            mask2.SetActive(true);
        }
    }

    //ダメージを受ける
    public void ReceiveDamage(int damage)
    {
        //ポーズ中(勝利後)はノーダメ
        if (pause)
        {
            damage = 0;
        }
        //回避に成功したらノーダメ
        else if (UnityEngine.Random.Range(0, 10) < avoidFactor)
        {
            damage = 0;
            if (commentCoroutine != null)
            {
                StopCoroutine(commentCoroutine);
                ResetComment();
            }
            commentCoroutine = StartCoroutine(Comment("回避成功"));
        }
        //無敵時間ならノーダメ
        else if (isInvincible)
        {
            damage = 0;
        }
        //ガード中なら9割カット
        else if (isGuard)
        {
            damage /= 10;
            if (commentCoroutine != null)
            {
                StopCoroutine(commentCoroutine);
                ResetComment();
            }
            commentCoroutine = StartCoroutine(Comment("ガード成功"));
        }
        //攻撃待機中なら5割カット
        else if (intervalCount == 0)
        {
            damage /= 2;
        }
        //ガードなどをしていなかった時は振動
        else
        {
            StartCoroutine(DamageVibration());
        }
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        damageCoroutine = StartCoroutine(DamageDisplay(damage));
        currentHP = Mathf.Max(0, currentHP - damage);
        HPslider.value = (float)currentHP / maxHP;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        if (currentHP == 0)
        {
            StartCoroutine(bSManager.GameOver());
        }
    }
    //必殺技ダメージを受ける(ガード等無効)
    public void ReceiveSpecialDamage(int damage)
    {
        StartCoroutine(DamageVibration());
        StartCoroutine(DamageDisplay(damage));
        currentHP = Mathf.Max(0, currentHP - damage);
        HPslider.value = (float)currentHP / maxHP;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        if (currentHP == 0)
        {
            StartCoroutine(bSManager.GameOver());
        }
    }

    //ダメージ表示(同時に2つの攻撃を受けた時も分かりやすいようにしたい)
    private IEnumerator DamageDisplay(int damage)
    {
        damageText.text = damage.ToString();
        yield return new WaitForSeconds(0.35f);
        damageText.text = "";
    }
    //ガード・回避成功時のコメント
    private IEnumerator Comment(string text)
    {
        comment.text = text;
        commentPanel.SetActive(true);
        while (commentPanelRect.anchoredPosition.x < 450)
        {
            yield return null;
            Vector2 pos = commentPanelRect.anchoredPosition;
            pos.x += 1000 * Time.deltaTime;
            commentPanelRect.anchoredPosition = pos;
        }
        yield return new WaitForSeconds(1);
        commentPanel.SetActive(false);
        Vector2 temp = commentPanelRect.anchoredPosition;
        temp.x = 250;
        commentPanelRect.anchoredPosition = temp;
    }
    //0.5秒の間で2回呼び出された時用
    private void ResetComment()
    {
        commentPanel.SetActive(false);
        Vector2 temp = commentPanelRect.anchoredPosition;
        temp.x = 250;
        commentPanelRect.anchoredPosition = temp;
    }
    //ダメージ受け取り時の揺れ
    private IEnumerator DamageVibration()
    {
        Vector2 temp = myRect.anchoredPosition;
        temp.x += 10;
        myRect.anchoredPosition = temp;
        yield return new WaitForSeconds(0.1f);
        temp.x -= 20;
        myRect.anchoredPosition = temp;
        yield return new WaitForSeconds(0.1f);
        temp.x += 20;
        myRect.anchoredPosition = temp;
        yield return new WaitForSeconds(0.1f);
        temp.x -= 20;
        myRect.anchoredPosition = temp;
        yield return new WaitForSeconds(0.1f);
        temp.x = 0;
        myRect.anchoredPosition = temp;
    }

    //ガード状態になる
    public IEnumerator ReceiveGuard()
    {
        isGuard = true;
        guardEffect.SetActive(true);
        float tempTimer = buffTimer;
        yield return new WaitUntil(() => buffTimer - tempTimer >= 0.5f);
        isGuard = false;
        guardEffect.SetActive(false);    
    }
    //体力アシストを受ける(暫定5割回復)
    public void ReceiveHPAssist()
    {
        currentHP = Mathf.Min(currentHP + maxHP/2, maxHP);
        HPslider.value = (float)currentHP / maxHP;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        StartCoroutine(EffectOnandOff(healEffect));
    }
    //攻撃アシストを受ける(暫定15秒間攻撃+1.5倍)
    public IEnumerator ReceiveAttackAssist()
    {
        StartCoroutine(EffectOnandOff(buffEffect));
        attackFactor += 5;
        float tempTimer = buffTimer;
        //バフ状況の管理
        int index = -1;
        for (int i = 0; i < buffDebuffNumber; i++)
        {
            if (buffAndDebuffs[i].sprite == noneSprite)
            {
                buffAndDebuffs[i].sprite = attackBuffIcon;
                for (int j = 0; j < buffDebuffNumber; j++)
                {
                    if (buffIndex[j] == -1)
                    {
                        index = j;
                        buffIndex[j] = i;
                        break;
                    }
                }
                break;
            }
        }
        yield return new WaitUntil(() => buffTimer - tempTimer >= 15);
        BuffIndexCheck(index, 1);
        attackFactor -= 5;
    }
    //速度アシストを受ける(暫定20秒間5上昇)
    public IEnumerator ReceiveSpeedAssist()
    {
        StartCoroutine(EffectOnandOff(buffEffect));
        speedFactor += 5;
        float tempTimer = buffTimer;
        //バフ状況の管理
        int index = -1;
        for (int i = 0; i < buffDebuffNumber; i++)
        {
            if (buffAndDebuffs[i].sprite == noneSprite)
            {
                buffAndDebuffs[i].sprite = speedBuffIcon;
                for (int j = 0; j < buffDebuffNumber; j++)
                {
                    if (buffIndex[j] == -1)
                    {
                        index = j;
                        buffIndex[j] = i;
                        break;
                    }
                }
                break;
            }
        }
        yield return new WaitUntil(() => buffTimer - tempTimer >= 20);
        BuffIndexCheck(index, 1);
        speedFactor -= 5;
    }

    //バフなどのアニメーション(フェードいる？)
    private IEnumerator EffectOnandOff(GameObject effect)
    {
        //重ね掛け時はとりあえず最初のだけ表示
        if (!effect.activeSelf)
        {
            effect.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            effect.SetActive(false);
        }
    }
    //バフ状況の管理
    //バフ発動時に取得したインデックスが適宜変更されていく形
    //(例：0番目・1番目のアイコン枠がnullでなく2番目にアイコンを置いた→インデックス保持配列の値が-1のところを見つけそのインデックスを一時的に記憶し2を格納→
    //この関数が適切にインデックスを管理→バフ終了時にインデックス保持配列が指す場所のアイコンをnullにし配列の値を-1に戻す)
    //第1引数は一時的に記憶したインデックス、第2引数は消すバフの数
    private void BuffIndexCheck(int n, int m)
    {
        //アイコン書き換え
        for (int i = buffIndex[n]; i < buffIndex[n]+m; i++)
        {
            buffAndDebuffs[i].sprite = noneSprite;
        }
        for (int i= buffIndex[n] + m; i<buffDebuffNumber; i++)
        {
            buffAndDebuffs[i - m].sprite = buffAndDebuffs[i].sprite;
            buffAndDebuffs[i].sprite = noneSprite;
        }
        //その他のアイコンのインデックス管理
        for (int i=0; i<buffDebuffNumber; i++)
        {
            if (buffIndex[i] > buffIndex[n])
            {
                buffIndex[i] -= m;
            }
        }
        buffIndex[n] = -1;
    }
}