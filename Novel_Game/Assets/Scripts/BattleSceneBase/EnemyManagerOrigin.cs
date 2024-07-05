using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyManagerOrigin : MonoBehaviour
{
    [SerializeField] private GameObject bSManagerObject;
    protected BattleSceneManagerOrigin bSManager;
    [SerializeField] private GameObject myAllObject;
    public bool AllObject { set { myAllObject.SetActive(value); } }
    [SerializeField] private GameObject myObject;
    protected RectTransform myRect;
    protected Image myImage;
    [SerializeField] private GameObject namePanel;
    [SerializeField] private GameObject gagePanel;
    private RectTransform gageRect;
    [SerializeField] private GameObject HPbar;
    private Slider HPslider;
    [SerializeField] private GameObject HPTextObject;
    private TMP_Text HPText;
    [SerializeField] protected Sprite grayGage;
    [SerializeField] protected Sprite redGage;
    [SerializeField] protected int maxHP;
    [SerializeField] protected int attack;
    protected int maxGage;
    protected int currentHP;
    protected int currentGage;
    [SerializeField] private GameObject intervalDisplay;
    private Text intervalText;
    protected float interval;
    protected float intervalCount;
    protected bool isAttack = false;
    private bool isDied = false;
    public bool Dead { get { return isDied; } }
    private bool pause = true;
    public bool Pause { set { pause = value; } }

    // Start is called before the first frame update
    void Start()
    {
        bSManager = bSManagerObject.GetComponent<BattleSceneManagerOrigin>();
        myRect = myObject.GetComponent<RectTransform>();
        myImage = myObject.GetComponent<Image>();
        gageRect = gagePanel.GetComponent<RectTransform>();
        HPslider = HPbar.GetComponent<Slider>();
        HPText = HPTextObject.GetComponent<TMP_Text>();
        currentHP = maxHP;
        HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
        intervalText = intervalDisplay.GetComponent<Text>();
        StartSet();
    }
    protected abstract void StartSet();

    //通常とチャージの2種類しか攻撃を持たない雑魚エネミー用 ボスはオーバーライドする
    void Update()
    {
        if (!pause)
        {
            //攻撃中以外で生きている間はインターバルをカウント
            if (intervalCount > 0 && !isAttack && !isDied)
            {
                intervalCount = Mathf.Max(0, intervalCount - Time.deltaTime);
                intervalText.text = intervalCount.ToString("F2");
            }
            else if (intervalCount == 0)
            {
                if (currentGage < maxGage)
                {
                    intervalCount = interval;
                    //通常攻撃
                    currentGage++;
                    StartCoroutine(NormalAttack());
                }
                else if (currentGage == maxGage)
                {
                    intervalCount = interval;
                    //チャージ技
                    currentGage = 0;
                    StartCoroutine(ChargeAttack());
                }
            }
        }
    }
    protected abstract IEnumerator NormalAttack();
    protected abstract IEnumerator ChargeAttack();

    //デフォルトの攻撃演出((1-0.5)秒で着弾)
    protected IEnumerator AttackEffect(RectTransform attackRect, Image attackImage)
    {
        attackImage.color = Color.white;
        float diffX = -attackRect.anchoredPosition.x;
        float diffY = -100;
        float diffScale = attackRect.localScale.x * 2;
        while (true)
        {
            Vector2 temp = attackRect.anchoredPosition;
            Vector2 temp2 = attackRect.localScale;
            //0.5秒で座標(0,-100)へ
            temp.x += diffX * Time.deltaTime * 2;
            temp.y += diffY * Time.deltaTime * 2;
            //0.5秒で3倍の大きさへ
            temp2.x += diffScale * Time.deltaTime * 2;
            temp2.y += diffScale * Time.deltaTime * 2;
            attackRect.anchoredPosition = temp;
            attackRect.localScale = temp2;
            if (temp.y <= -100)
            {
                break;
            }
            yield return null;
        }
        //着弾・消滅
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
        attackRect.anchoredPosition = new (-diffX, 0);
        attackRect.localScale = new Vector2(1, 1);
    }
    //ダメージ受け取り
    public void ReceiveDamage(int damage)
    {
        //今は死んだ敵も完全に消滅させていないため、条件を付けないと必殺で2回目の消滅判定が起きる(必殺側でも対応済みだが念のため)
        if (!isDied)
        {
            currentHP = Mathf.Max(0, currentHP - damage);
            HPslider.value = (float)currentHP / maxHP;
            HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
            StartCoroutine(DamageVibration());
            if (currentHP == 0)
            {
                StartCoroutine(Died());
            }
        }
    }
    //ダメージ受け取り時の揺れ
    private IEnumerator DamageVibration()
    {
        Vector2 temp = myRect.anchoredPosition;
        //float計算の誤差対策
        float defaultX = temp.x;
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
        temp.x = defaultX;
        myRect.anchoredPosition = temp;
    }
    //消滅
    private IEnumerator Died()
    {
        isDied = true;
        float waitTime = 0.1f;
        float fadeTime = 1;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alphaChangeAmount)
        {
            Color newColor = myImage.color;
            newColor.a = alpha / 255.0f;
            myImage.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
        myAllObject.SetActive(false);
        bSManager.EnemyDied();
    }
    //攻撃対象として選択されたときのUI拡大縮小
    public void Select()
    {
        namePanel.SetActive(true);
        HPTextObject.SetActive(true);
        gageRect.localScale = Vector3.one;
    }
    public void DisSelect()
    {
        namePanel.SetActive(false);
        HPTextObject.SetActive(false);
        gageRect.localScale = new(0.5f, 0.5f);
    }
}