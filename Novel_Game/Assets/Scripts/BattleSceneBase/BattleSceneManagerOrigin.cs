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
    [SerializeField] private Text expMessage;
    [SerializeField] private RectTransform expSwitchRect;
    [SerializeField] protected TMP_Text battleStartAndFinishText;
    [SerializeField] private GameObject specialSkillAnimation;
    [SerializeField] private Text enemyNumberText;
    [SerializeField] private Text waveNumberText;
    protected int[] numberOfEnemy;
    protected int numberOfWave;
    protected EnemyManagerOrigin[][] enemyComposition;
    protected bool[][] deadEnemyComposition;
    protected int numberOfArriveEnemy;
    protected int numberOfCurrentWave = 0;      //�z��̃C���f�b�N�X�Ƃ��Ďg�����Ƃ���������0����
    protected int selectedEnemy = 0;
    private bool isSainAttack = false;
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
    //�e�V�[���ł̏���������
    protected abstract void StartSet();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && explanation.activeSelf)
        {
            Close();
        }
        //�U���Ώۂ̑I��
        if (Input.GetKeyDown(KeyCode.A))
        {
            EnemySelectA();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            EnemySelectD();
        }
    }
    //�U���Ώۂ̑I��(�G�̐���|���ꂽ�G�ɏ_��ɑΉ�)
    private void EnemySelectA()
    {
        if (selectedEnemy != 0)
        {
            //����Ɉړ�
            if (!deadEnemyComposition[numberOfCurrentWave][selectedEnemy - 1])
            {
                enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
                selectedEnemy--;
                enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
                seSource.clip = seUIClick;
                seSource.Play();
            }
            //���(�E�[���獶�[�A�^�񒆂̓G������ł���ꍇ)�Ɉړ�
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
            //��E�Ɉړ�
            if (!deadEnemyComposition[numberOfCurrentWave][selectedEnemy + 1])
            {
                enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
                selectedEnemy++;
                enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
                seSource.clip = seUIClick;
                seSource.Play();
            }
            //��E�Ɉړ�
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
    //�G���|�ꂽ���̎����؊���
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
    //�G�̉摜(�����ɂ͕ʃI�u�W�F�N�g)���N���b�N�����Ƃ��̑Ώې؂�ւ�
    public void EnemySelect0()
    {
        Select(0);
    }
    public void EnemySelect1()
    {
        Select(1);
    }
    public void EnemySelect2()
    {
        Select(2);
    }
    private void Select(int n)
    {
        if (selectedEnemy != n && !deadEnemyComposition[numberOfCurrentWave][n])
        {
            enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
            selectedEnemy = n;
            enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
            seSource.clip = seUIClick;
            seSource.Play();
        }
    }

    //�G����̍U��
    public void EnemyToSainAttack(int damage)
    {
        sainManager.ReceiveDamage(damage);
    }
    //�G����̕K�E�Z(�K�[�h�E���s��)
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
        seSource.Play();                        //������SE�𓯎��ɖ炷����
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
        //0.25�b��400�ړ�
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
    //�G�ւ̍U���̒��e�n�_�擾
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
    //�퓬�X�L��1
    public IEnumerator SainSkill1(int damage, RectTransform attackRect, Image attackImage)
    {
        isSainAttack = true;
        seSource.clip = seSword;
        seSource.Play();
        attackImage.color = Color.white;
        float diffX = AttackPoint();
        int selected = selectedEnemy;       //�U����I���������_�ł̍U���Ώۂ�ێ�
        while (true)
        {
            Vector2 temp = attackRect.anchoredPosition;
            Vector2 temp2 = attackRect.localScale;
            //0.2�b�ō��W(x,0)��
            temp.x += diffX * Time.deltaTime * 5;
            //0.2�b��1/2�{�̑傫����
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
        //���e�E����
        enemyComposition[numberOfCurrentWave][selected].ReceiveDamage(damage);
        isSainAttack = false;
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeIn(0.5f, attackImage));
        //������
        attackRect.anchoredPosition = new(0, 0);
        attackRect.localScale = new Vector2(1, 1);
    }
    //�퓬�X�L��2
    public IEnumerator SainSkill2(int damage, RectTransform attackRect, Image attackImage)
    {
        isSainAttack = true;
        seSource.clip = seWind;
        seSource.Play();
        attackImage.color = Color.white;
        float diffX = AttackPoint();
        int selected = selectedEnemy;       //�U����I���������_�ł̍U���Ώۂ�ێ�
        while (attackRect.localScale.x > 0.5f)
        {
            Vector2 temp = attackRect.anchoredPosition;
            Vector2 temp2 = attackRect.localScale;
            Vector3 temp3 = attackRect.localEulerAngles; 
            //0.2�b�ō��W(x,0)��
            temp.x += diffX * Time.deltaTime * 5;
            //0.2�b��1/2�{�̑傫����
            temp2.x -= 0.5f * Time.deltaTime * 5;
            temp2.y -= 0.5f * Time.deltaTime * 5;
            //0.5�b��2��]
            temp3.z += 720 * Time.deltaTime * 5;
            attackRect.anchoredPosition = temp;
            attackRect.localScale = temp2;
            attackRect.localEulerAngles = temp3;
            yield return null;
        }
        //���e�E����
        enemyComposition[numberOfCurrentWave][selected].ReceiveDamage(damage);
        enemyComposition[numberOfCurrentWave][selected].ReceiveDelay();
        isSainAttack = false;
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeIn(0.5f, attackImage));
        //������
        attackRect.anchoredPosition = new(0, 0);
        attackRect.localScale = new Vector2(1, 1);
    }

    //�K�E�Z�ł̍U��
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
                //���G�̃o���A�j��
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
    //�G�����񂾂��Ƃ̎󂯎��
    public void EnemyDied()
    {
        //���ł����������΍�
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
            //�G��|���Ă��炱�̔��������܂łɃ��O�����遨�C�� ���̊ԂɃ^�[�Q�b�g���ړ������Ă�����I�ђ����K�v�͂Ȃ�
            else if (deadEnemyComposition[numberOfCurrentWave][selectedEnemy])
            {
                EnemySelect();
            }
        }
    }
    //�����Ă���G�̊Ǘ�
    private void ArriveEnemyCheck()
    {
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            deadEnemyComposition[numberOfCurrentWave][i] = enemyComposition[numberOfCurrentWave][i].Dead;
        }
    }
    //�����^�ɂ�鑝�B(�Đ�)����
    public void ReviveEnemy()
    {
        for (int i = 0; i< numberOfEnemy[numberOfCurrentWave]; i++)
        {
            //���񂾉����ٗd���������̕���
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

    //�E�F�[�u�J��
    private IEnumerator GoToNextWave()
    {
        //�K�E�Z�ɂ���ēG��S���|�����ꍇ��Pause��������������̂�����邽��
        yield return new WaitUntil(() => !isSpecialAttack);
        sainManager.Pause = true;
        leaderManager.Pause = true;
        battleSystemManager.MenuOff();
        yield return new WaitForSeconds(2);
        StartCoroutine(Wipe());
        yield return new WaitForSeconds(0.5f);//���C�v�ɂ��2�b�ԈÓ]
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

    //�E�F�[�u�J�ڎ��̃��C�v(0.5+2+0.5�b)
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

    //�Q�[���I�[�o�[����
    public IEnumerator GameOver()
    {
        //�K�E�Z�ɂ���ē|���ꂽ�ꍇ��Pause��������������̂�����邽��
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
    //����&�V�[���J��
    private IEnumerator Win()
    {
        //�K�E�Z�ɂ���ēG��S���|�����ꍇ��Pause��������������̂�����邽��
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
    //�퓬�J�n���̐����N���[�Y
    public void Close()
    {
        explanation.SetActive(false);
        battleSystemManager.IsMessageDisplay = false;
        seSource.clip = seUIUnactive;
        seSource.Play();
    }
    //�|�[�Y(���j���[�\��)���\��(�G���������U�����łȂ���)
    public bool IsPausePossible()
    {
        bool possible = true;
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            if (enemyComposition[numberOfCurrentWave][i].IsAttack)
            {
                possible = false;
                break;
            }
        }
        return possible && !isSainAttack && !isSpecialAttack && !isEnemySpecialAttack && !sainManager.Pause;
    }
    public void Pause()
    {
        sainManager.Pause = true;
        leaderManager.Pause = true;
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            enemyComposition[numberOfCurrentWave][i].Pause = true;
        }
    }
    public void Restart()
    {
        sainManager.Pause = false;
        leaderManager.Pause = false;
        for (int i = 0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            enemyComposition[numberOfCurrentWave][i].Pause = false;
        }
    }
    //�����̍ĕ\��
    public void Explanation()
    {
        StartCoroutine(ButtonAnim(expSwitchRect));
        seSource.clip = seUIClick;
        seSource.Play();
        expMessage.text = "�g�O���N���b�N�A�܂��̓X�y�[�X�L�[�ŕ���";
        explanation.SetActive(true);
        battleSystemManager.IsMessageDisplay = true;
    }
    //�e�V�[���ɂ����郍�[�h&�����X�L�b�v�p
    public abstract void SceneLoad();
}