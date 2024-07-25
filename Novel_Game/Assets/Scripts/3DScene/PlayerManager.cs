using System.Collections;
using UnityEngine;

//いずれは全体的な見直しが必要(非常にデバッグがしにくい)

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [SerializeField] private StageManagerOrigin stageManager;
    [SerializeField] private GameObject character;
    private Animator playerAnimator;
    private Rigidbody _rb;
    private Transform _transform;
    private const float deathLine = -10;
    private const int gravity = -40;
    private const int jumpSpeed = 15;
    private const int continueUpForce = 10;
    private const float jumpTime = 0.2f;
    private float floatingTime = 0;
    private bool isJumped = false;
    private bool isSecondJumpPossible = false;
    private bool fallingAccel = false;
    private const int horizontalForce = 20;
    private const float maxSpeed = 10f;
    private const float frictionForce = 8f;
    [SerializeField] private Collider normalAttackCollider;
    [SerializeField] private GameObject normalAttackEffect;
    private bool isDamaged = false;
    private bool isInvincible = false;
    private bool isAttacked = false;
    private const float attack1Interval = 0.5f;
    private bool attackPossible = true;
    [SerializeField] GameObject attack2EffectsFolder;
    private GameObject[] attack2Effects;
    private int attack2EffectsCount;
    private bool isAttack2 = false;
    public bool IsAttack2 { get { return isAttack2; } }
    private bool attack2Stop = false;
    private const int attack2Possible = 3;
    private int attack2Count = 0;
    private const float attack2reception = 0.12f;
    private bool clear = false;
    public bool Clear { set { clear = value; } }
    //プレイヤーのアニメーション(ポーズ)を管理
    private enum PlayerState
    {
        IDLING = 0,
        DASH = 1,
        TURNBACK = 2,
        JUMPUP = 3,
        JUMPDOWN = 4,
        SQUAT = 5,
        ATTACK1 = 6,
        ATTACK2 = 7,
    }
    private PlayerState playerState;
    //プレイヤーの状態(場所)を管理
    private enum PositionState
    {
        GROUND = 1,
        UP = 2,
        DOWN = 3
    }
    private PositionState positionState;
    //浮き床に渡す用(強攻撃時すり抜け)
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = character.GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        playerState = PlayerState.JUMPDOWN;
        positionState = PositionState.DOWN;
        normalAttackCollider.enabled = false;
        normalAttackEffect.SetActive(false);
        playerAnimator.SetInteger("PlayerState", (int)playerState);
        attack2EffectsCount = attack2EffectsFolder.transform.childCount;
        attack2Effects = new GameObject[attack2EffectsCount];
        for (int i = 0; i < attack2EffectsCount; i++)
        {
            attack2Effects[i] = attack2EffectsFolder.transform.GetChild(i).gameObject;
            attack2Effects[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ダメージを受けた直後、攻撃直後、クリア後は操作できない
        if (!isDamaged && !isAttacked)
        {
            //ジャンプ
            if (Input.GetKeyDown(KeyCode.W) && positionState == PositionState.GROUND)
            {
                playerState = PlayerState.JUMPUP;
                positionState = PositionState.UP;
                playerAnimator.SetInteger("PlayerState", (int)playerState);
                Vector3 afterJumpVelocity = _rb.velocity;
                afterJumpVelocity.y = jumpSpeed;
                _rb.velocity = afterJumpVelocity;
                isJumped = true;
                fallingAccel = false;
            }
            //2段ジャンプ
            if (Input.GetKeyDown(KeyCode.W) && (positionState == PositionState.DOWN || floatingTime > 0.15) && isSecondJumpPossible)
            {
                //方向転換が可能
                if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                {
                    _rb.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                else if (!Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
                {
                    _rb.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
                playerState = PlayerState.JUMPUP;
                positionState = PositionState.UP;
                playerAnimator.SetInteger("PlayerState", (int)playerState);
                Vector3 afterJumpVelocity = _rb.velocity;
                afterJumpVelocity.y = jumpSpeed;
                _rb.velocity = afterJumpVelocity;
                floatingTime = 0;
                isJumped = true;
                isSecondJumpPossible = false;
                fallingAccel = false;
            }
        }
        //空中に出てからの時間を保持
        if (positionState != PositionState.GROUND)
        {
            floatingTime += Time.deltaTime;
        }
        //ジャンプ長押し終了
        if (Input.GetKeyUp(KeyCode.W))
        {
            isJumped = false;
        }
        //速度マイナス検知(しゃがんでいるとき(若干浮いている可能性がある))
        if (_rb.velocity.y < -1e-4 && positionState != PositionState.DOWN && playerState != PlayerState.SQUAT)
        {
            positionState = PositionState.DOWN;
            //攻撃中にはポーズは変えない
            if (!isAttacked)
            {
                playerState = PlayerState.JUMPDOWN;
                playerAnimator.SetInteger("PlayerState", (int)playerState);
            }
        }
        //ダッシュ中の方向転換(強攻撃の直前に方向が変わってしまわないように)
        if (playerState == PlayerState.DASH && !isAttack2)
        {
            if (Input.GetKeyDown(KeyCode.D) && _rb.rotation != Quaternion.Euler(0f, 0f, 0f))
            {
                _rb.rotation = Quaternion.Euler(0f, 0f, 0f);
                playerState = PlayerState.TURNBACK;
                playerAnimator.SetInteger("PlayerState", (int)playerState);
            }
            if (Input.GetKeyDown(KeyCode.A) && _rb.rotation != Quaternion.Euler(0f, 180f, 0f))
            {
                _rb.rotation = Quaternion.Euler(0f, 180f, 0f);
                playerState = PlayerState.TURNBACK;
                playerAnimator.SetInteger("PlayerState", (int)playerState);
            }
        }
        //折り返し・しゃがみ状態からダッシュへ(しゃがみ時間を測る代わりに2段ジャンプ可否変数を使用)
        if (playerState == PlayerState.TURNBACK || (playerState == PlayerState.SQUAT && isSecondJumpPossible))
        {
            //右を向いているとき
            if (_rb.rotation == Quaternion.Euler(0f, 0f, 0f))
            {
                if (_rb.velocity.x > 1e-4)
                {
                    playerState = PlayerState.DASH;
                    playerAnimator.SetInteger("PlayerState", (int)playerState);
                }
                //反対方向が入力されたら即座に向きを変える
                if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                {
                    _rb.rotation = Quaternion.Euler(0f, 180f, 0f);
                    playerState = PlayerState.DASH;
                    playerAnimator.SetInteger("PlayerState", (int)playerState);
                }
            }
            //左を向いているとき
            if (_rb.rotation == Quaternion.Euler(0f, 180f, 0f))
            {
                if (_rb.velocity.x < -1e-4)
                {
                    playerState = PlayerState.DASH;
                    playerAnimator.SetInteger("PlayerState", (int)playerState);
                }
                //反対方向が入力されたら即座に向きを変える
                if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                {
                    _rb.rotation = Quaternion.Euler(0f, 0f, 0f);
                    playerState = PlayerState.DASH;
                    playerAnimator.SetInteger("PlayerState", (int)playerState);
                }
            }
        }
        //待機時は動き始めた方向に合わせて方向転換(キー両押し→片方離すなどに対処するため) 強攻撃時は状態がリセットされるため方向を変えられないようにする
        if (playerState == PlayerState.IDLING && !isAttack2)
        {
            if (_rb.velocity.x > 1e-4)
            {
                _rb.rotation = Quaternion.Euler(0f, 0f, 0f);
                playerState = PlayerState.DASH;
                playerAnimator.SetInteger("PlayerState", (int)playerState);
            }
            if (_rb.velocity.x < -1e-4)
            {
                _rb.rotation = Quaternion.Euler(0f, 180f, 0f);
                playerState = PlayerState.DASH;
                playerAnimator.SetInteger("PlayerState", (int)playerState);
            }
        }
        //地上でジャンプ状態の場合強制的に待機状態へ
        if (playerState == PlayerState.JUMPDOWN && positionState == PositionState.GROUND)
        {
            playerState = PlayerState.IDLING;
            playerAnimator.SetInteger("PlayerState", (int)playerState);
        }
        //下キーで落下加速
        if (positionState == PositionState.DOWN && !isAttack2)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                fallingAccel = true;
            }
        }
        //通常攻撃
        if (Input.GetMouseButtonDown(0) && attackPossible && !clear)
        {
            StartCoroutine(NormalAttack());
        }
        //強攻撃
        if (Input.GetMouseButtonDown(1) && attackPossible && attack2Count == 0)
        {
            StartCoroutine(StrongAttack());
        }
        //落下判定(何らかのミスでクリア後に落ちても反応しない)
        if (_transform.position.y < deathLine　&& !clear)
        {
            stageManager.GameOver();
        }
    }

    //通常攻撃(終わる瞬間に着地したときに着地判定による状態変化と競合する……？)(地上でジャンプ姿勢だった時に待機へ強制的に戻す処理を追加)
    private IEnumerator NormalAttack()
    {
        //ポーズの変更、1フレームの間当たり判定を出す、攻撃中状態・攻撃可能状態の管理
        playerState = PlayerState.ATTACK1;
        playerAnimator.SetInteger("PlayerState", (int)(playerState));
        normalAttackCollider.enabled = true;
        //エフェクトをその場に留める
        StartCoroutine(StopEffect(normalAttackEffect, attack1Interval));
        isAttacked = true;
        attackPossible = false;
        //下降中なら下降を止め若干浮く
        if (positionState == PositionState.DOWN)
        {
            Vector3 temp = _rb.velocity;
            temp.y = 0;
            _rb.velocity = temp;
            _rb.AddForce(new Vector3(0, continueUpForce*4, 0));
        }
        yield return new WaitForSeconds(0.05f);
        normalAttackCollider.enabled = false;
        yield return new WaitForSeconds(attack1Interval);
        isAttacked = false;
        //地上ならダッシュか待機へ
        if (positionState == PositionState.GROUND)
        {
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                _rb.rotation = Quaternion.Euler(0f, 0f, 0f);
                playerState = PlayerState.DASH;
                playerAnimator.SetInteger("PlayerState", (int)playerState);
            }
            else if (!Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
            {
                _rb.rotation = Quaternion.Euler(0f, 180f, 0f);
                playerState = PlayerState.DASH;
                playerAnimator.SetInteger("PlayerState", (int)playerState);
            }
            else
            {
                playerState = PlayerState.IDLING;
                playerAnimator.SetInteger("PlayerState", (int)playerState);
            }
        }
        //空中なら下降状態へ(攻撃間隔長め)
        else
        {
            playerState = PlayerState.JUMPDOWN;
            playerAnimator.SetInteger("PlayerState", (int)playerState);
            yield return new WaitForSeconds(attack1Interval);
        }
        attackPossible = true;
    }
    //強攻撃
    private IEnumerator StrongAttack()
    {
        isAttacked = true;
        isAttack2 = true;
        attackPossible = false;
        _rb.velocity = Vector3.zero;
        attack2Count++;
        //前回の進行方向を保持するための変数
        int former;
        //空中でも方向転換が可能
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            _rb.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (!Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            _rb.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        if (_rb.rotation.eulerAngles.y == 0)
        {
            yield return StartCoroutine(SAExecute(2));
            playerState = PlayerState.ATTACK1;
            playerAnimator.SetInteger("PlayerState", (int)playerState);
            former = 2;
        }
        else
        {
            yield return StartCoroutine(SAExecute(6));
            playerState = PlayerState.ATTACK1;
            playerAnimator.SetInteger("PlayerState", (int)playerState);
            former = 6;
        }
        //受付時間内に再び入力されたら再発動できる
        while (attack2Count < attack2Possible)
        {
            //受付時間と同じだけの時間は完全に硬直
            yield return new WaitForSeconds(attack2reception);
            float timeCount = 0;
            bool attacked = false;
            while (timeCount < attack2reception)
            {
                yield return null;
                timeCount += Time.deltaTime;
                //8方向に移動できる(優先は右と上)
                if (Input.GetMouseButtonDown(1))
                {
                    //右上
                    if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && former != 3){
                        yield return StartCoroutine(SAExecute(3));
                        positionState = PositionState.UP;
                        playerState = PlayerState.ATTACK2;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 3;
                    }
                    //右下
                    else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) && former != 1)
                    {
                        yield return StartCoroutine(SAExecute(1));
                        playerState = PlayerState.ATTACK2;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 1;
                    }
                    //左上
                    else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && former != 5)
                    {
                        yield return StartCoroutine(SAExecute(5));
                        positionState = PositionState.UP;
                        playerState = PlayerState.ATTACK2;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 5;
                    }
                    //左下
                    else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) && former != 7)
                    {
                        yield return StartCoroutine(SAExecute(7));
                        playerState = PlayerState.ATTACK2;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 7;
                    }
                    //右
                    else if (Input.GetKey(KeyCode.D) && former != 2)
                    {
                        yield return StartCoroutine(SAExecute(2));
                        playerState = PlayerState.ATTACK1;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 2;
                    }
                    //左
                    else if (Input.GetKey(KeyCode.A) && former != 6)
                    {
                        yield return StartCoroutine(SAExecute(6));
                        playerState = PlayerState.ATTACK1;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 6;
                    }
                    //上
                    else if (Input.GetKey(KeyCode.W) && former != 4)
                    {
                        yield return StartCoroutine(SAExecute(4));
                        positionState = PositionState.UP;
                        playerState = PlayerState.ATTACK2;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 4;
                    }
                    //下
                    else if (Input.GetKey(KeyCode.S) && former != 0)
                    {
                        yield return StartCoroutine(SAExecute(0));
                        playerState = PlayerState.ATTACK2;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 0;
                    }
                    //移動できない方向だった場合入力受付状態に戻る
                    else
                    {
                        continue;
                    }
                    attack2Count++;
                    //最終段まで攻撃した場合の硬直
                    if (attack2Count == attack2Possible)
                    {
                        yield return new WaitForSeconds(attack2reception);
                    }
                    attacked = true;
                    break;
                }
            }
            //受付中に攻撃していなかった場合攻撃回数が残っていても終了
            if (!attacked)
            {
                break;
            }
        }
        //終了処理
        Vector3 temp = _rb.rotation.eulerAngles;
        temp.z = 0;
        _rb.rotation = Quaternion.Euler(temp);
        //回転によりめり込んだ場合に着地判定が出ないように
        yield return null;
        isAttack2 = false;
        isAttacked = false;
        if (positionState == PositionState.GROUND)
        {
            attack2Count = 0;
            playerState = PlayerState.IDLING;
            playerAnimator.SetInteger("PlayerState", (int)playerState);
        }
        else
        {
            playerState = PlayerState.JUMPDOWN;
            playerAnimator.SetInteger("PlayerState", (int)playerState);
            yield return new WaitForSeconds(attack1Interval);
        }
        attackPossible = true;
    }
    //約4m移動しつつ斬撃(足元側(オブジェクトの回転中心に対して当たり判定が長い側)が地面を向いたときにめり込みが解消できないことが分かっている)
    private IEnumerator SAExecute(int n)
    {
        int[] theta = { -90, -45, 0, 45, 90, 135, 180, -135 };
        Vector3 v = new(4 * Mathf.Cos(theta[n] * Mathf.PI / 180), 4 * Mathf.Sin(theta[n] * Mathf.PI / 180), 0);
        //体の向きの切り替え
        if (1 <= n && n <= 3)
        {
            _rb.rotation = Quaternion.Euler(0f, 0f, theta[n]);
        }
        else if (n == 5 || n == 6)
        {
            _rb.rotation = Quaternion.Euler(0f, 180f, 180f-theta[n]);
        }
        else if (n == 7)
        {
            _rb.rotation = Quaternion.Euler(0f, 180f, -45f);
        }
        else
        {
            Vector3 temp = _rb.rotation.eulerAngles;
            temp.z = theta[n];
            _rb.rotation = Quaternion.Euler(temp);
        }
        character.SetActive(false);
        yield return null;              //埋まり状態解消用
        float oneAttackTime = 0.05f;
        _rb.velocity = v / (oneAttackTime*attack2EffectsCount);
        int attackCount = 0;
        attack2Stop = false;
        while (attackCount < attack2EffectsCount)
        {
            if (attack2Stop)
            {
                _rb.velocity = Vector3.zero;
            }
            normalAttackCollider.enabled = true;
            StartCoroutine(StopEffect(attack2Effects[attackCount], 0.2f));
            attackCount++;
            yield return new WaitForSeconds(oneAttackTime);
            normalAttackCollider.enabled = false;
        }
        _rb.velocity = Vector3.zero;
        character.SetActive(true);
        attack2Stop = false;
    }
    //斬撃をその場に残すための関数
    private IEnumerator StopEffect(GameObject effect, float durationTime)
    {
        effect.SetActive(true);
        Vector3 effectPosition = effect.transform.position;
        Vector3 effectRotation = effect.transform.rotation.eulerAngles;
        Vector3 ePdefault = effect.transform.localPosition;
        Vector3 eRdefault = effect.transform.localEulerAngles;
        float timeCount = 0;
        while (timeCount < durationTime)
        {
            effect.transform.SetPositionAndRotation(effectPosition, Quaternion.Euler(effectRotation));
            timeCount += Time.deltaTime;
            yield return null;
        }
        effect.SetActive(false);
        effect.transform.SetLocalPositionAndRotation(ePdefault, Quaternion.Euler(eRdefault));
    }
    private void FixedUpdate()
    {
        //重力
        if (!isAttack2)
        {
            //下キーを押した場合
            if (fallingAccel)
            {
                _rb.AddForce(new Vector3(0, 1.5f * gravity, 0));
            }
            //通常
            else
            {
                _rb.AddForce(new Vector3(0, gravity, 0));
            }
        }
        //ダメージを受けた直後、攻撃直後、クリア後は操作できない
        if (!isDamaged && !isAttacked)
        {
            //右移動
            if (Input.GetKey(KeyCode.D))
            {
                if (positionState == PositionState.GROUND)
                {
                    if (_rb.velocity.x < maxSpeed)
                    {
                        _rb.AddForce(new Vector3(horizontalForce, 0, 0));
                    }
                    //何らかのバグでダッシュ方向と体の向きが違っていた時用
                    else if (playerState == PlayerState.DASH && _rb.rotation == Quaternion.Euler(0f, 180f, 0f))
                    {
                        _rb.rotation = Quaternion.Euler(0f, 0f, 0f);
                    }
                    //何らかのバグで最高速度なのにダッシュ状態じゃなかった時用
                    else if (playerState != PlayerState.DASH)
                    {
                        playerState = PlayerState.DASH;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                    }
                }
                //空中では制限
                else
                {
                    if (_rb.velocity.x < maxSpeed * 0.8f)
                    {
                        _rb.AddForce(new Vector3(horizontalForce * 0.5f, 0, 0));
                    }
                }
            }
            //左移動
            if (Input.GetKey(KeyCode.A))
            {
                if (positionState == PositionState.GROUND)
                {
                    if (_rb.velocity.x > -maxSpeed)
                    {
                        _rb.AddForce(new Vector3(-horizontalForce, 0, 0));
                    }
                    //何らかのバグでダッシュ方向と体の向きが違っていた時用
                    else if (playerState == PlayerState.DASH && _rb.rotation == Quaternion.Euler(0f, 0f, 0f))
                    {
                        _rb.rotation = Quaternion.Euler(0f, 180f, 0f);
                    }
                    //何らかのバグで最高速度なのにダッシュ状態じゃなかった時用
                    else if (playerState != PlayerState.DASH)
                    {
                        playerState = PlayerState.DASH;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                    }
                }
                //空中では制限
                else
                {
                    if (_rb.velocity.x > -maxSpeed * 0.8f)
                    {
                        _rb.AddForce(new Vector3(-horizontalForce * 0.5f, 0, 0));
                    }
                }
            }
        }
        //摩擦(空中ではRigidBodyの抵抗だけで)
        if (((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))) && !isAttacked)
        {
            if (positionState == PositionState.GROUND)
            {
                if (_rb.velocity.x > 1)
                {
                    _rb.AddForce(new Vector3(-frictionForce, 0, 0));
                }
                else if (_rb.velocity.x < -1)
                {
                    _rb.AddForce(new Vector3(frictionForce, 0, 0));
                }
                else if (Mathf.Abs(_rb.velocity.x) < 1)
                {
                    _rb.velocity = Vector3.zero;
                    //しゃがみ状態からすぐには変わらないようにする
                    if (isSecondJumpPossible)
                    {
                        playerState = PlayerState.IDLING;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                    }
                }
            }
        }
        //通常攻撃後は強く摩擦をかける
        if (isAttacked)
        {
            if (positionState == PositionState.GROUND)
            {
                if (_rb.velocity.x > 1)
                {
                    _rb.AddForce(new Vector3(-frictionForce*2, 0, 0));
                }
                else if (_rb.velocity.x < -1)
                {
                    _rb.AddForce(new Vector3(frictionForce*2, 0, 0));
                }
            }
        }
        //長押しでジャンプ延長
        if (isJumped)
        {
            _rb.AddForce(new Vector3(0, continueUpForce, 0));
            if (floatingTime > jumpTime)
            {
                isJumped = false;
            }
        }
    }

    //ダメージ時(ダメージ後の無敵時間および攻撃中を除く)&コイン獲得
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Enemy") || other.CompareTag("Bullet")) && !isInvincible && !isAttacked)
        {
            isDamaged = true;
            isInvincible = true;
            attackPossible = false;
            stageManager.Damage();
            if (other.CompareTag("Bullet"))
            {
                other.enabled = false;
                other.gameObject.SetActive(false);
            }
            Vector3 decreasedVelocity = new(0.2f * _rb.velocity.x, 0.2f * _rb.velocity.y, 0.2f * _rb.velocity.z);
            _rb.velocity = decreasedVelocity;
            StartCoroutine(Damage());
        }
        if (other.CompareTag("Medal"))
        {
            stageManager.GotMedal();
        }
    }
    //ダメージ時の点滅&操作不能や無敵時間の管理
    private IEnumerator Damage()
    {
        character.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        character.SetActive(true);
        playerAnimator.SetInteger("PlayerState", (int)playerState);
        yield return new WaitForSeconds(0.2f);
        isDamaged = false;
        character.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.2f);
            character.SetActive(true);
            playerAnimator.SetInteger("PlayerState", (int)playerState);
            yield return new WaitForSeconds(0.2f);
            character.SetActive(false);
            playerAnimator.SetInteger("PlayerState", (int)playerState);
        }
        yield return new WaitForSeconds(0.2f);
        isInvincible = false;
        attackPossible = true;
        character.SetActive(true);
        playerAnimator.SetInteger("PlayerState", (int)playerState);
    }
    //強攻撃時用の地形衝突判定
    public void FieldDitected()
    {
        if (isAttack2)
        {
            attack2Stop = true;
        }
    }
    //着地時のしゃがみ(着地時点で押していたキーの方向を向く)(着地判定は専用のColliderから受け取る)(浮き床を下から通り抜けた時反応しないように下降中以外は省く)
    public IEnumerator Squat()
    {
        //落下中かつ強攻撃をしていないとき(体が回転していると色々とまずそう)
        if (positionState == PositionState.DOWN && !isAttack2)
        {
            //攻撃姿勢の場合はポーズを変えない
            if (!isAttacked)
            {
                playerState = PlayerState.SQUAT;
                playerAnimator.SetInteger("PlayerState", (int)playerState);
            }
            positionState = PositionState.GROUND;
            floatingTime = 0;
            attack2Count = 0;
            isSecondJumpPossible = false;
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                _rb.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                _rb.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            yield return new WaitForSeconds(0.15f);
            isSecondJumpPossible = true;    //着地後すぐに2回目のジャンプをすることはないのでしゃがみ中であることを表す変数としても使える
        }
    }
    //何らかの理由で着地判定ができなかった時用
    public void SetGround()
    {
        if ((positionState == PositionState.DOWN || playerState == PlayerState.JUMPDOWN) && !isAttack2)
        {
            playerState = PlayerState.IDLING;
            positionState = PositionState.GROUND;
            playerAnimator.SetInteger("PlayerState", (int)playerState);
            floatingTime = 0;
            attack2Count = 0;
            isSecondJumpPossible = true;
        }
    }
}
