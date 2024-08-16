using System.Collections;
using UnityEngine;

//������͑S�̓I�Ȍ��������K�v(���Ƀf�o�b�O�����ɂ���)

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
    //�v���C���[�̃A�j���[�V����(�|�[�Y)���Ǘ�
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
    //�v���C���[�̏��(�ꏊ)���Ǘ�
    private enum PositionState
    {
        GROUND = 1,
        UP = 2,
        DOWN = 3
    }
    private PositionState positionState;
    //�������ɓn���p(���U�������蔲��)
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
        //�_���[�W���󂯂�����A�U������A�N���A��͑���ł��Ȃ�
        if (!isDamaged && !isAttacked)
        {
            //�W�����v
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
            //2�i�W�����v
            if (Input.GetKeyDown(KeyCode.W) && (positionState == PositionState.DOWN || floatingTime > 0.15) && isSecondJumpPossible)
            {
                //�����]�����\
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
        //�󒆂ɏo�Ă���̎��Ԃ�ێ�
        if (positionState != PositionState.GROUND)
        {
            floatingTime += Time.deltaTime;
        }
        //�W�����v�������I��
        if (Input.GetKeyUp(KeyCode.W))
        {
            isJumped = false;
        }
        //���x�}�C�i�X���m(���Ⴊ��ł���Ƃ�(�኱�����Ă���\��������))
        if (_rb.velocity.y < -1e-4 && positionState != PositionState.DOWN && playerState != PlayerState.SQUAT)
        {
            positionState = PositionState.DOWN;
            //�U�����ɂ̓|�[�Y�͕ς��Ȃ�
            if (!isAttacked)
            {
                playerState = PlayerState.JUMPDOWN;
                playerAnimator.SetInteger("PlayerState", (int)playerState);
            }
        }
        //�_�b�V�����̕����]��(���U���̒��O�ɕ������ς���Ă��܂�Ȃ��悤��)
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
        //�܂�Ԃ��E���Ⴊ�ݏ�Ԃ���_�b�V����(���Ⴊ�ݎ��Ԃ𑪂�����2�i�W�����v�ەϐ����g�p)
        if (playerState == PlayerState.TURNBACK || (playerState == PlayerState.SQUAT && isSecondJumpPossible))
        {
            //�E�������Ă���Ƃ�
            if (_rb.rotation == Quaternion.Euler(0f, 0f, 0f))
            {
                if (_rb.velocity.x > 1e-4)
                {
                    playerState = PlayerState.DASH;
                    playerAnimator.SetInteger("PlayerState", (int)playerState);
                }
                //���Ε��������͂��ꂽ�瑦���Ɍ�����ς���
                if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                {
                    _rb.rotation = Quaternion.Euler(0f, 180f, 0f);
                    playerState = PlayerState.DASH;
                    playerAnimator.SetInteger("PlayerState", (int)playerState);
                }
            }
            //���������Ă���Ƃ�
            if (_rb.rotation == Quaternion.Euler(0f, 180f, 0f))
            {
                if (_rb.velocity.x < -1e-4)
                {
                    playerState = PlayerState.DASH;
                    playerAnimator.SetInteger("PlayerState", (int)playerState);
                }
                //���Ε��������͂��ꂽ�瑦���Ɍ�����ς���
                if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
                {
                    _rb.rotation = Quaternion.Euler(0f, 0f, 0f);
                    playerState = PlayerState.DASH;
                    playerAnimator.SetInteger("PlayerState", (int)playerState);
                }
            }
        }
        //�ҋ@���͓����n�߂������ɍ��킹�ĕ����]��(�L�[���������Е������ȂǂɑΏ����邽��) ���U�����͏�Ԃ����Z�b�g����邽�ߕ�����ς����Ȃ��悤�ɂ���
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
        //�n��ŃW�����v��Ԃ̏ꍇ�����I�ɑҋ@��Ԃ�
        if (playerState == PlayerState.JUMPDOWN && positionState == PositionState.GROUND)
        {
            playerState = PlayerState.IDLING;
            playerAnimator.SetInteger("PlayerState", (int)playerState);
        }
        //���L�[�ŗ�������
        if (positionState == PositionState.DOWN && !isAttack2)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                fallingAccel = true;
            }
        }
        //�ʏ�U��
        if (Input.GetMouseButtonDown(0) && attackPossible && !clear)
        {
            StartCoroutine(NormalAttack());
        }
        //���U��
        if (Input.GetMouseButtonDown(1) && attackPossible && attack2Count == 0)
        {
            StartCoroutine(StrongAttack());
        }
        //��������(���炩�̃~�X�ŃN���A��ɗ����Ă��������Ȃ�)
        if (_transform.position.y < deathLine�@&& !clear)
        {
            stageManager.GameOver();
        }
    }

    //�ʏ�U��(�I���u�Ԃɒ��n�����Ƃ��ɒ��n����ɂ���ԕω��Ƌ�������c�c�H)(�n��ŃW�����v�p�����������ɑҋ@�֋����I�ɖ߂�������ǉ�)
    private IEnumerator NormalAttack()
    {
        //�|�[�Y�̕ύX�A1�t���[���̊ԓ����蔻����o���A�U������ԁE�U���\��Ԃ̊Ǘ�
        playerState = PlayerState.ATTACK1;
        playerAnimator.SetInteger("PlayerState", (int)(playerState));
        normalAttackCollider.enabled = true;
        //�G�t�F�N�g�����̏�ɗ��߂�
        StartCoroutine(StopEffect(normalAttackEffect, attack1Interval));
        isAttacked = true;
        attackPossible = false;
        //���~���Ȃ牺�~���~�ߎ኱����
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
        //�n��Ȃ�_�b�V�����ҋ@��
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
        //�󒆂Ȃ牺�~��Ԃ�(�U���Ԋu����)
        else
        {
            playerState = PlayerState.JUMPDOWN;
            playerAnimator.SetInteger("PlayerState", (int)playerState);
            yield return new WaitForSeconds(attack1Interval);
        }
        attackPossible = true;
    }
    //���U��
    private IEnumerator StrongAttack()
    {
        isAttacked = true;
        isAttack2 = true;
        attackPossible = false;
        _rb.velocity = Vector3.zero;
        attack2Count++;
        //�O��̐i�s������ێ����邽�߂̕ϐ�
        int former;
        //�󒆂ł������]�����\
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
        //��t���ԓ��ɍĂѓ��͂��ꂽ��Ĕ����ł���
        while (attack2Count < attack2Possible)
        {
            //��t���ԂƓ��������̎��Ԃ͊��S�ɍd��
            yield return new WaitForSeconds(attack2reception);
            float timeCount = 0;
            bool attacked = false;
            while (timeCount < attack2reception)
            {
                yield return null;
                timeCount += Time.deltaTime;
                //8�����Ɉړ��ł���(�D��͉E�Ə�)
                if (Input.GetMouseButtonDown(1))
                {
                    //�E��
                    if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) && former != 3){
                        yield return StartCoroutine(SAExecute(3));
                        positionState = PositionState.UP;
                        playerState = PlayerState.ATTACK2;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 3;
                    }
                    //�E��
                    else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S) && former != 1)
                    {
                        yield return StartCoroutine(SAExecute(1));
                        playerState = PlayerState.ATTACK2;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 1;
                    }
                    //����
                    else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) && former != 5)
                    {
                        yield return StartCoroutine(SAExecute(5));
                        positionState = PositionState.UP;
                        playerState = PlayerState.ATTACK2;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 5;
                    }
                    //����
                    else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) && former != 7)
                    {
                        yield return StartCoroutine(SAExecute(7));
                        playerState = PlayerState.ATTACK2;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 7;
                    }
                    //�E
                    else if (Input.GetKey(KeyCode.D) && former != 2)
                    {
                        yield return StartCoroutine(SAExecute(2));
                        playerState = PlayerState.ATTACK1;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 2;
                    }
                    //��
                    else if (Input.GetKey(KeyCode.A) && former != 6)
                    {
                        yield return StartCoroutine(SAExecute(6));
                        playerState = PlayerState.ATTACK1;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 6;
                    }
                    //��
                    else if (Input.GetKey(KeyCode.W) && former != 4)
                    {
                        yield return StartCoroutine(SAExecute(4));
                        positionState = PositionState.UP;
                        playerState = PlayerState.ATTACK2;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 4;
                    }
                    //��
                    else if (Input.GetKey(KeyCode.S) && former != 0)
                    {
                        yield return StartCoroutine(SAExecute(0));
                        playerState = PlayerState.ATTACK2;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                        former = 0;
                    }
                    //�ړ��ł��Ȃ������������ꍇ���͎�t��Ԃɖ߂�
                    else
                    {
                        continue;
                    }
                    attack2Count++;
                    //�ŏI�i�܂ōU�������ꍇ�̍d��
                    if (attack2Count == attack2Possible)
                    {
                        yield return new WaitForSeconds(attack2reception);
                    }
                    attacked = true;
                    break;
                }
            }
            //��t���ɍU�����Ă��Ȃ������ꍇ�U���񐔂��c���Ă��Ă��I��
            if (!attacked)
            {
                break;
            }
        }
        //�I������
        Vector3 temp = _rb.rotation.eulerAngles;
        temp.z = 0;
        _rb.rotation = Quaternion.Euler(temp);
        //��]�ɂ��߂荞�񂾏ꍇ�ɒ��n���肪�o�Ȃ��悤��
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
    //��4m�ړ����a��(������(�I�u�W�F�N�g�̉�]���S�ɑ΂��ē����蔻�肪������)���n�ʂ��������Ƃ��ɂ߂荞�݂������ł��Ȃ����Ƃ��������Ă���)
    private IEnumerator SAExecute(int n)
    {
        int[] theta = { -90, -45, 0, 45, 90, 135, 180, -135 };
        Vector3 v = new(4 * Mathf.Cos(theta[n] * Mathf.PI / 180), 4 * Mathf.Sin(theta[n] * Mathf.PI / 180), 0);
        //�̂̌����̐؂�ւ�
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
        yield return null;              //���܂��ԉ����p
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
    //�a�������̏�Ɏc�����߂̊֐�
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
        //�d��
        if (!isAttack2)
        {
            //���L�[���������ꍇ
            if (fallingAccel)
            {
                _rb.AddForce(new Vector3(0, 1.5f * gravity, 0));
            }
            //�ʏ�
            else
            {
                _rb.AddForce(new Vector3(0, gravity, 0));
            }
        }
        //�_���[�W���󂯂�����A�U������A�N���A��͑���ł��Ȃ�
        if (!isDamaged && !isAttacked)
        {
            //�E�ړ�
            if (Input.GetKey(KeyCode.D))
            {
                if (positionState == PositionState.GROUND)
                {
                    if (_rb.velocity.x < maxSpeed)
                    {
                        _rb.AddForce(new Vector3(horizontalForce, 0, 0));
                    }
                    //���炩�̃o�O�Ń_�b�V�������Ƒ̂̌���������Ă������p
                    else if (playerState == PlayerState.DASH && _rb.rotation == Quaternion.Euler(0f, 180f, 0f))
                    {
                        _rb.rotation = Quaternion.Euler(0f, 0f, 0f);
                    }
                    //���炩�̃o�O�ōō����x�Ȃ̂Ƀ_�b�V����Ԃ���Ȃ��������p
                    else if (playerState != PlayerState.DASH)
                    {
                        playerState = PlayerState.DASH;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                    }
                }
                //�󒆂ł͐���
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
                if (positionState == PositionState.GROUND)
                {
                    if (_rb.velocity.x > -maxSpeed)
                    {
                        _rb.AddForce(new Vector3(-horizontalForce, 0, 0));
                    }
                    //���炩�̃o�O�Ń_�b�V�������Ƒ̂̌���������Ă������p
                    else if (playerState == PlayerState.DASH && _rb.rotation == Quaternion.Euler(0f, 0f, 0f))
                    {
                        _rb.rotation = Quaternion.Euler(0f, 180f, 0f);
                    }
                    //���炩�̃o�O�ōō����x�Ȃ̂Ƀ_�b�V����Ԃ���Ȃ��������p
                    else if (playerState != PlayerState.DASH)
                    {
                        playerState = PlayerState.DASH;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                    }
                }
                //�󒆂ł͐���
                else
                {
                    if (_rb.velocity.x > -maxSpeed * 0.8f)
                    {
                        _rb.AddForce(new Vector3(-horizontalForce * 0.5f, 0, 0));
                    }
                }
            }
        }
        //���C(�󒆂ł�RigidBody�̒�R������)
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
                    //���Ⴊ�ݏ�Ԃ��炷���ɂ͕ς��Ȃ��悤�ɂ���
                    if (isSecondJumpPossible)
                    {
                        playerState = PlayerState.IDLING;
                        playerAnimator.SetInteger("PlayerState", (int)playerState);
                    }
                }
            }
        }
        //�ʏ�U����͋������C��������
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

    //�_���[�W��(�_���[�W��̖��G���Ԃ���эU����������)&�R�C���l��
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
    //�_���[�W���̓_��&����s�\�△�G���Ԃ̊Ǘ�
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
    //���U�����p�̒n�`�Փ˔���
    public void FieldDitected()
    {
        if (isAttack2)
        {
            attack2Stop = true;
        }
    }
    //���n���̂��Ⴊ��(���n���_�ŉ����Ă����L�[�̕���������)(���n����͐�p��Collider����󂯎��)(��������������ʂ蔲�������������Ȃ��悤�ɉ��~���ȊO�͏Ȃ�)
    public IEnumerator Squat()
    {
        //�����������U�������Ă��Ȃ��Ƃ�(�̂���]���Ă���ƐF�X�Ƃ܂�����)
        if (positionState == PositionState.DOWN && !isAttack2)
        {
            //�U���p���̏ꍇ�̓|�[�Y��ς��Ȃ�
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
            isSecondJumpPossible = true;    //���n�シ����2��ڂ̃W�����v�����邱�Ƃ͂Ȃ��̂ł��Ⴊ�ݒ��ł��邱�Ƃ�\���ϐ��Ƃ��Ă��g����
        }
    }
    //���炩�̗��R�Œ��n���肪�ł��Ȃ��������p
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
