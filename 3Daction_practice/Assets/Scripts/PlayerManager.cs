using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerManager : MonoBehaviour
{
    private Rigidbody _rb;
    private Transform _transform;
    private Collider _collider;
    private MeshRenderer _meshRenderer;
    public GameObject effect2;      //�{�Ԃł͕ʂ̎������@��ڎw��
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
        Application.targetFrameRate = 60;           //60FPS�Œ�
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
        //�_���[�W���󂯂�����A���U������͑���ł��Ȃ�
        if (!isDamaged && !isAttacked)
        {
            //�W�����v
            if (Input.GetKeyDown(KeyCode.W) && playerStatus == Status.GROUND)
            {
                Vector3 afterJumpVelocity = _rb.velocity;
                afterJumpVelocity.y = jumpSpeed;
                _rb.velocity = afterJumpVelocity;
                isJumped = true;
                fallingAccel = false;
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
                fallingAccel = false;
            }
        }
        //�󒆂ɏo�Ă���̎��Ԃ�ێ�
        if (playerStatus != Status.GROUND)
        {
            floatingTime += Time.deltaTime;
        }
        //�W�����v�������I��
        if (Input.GetKeyUp(KeyCode.W))
        {
            isJumped = false;
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
        //���L�[�ŗ�������
        if (playerStatus == Status.DOWN)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                fallingAccel = true;
            }
        }
        //�U������2�i�W�����v���̓L�[���͂ɂ���Č�����ς���
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
        //��������
        if (_transform.position.y < deathLine)
        {
            SceneManager.LoadScene("MainScene");
        }
        //���U��(�A�j���[�V�������v���C���[�ɒ��ڐݒ肷��Ƌ��������������Ȃ邽�ߎq�I�u�W�F�N�g�̂݃A�j���[�V�����œ�����)
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

            //��Q�����m�p(��~�ʒu��ړ������͓K�X�킩��₷�����O�̕ϐ��ɒu������)
            RaycastHit hit;

            //�ړ������̓L�[���͗D��A���_�Ō��݂̌���
            if (Input.GetKey(KeyCode.D))
            {
                if (Physics.Raycast(_transform.position, Vector3.right, out hit, 5f, mainField))
                {
                    // ��Q��������ꍇ�́A��Q���̎�O�܂ŏu�Ԉړ�����
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
                    // ��Q��������ꍇ�́A��Q���̎�O�܂ŏu�Ԉړ�����
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
                    // ��Q��������ꍇ�́A��Q���̎�O�܂ŏu�Ԉړ�����
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
                    // ��Q��������ꍇ�́A��Q���̎�O�܂ŏu�Ԉړ�����
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
        //�d��(���U�����͋󒆂ŐÎ~)
        if (!isAttacked)
        {
            //���L�[���������ꍇ
            if (fallingAccel)
            {
                _rb.AddForce(new Vector3(0, 1.5f * gravity, 0));
            }
            else
            {
                _rb.AddForce(new Vector3(0, gravity, 0));
            }
        }
        //�_���[�W���󂯂�����A���U������͑���ł��Ȃ�
        if (!isDamaged && !isAttacked)
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
            //�������ŃW�����v����
            if (isJumped)
            {
                _rb.AddForce(new Vector3(0, continueUpForce, 0));
                if (floatingTime > jumpTime)
                {
                    isJumped = false;
                }
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
    }

    private void OnCollisionStay(Collision collision)
    {
        //�n�㔻��,�؋󎞊�.2�i�W�����v,���U���̃��Z�b�g
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
        //�_���[�W��(�_���[�W��̖��G���Ԃ���ы��U����������)
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
        _collider.enabled = true;
        _effect2Collider.enabled = false;
    }
}
