using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SainManager : MonoBehaviour
{
    [SerializeField] private GameObject bSManagerObject;
    private BattleSceneManager1 bSManager;
    [SerializeField] private GameObject HPbar;
    [SerializeField] private GameObject SGbar;
    private Slider HPslider;
    private Slider SGslider;
    private int maxHP = 1000;
    private const int maxSG = 100;
    private int currentHP = 1000;
    private int currentSG = 20;
    [SerializeField] private GameObject battleSkill1;
    [SerializeField] private GameObject battleSkill2;
    [SerializeField] private GameObject battleSkill3;
    [SerializeField] private GameObject specialAttack;
    private RectTransform bS1Rect;
    private RectTransform bS2Rect;
    private RectTransform bS3Rect;
    private RectTransform sARect;
    private Image sAImage;
    [SerializeField] private GameObject mask;
    [SerializeField] private GameObject intervalDisplay;
    private Text intervalText;
    private const float interval = 4f;
    private float intervalCount = 0;
    private bool isGuard = false;
    private float attackFactor = 1;
    private float speedFactor = 1;
    private bool pause = false;
    public bool Pause { set { pause = value; } }

    // Start is called before the first frame update
    void Start()
    {
        bSManager = bSManagerObject.GetComponent<BattleSceneManager1>();
        HPslider = HPbar.GetComponent<Slider>();
        SGslider = SGbar.GetComponent<Slider>();
        bS1Rect = battleSkill1.GetComponent<RectTransform>();
        bS2Rect = battleSkill2.GetComponent<RectTransform>();
        bS3Rect = battleSkill3.GetComponent<RectTransform>();
        sARect = specialAttack.GetComponent<RectTransform>();
        sAImage = specialAttack.GetComponent<Image>();
        intervalText = intervalDisplay.GetComponent<Text>();
        sAImage.color = new(0.4f, 0.4f, 0.4f, 1);
        mask.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //インターバル管理
        if (intervalCount > 0 && !pause)
        {
            intervalCount = Mathf.Max(0, intervalCount - Time.deltaTime);
            intervalText.text = intervalCount.ToString("F2");
        }
        //攻撃可能状態なら暗転用マスクを解除
        else if (!pause && mask.activeSelf)
        {
            mask.SetActive(false);
        }
    }

    public void BattleSkill1Click()
    {
        //インターバルが終わっているかの確認
        if (intervalCount == 0　 && !pause)
        {
            intervalCount = interval*speedFactor;
            StartCoroutine(ButtonAnim(bS1Rect));
            mask.SetActive(true);
            //通常攻撃(攻撃小 SG回復10)
            CauseDamage((int)(50*attackFactor));
            currentSG += 10;
            if (currentSG == 100)
            {
                sAImage.color = Color.white;
            }
            SGslider.value = (float)currentSG / maxSG;
        }
    }
    public void BattleSkill2Click()
    {
        //インターバルの確認＆SG>=10
        if (intervalCount == 0 && currentSG >= 10 && !pause)
        {
            intervalCount = interval*speedFactor;
            StartCoroutine(ButtonAnim(bS2Rect));
            mask.SetActive(true);
            //強攻撃(攻撃中 SG消費10 何か追加効果)　案：敵の行動遅延(〇？) 自身の速度上昇 次の攻撃回避
            CauseDamage(100);
            currentSG -= 10;
            SGslider.value = (float)currentSG / maxSG;
        }
    }
    public void BattleSkill3Click()
    {
        //インターバルの確認＆SG>=20
        if (interval == 0 && currentSG >= 20 && !pause)
        {
            intervalCount = interval * speedFactor;
            StartCoroutine(ButtonAnim(bS3Rect));
            mask.SetActive(true);
            //自己強化(SG消費20 インターバル半減 ガードが不可能になる 回避率30%上昇 攻撃力上昇)
            currentSG -= 20;
            SGslider.value = (float)currentSG / maxSG;
        }
    }

    public void SpecialAttackClick()
    {
        //SG>=100ならいつでも
        if (currentSG == maxSG && !pause)
        {
            StartCoroutine(ButtonAnim(sARect));
            sAImage.color = new(0.4f, 0.4f, 0.4f, 1);
            //敵全体に大ダメージ(SG消費100 時間を止めて専用演出 必殺持ちの敵のガードを割る)
            currentSG = 0;
            SGslider.value = 0;
        }
    }

    //ダメージの値を転送
    private void CauseDamage(int damege)
    {
        bSManager.SainToEnemyAttack(damege);
    }
    //ダメージを受ける
    public void ReceiveDamage(int damage)
    {
        //ガード中なら9割カット
        if (isGuard)
        {
            damage /= 10;
        }
        //攻撃待機中なら5割カット
        else if (intervalCount == 0)
        {
            damage /= 2;
        }
        currentHP = Mathf.Max(0, currentHP - damage);
        HPslider.value = (float)currentHP / maxHP;
        if (currentHP == 0)
        {
            //ゲームオーバー処理
        }
    }

    //ガード状態になる
    public IEnumerator ReceiveGuard()
    {
        isGuard = true;
        yield return new WaitForSeconds(0.5f);
        isGuard = false;
    }
    //体力アシストを受ける(暫定400回復)
    public void ReceiveHPAssist()
    {
        currentHP = Mathf.Min(currentHP + 400, maxHP);
        HPslider.value = (float)currentHP / maxHP;
    }
    //攻撃アシストを受ける(暫定10秒間攻撃+1.5倍)
    public IEnumerator ReceiveAttackAssist()
    {
        attackFactor += 0.5f;
        yield return new WaitForSeconds(10);
        attackFactor -= 0.5f;
        //float計算の誤差累積対策
        if ((int)attackFactor == 1)
        {
            attackFactor = 1;
        }
    }
    //速度アシストを受ける(暫定10秒間30%上昇)
    public IEnumerator ReceiveSpeedAssist()
    {
        speedFactor *= 0.7f;
        yield return new WaitForSeconds(10);
        speedFactor /= 0.7f;
        //float計算の誤差累積対策
        if ((int)speedFactor == 1)
        {
            speedFactor = 1;
        }
    }

    //ボタンのアニメーション
    private IEnumerator ButtonAnim(RectTransform rect)
    {
        Vector2 temp = rect.localScale;
        rect.localScale = new(0.9f*temp.x, 0.9f*temp.y);
        yield return new WaitForSeconds(0.1f);
        rect.localScale = temp;
    }
}
