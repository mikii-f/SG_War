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

    //�v���C���[�̏�Ԃ�ێ�
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
        //�_���[�W���󂯂�����͑���ł��Ȃ�
        if (!isDamaged)
        {
            //�E�ړ�
            if (Input.GetKey(KeyCode.D))
            {
                if (playerStatus == Status.GROUND)
                {
                    if (_rb.velocity.x < maxSpeed)
                    {
                        _rb.AddForce(new Vector3(horizontalForce, 0, 0));
                    }
                }
                //�󒆂̉��ړ��𐧌�
                else
                {
                    if (_rb.velocity.x < maxSpeed * 0.8f)
                    {
                        _rb.AddForce(new Vector3(horizontalForce * 0.5f, 0, 0));
                    }
                }
            }
            //���ړ�
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
            //�W�����v
            if (Input.GetKeyDown(KeyCode.W) && playerStatus == Status.GROUND)
            {
                Vector3 afterJumpVelocity = _rb.velocity;
                afterJumpVelocity.y = jumpSpeed;
                _rb.velocity = afterJumpVelocity;
                isJumped = true;
            }
            //2�i�W�����v
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
        //�󒆂ɏo�Ă���̎��Ԃ�ێ�
        if (playerStatus != Status.GROUND)
        {
            floatingTime += Time.deltaTime;
        }
        //�������ŃW�����v����
        if (isJumped)
        {
            _rb.AddForce(new Vector3(0, continueUpForce, 0));
            if (floatingTime > jumpTime)
            {
                isJumped = false;
            }
        }
        //�W�����v�������I��
        if (Input.GetKeyUp(KeyCode.W))
        {
            isJumped = false;
        }
        //�W�����v�̃L�����グ��
        if (playerStatus != Status.GROUND)
        {
            _rb.AddForce(new Vector3(0, additionalGravity, 0));
        }
        //���x�v���X���m
        if (_rb.velocity.y > 1e-4)
        {
            playerStatus = Status.UP;
        }
        //���x�}�C�i�X���m
        if (_rb.velocity.y < -1e-4)
        {
            playerStatus = Status.DOWN;
        }
        //�i�s����������
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
        //�U������2�i�W�����v���̓L�[���͂ɂ���Č�����ς���
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
        //���C�̑���
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
        //��������
        if (_transform.position.y < deathLine)
        {
            SceneManager.LoadScene("MainScene");
        }
        //���U��(�A�j���[�V�������v���C���[�ɒ��ڐݒ肷��Ƌ��������������Ȃ邽�ߎq�I�u�W�F�N�g�̂݃A�j���[�V�����œ�����)
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
        //�n�㔻��&�؋󎞊Ԃ̃��Z�b�g
        if (collision.gameObject.CompareTag("Field") && Mathf.Abs(_rb.velocity.y) <= 1e-4)
        {
            playerStatus = Status.GROUND;
            floatingTime = 0;
            isSecondJumpPossible = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //�_���[�W��(�_���[�W��̖��G���Ԃ�����)
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


    //�_���[�W���̓_��&����s�\�△�G���Ԃ̊Ǘ�
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

    //�čU���\�܂ł̃^�C�}�[
    private IEnumerator AttackFinish()
    {
        yield return new WaitForSeconds(attackTime);
        isAttacked = false;
    }
    }
