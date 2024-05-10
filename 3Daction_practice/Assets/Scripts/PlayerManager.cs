using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerManager : MonoBehaviour
{
    private Rigidbody _rb;
    private Transform _transform;
    private Collider _collider;
    private MeshRenderer _meshRenderer;
    public GameObject effect2;      //本番では別の実現方法を目指す
    private Collider _effect2Collider;
    private Animator _effect2Animator;
    public LayerMask mainField;
    public int hp = 3;
    private const int horizontalForce = 40;
    private const int jumpSpeed = 30;
    private const int continueUpForce = 30;
    private const int gravity = -80;
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
    private bool isAttack2enable = true;
    private bool fallingAccel = false;
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
        Application.targetFrameRate = 60;           //60FPS固定
        _rb = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _collider = GetComponent<Collider>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _effect2Collider = effect2.GetComponent<Collider>();
        _effect2Animator = effect2.GetComponent<Animator>();
        _effect2Collider.enabled = false;
        playerStatus = Status.DOWN;
    }

    // Update is called once per frame
    void Update()
    {
        //ダメージを受けた直後、強攻撃直後は操作できない
        if (!isDamaged && !isAttacked)
        {
            //ジャンプ
            if (Input.GetKeyDown(KeyCode.W) && playerStatus == Status.GROUND)
            {
                Vector3 afterJumpVelocity = _rb.velocity;
                afterJumpVelocity.y = jumpSpeed;
                _rb.velocity = afterJumpVelocity;
                isJumped = true;
                fallingAccel = false;
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
                fallingAccel = false;
            }
        }
        //空中に出てからの時間を保持
        if (playerStatus != Status.GROUND)
        {
            floatingTime += Time.deltaTime;
        }
        //ジャンプ長押し終了
        if (Input.GetKeyUp(KeyCode.W))
        {
            isJumped = false;
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
        if (playerStatus == Status.GROUND)
        {
            if (_rb.velocity.x > 0.5f && _rb.rotation != Quaternion.Euler(0f, 0f, 0f))
            {
                _rb.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            if (_rb.velocity.x < -0.5f && _rb.rotation != Quaternion.Euler(0f, 180f, 0f))
            {
                _rb.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
        //下キーで落下加速
        if (playerStatus == Status.DOWN)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                fallingAccel = true;
            }
        }
        //攻撃時や2段ジャンプ時はキー入力によって向きを変える
        if (Input.GetKeyDown(KeyCode.W) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        { 
            if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                _rb.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            if (!Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
            {
                _rb.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
        //落下判定
        if (_transform.position.y < deathLine)
        {
            SceneManager.LoadScene("MainScene");
        }
        //強攻撃(アニメーションをプレイヤーに直接設定すると挙動がおかしくなるため子オブジェクトのみアニメーションで動かす)
        if (Input.GetMouseButtonDown(1) && !isAttacked && isAttack2enable)
        {
            isAttacked = true;
            isAttack2enable = false;
            _collider.enabled = false;
            fallingAccel = false;
            _effect2Animator.SetTrigger("Attack2");
            _effect2Collider.enabled = true;
            StartCoroutine(AttackFinish());
            _rb.velocity = Vector3.zero;

            //障害物検知用(停止位置や移動距離は適宜わかりやすい名前の変数に置き換え)
            RaycastHit hit;

            //移動方向はキー入力優先、次点で現在の向き
            if (Input.GetKey(KeyCode.D))
            {
                if (Physics.Raycast(_transform.position, Vector3.right, out hit, 5f, mainField))
                {
                    // 障害物がある場合は、障害物の手前まで瞬間移動する
                    _rb.position = hit.point + Vector3.left;
                }
                else
                {
                    _rb.position += new Vector3(5, 0, 0);
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (Physics.Raycast(_transform.position, Vector3.left, out hit, 5f, mainField))
                {
                    // 障害物がある場合は、障害物の手前まで瞬間移動する
                    _rb.position = hit.point + Vector3.right;
                }
                else
                {
                    _rb.position += new Vector3(-5, 0, 0);
                }
            }
            else if (_transform.eulerAngles.y == 0)
            {
                if (Physics.Raycast(_transform.position, Vector3.right, out hit, 5f, mainField))
                {
                    // 障害物がある場合は、障害物の手前まで瞬間移動する
                    _rb.position = hit.point + Vector3.left;
                }
                else
                {
                    _rb.position += new Vector3(5, 0, 0);
                }
            }
            else
            {
                if (Physics.Raycast(_transform.position, Vector3.left, out hit, 5f, mainField))
                {
                    // 障害物がある場合は、障害物の手前まで瞬間移動する
                    _rb.position = hit.point + Vector3.right;
                }
                else
                {
                    _rb.position += new Vector3(-5, 0, 0);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //重力(強攻撃時は空中で静止)
        if (!isAttacked)
        {
            //下キーを押した場合
            if (fallingAccel)
            {
                _rb.AddForce(new Vector3(0, 1.5f * gravity, 0));
            }
            else
            {
                _rb.AddForce(new Vector3(0, gravity, 0));
            }
        }
        //ダメージを受けた直後、強攻撃直後は操作できない
        if (!isDamaged && !isAttacked)
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
    }

    private void OnCollisionStay(Collision collision)
    {
        //地上判定,滞空時間.2段ジャンプ,強攻撃のリセット
        if (collision.gameObject.CompareTag("Field") && Mathf.Abs(_rb.velocity.y) <= 1e-4)
        {
            playerStatus = Status.GROUND;
            floatingTime = 0;
            isSecondJumpPossible = true;
            isAttack2enable = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //ダメージ時(ダメージ後の無敵時間および強攻撃中を除く)
        if ((other.CompareTag("Enemy") || other.CompareTag("Bullet")) && !isInvincible && !isAttacked)
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
        _collider.enabled = true;
        _effect2Collider.enabled = false;
    }
}
