using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerManager : MonoBehaviour
{
    private Rigidbody _rb;
    private Transform _transform;
    private MeshRenderer _meshRenderer;
    private Animator _animator;
    public GameObject effects;
    public int hp = 3;
    private const int horizontalForce = 40;
    private const int jumpSpeed = 25;
    private const int continueUpForce = 20;
    private const int additionalGravity = -40;
    private const float maxSpeed = 20f;
    private const float jumpTime = 0.2f;
    private float floatingTime = 0;
    private const float frictionForce = 15f;
    private const float deathLine = -10;
    private bool isJumped = false;
    private bool isSecondJumpPossible = false;
    private bool isDamaged = false;
    private bool isInvincible = false;
    private bool isAttacked = false;
    private const float attackTime = 0.6f;

    //プレイヤーの状態を保持
    enum Status
    {
        GROUND = 1,
        UP = 2,
        DOWN = 3
    }

    private Status playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _animator =effects.GetComponent<Animator>();
        playerStatus = Status.DOWN;
    }

    // Update is called once per frame
    void Update()
    {
        //ダメージを受けた直後は操作できない
        if (!isDamaged)
        {
            //右移動
            if (Input.GetKey(KeyCode.D))
            {
                if (playerStatus == Status.GROUND)
                {
                    if (_rb.velocity.x < maxSpeed)
                    {
                        _rb.AddForce(new Vector3(horizontalForce, 0, 0));
                    }
                }
                //空中の横移動を制限
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
                if (playerStatus == Status.GROUND)
                {
                    if (_rb.velocity.x > -maxSpeed)
                    {
                        _rb.AddForce(new Vector3(-horizontalForce, 0, 0));
                    }
                }
                else
                {
                    if (_rb.velocity.x > -maxSpeed * 0.8f)
                    {
                        _rb.AddForce(new Vector3(-horizontalForce * 0.5f, 0, 0));
                    }
                }
            }
            //ジャンプ
            if (Input.GetKeyDown(KeyCode.W) && playerStatus == Status.GROUND)
            {
                Vector3 afterJumpVelocity = _rb.velocity;
                afterJumpVelocity.y = jumpSpeed;
                _rb.velocity = afterJumpVelocity;
                isJumped = true;
            }
            //2段ジャンプ
            if (Input.GetKeyDown(KeyCode.W) && (playerStatus == Status.DOWN || floatingTime > 0.15) && isSecondJumpPossible)
            {
                Vector3 afterJumpVelocity = _rb.velocity;
                afterJumpVelocity.y = jumpSpeed;
                _rb.velocity = afterJumpVelocity;
                floatingTime = 0;
                isJumped = true;
                isSecondJumpPossible = false;
            }
        }
        //空中に出てからの時間を保持
        if (playerStatus != Status.GROUND)
        {
            floatingTime += Time.deltaTime;
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
        //ジャンプ長押し終了
        if (Input.GetKeyUp(KeyCode.W))
        {
            isJumped = false;
        }
        //ジャンプのキレを上げる
        if (playerStatus != Status.GROUND)
        {
            _rb.AddForce(new Vector3(0, additionalGravity, 0));
        }
        //速度プラス検知
        if (_rb.velocity.y > 1e-4)
        {
            playerStatus = Status.UP;
        }
        //速度マイナス検知
        if (_rb.velocity.y < -1e-4)
        {
            playerStatus = Status.DOWN;
        }
        //進行方向を向く
        if (playerStatus == Status.GROUND){
            if (_rb.velocity.x > 0.5f)
            {
                _transform.eulerAngles = new(0, 0, 0);
            }
            if (_rb.velocity.x < -0.5f)
            {
                _transform.eulerAngles = new(0, 180, 0);
            }
        }
        //攻撃時や2段ジャンプ時はキー入力によって向きを変える
        if (Input.GetKeyDown(KeyCode.W) || Input.GetMouseButtonDown(0))
        { 
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                _transform.eulerAngles = new(0, 0, 0);
            }
            if (!Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
            {
                _transform.eulerAngles = new(0, 180, 0);
            }
        }
        //摩擦の代わり
        if ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)))
        {
            if (playerStatus == Status.GROUND)
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
                }
            }
        }
        //落下判定
        if (_transform.position.y < deathLine)
        {
            SceneManager.LoadScene("MainScene");
        }
        //強攻撃(アニメーションをプレイヤーに直接設定すると挙動がおかしくなるため子オブジェクトのみアニメーションで動かす)
        if (Input.GetMouseButtonDown(1) && !isAttacked)
        {
            isAttacked = true;
            StartCoroutine(AttackFinish());
            _animator.SetTrigger("Attack2");
            if (_transform.eulerAngles.y == 0)
            {
                _transform.position += new Vector3(5, 0, 0);
            }
            else
            {
                _transform.position += new Vector3(-5, 0, 0);
            }
        }
    }

    //private void FixedUpdate()
    //{
        
    //}

    private void OnCollisionStay(Collision collision)
    {
        //地上判定&滞空時間のリセット
        if (collision.gameObject.CompareTag("Field") && Mathf.Abs(_rb.velocity.y) <= 1e-4)
        {
            playerStatus = Status.GROUND;
            floatingTime = 0;
            isSecondJumpPossible = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //ダメージ時(ダメージ後の無敵時間を除く)
        if ((other.CompareTag("Enemy") || other.CompareTag("Bullet")) && !isInvincible)
        {
            isDamaged = true;
            isInvincible = true;
            hp--;
            if (other.CompareTag("Bullet"))
            {
                other.enabled = false;
            }
            if (hp == 0)
            {
                SceneManager.LoadScene("MainScene");
            }
            Vector3 decreasedVelocity = new(0.2f * _rb.velocity.x, 0.2f * _rb.velocity.y, 0.2f * _rb.velocity.z);
            _rb.velocity = decreasedVelocity;
            StartCoroutine(Damage());
        }
    }


    //ダメージ時の点滅&操作不能や無敵時間の管理
    private IEnumerator Damage()
    {
        _meshRenderer.enabled = false;
        yield return new WaitForSeconds(0.2f);
        _meshRenderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        isDamaged = false;
        _meshRenderer.enabled = false;
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.2f);
            _meshRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
            _meshRenderer.enabled = false;
        }
        yield return new WaitForSeconds(0.2f);
        isInvincible = false;
        _meshRenderer.enabled = true;
    }

    //再攻撃可能までのタイマー
    private IEnumerator AttackFinish()
    {
        yield return new WaitForSeconds(attackTime);
        isAttacked = false;
    }
    }
