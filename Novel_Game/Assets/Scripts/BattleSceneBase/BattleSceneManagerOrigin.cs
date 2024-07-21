using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class BattleSceneManagerOrigin : MonoBehaviour
{
    [SerializeField] protected SainManager sainManager;
    [SerializeField] protected LeaderManager leaderManager;
    [SerializeField] protected BattleSystemManager battleSystemManager;
    [SerializeField] protected GameObject blackObject;
    protected RectTransform blackRect;
    protected Image blackImage;
    [SerializeField] protected GameObject explanation;
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
    protected bool isSpecialAttack = false;
    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        blackRect = blackObject.GetComponent<RectTransform>();
        blackImage = blackObject.GetComponent<Image>();
        specialSkillAnimation.SetActive(false);
        explanation.SetActive(false);
        StartCoroutine(FadeIn(1, blackImage));
        StartSet();
    }
    //�e�V�[���ł̏���������
    protected abstract void StartSet();

    // Update is called once per frame
    void Update()
    {
        //���������Ȃǂɂ��o�O���Ȃ������߂ɂ͑S��if-else�Ōq���ׂ��Ȃ̂��H���̏ꍇ�ǂ�̗D��x����������H�ʃX�N���v�g�́H
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
            }
            //���(�E�[���獶�[�A�^�񒆂̓G������ł���ꍇ)�Ɉړ�
            else if (selectedEnemy == 2 && !deadEnemyComposition[numberOfCurrentWave][0])
            {
                enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
                selectedEnemy = 0;
                enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
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
            }
            //��E�Ɉړ�
            else if (selectedEnemy == 0 && !deadEnemyComposition[numberOfCurrentWave][numberOfEnemy[numberOfCurrentWave] - 1])
            {
                enemyComposition[numberOfCurrentWave][selectedEnemy].DisSelect();
                selectedEnemy = numberOfEnemy[numberOfCurrentWave] - 1;
                enemyComposition[numberOfCurrentWave][selectedEnemy].Select();
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

    //�G����̍U��
    public void EnemyToSainAttack(int damage)
    {
        sainManager.ReceiveDamage(damage);
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
        attackImage.color = Color.white;
        float diffX = AttackPoint();
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
        enemyComposition[numberOfCurrentWave][selectedEnemy].ReceiveDamage(damage);
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
        //������
        attackRect.anchoredPosition = new(0, 0);
        attackRect.localScale = new Vector2(1, 1);
    }
    //�퓬�X�L��2
    public IEnumerator SainSkill2(int damage, RectTransform attackRect, Image attackImage)
    {
        attackImage.color = Color.white;
        float diffX = AttackPoint();
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
        enemyComposition[numberOfCurrentWave][selectedEnemy].ReceiveDamage(damage);
        enemyComposition[numberOfCurrentWave][selectedEnemy].ReceiveDelay();
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
        yield return new WaitForSeconds(2.5f);
        specialSkillAnimation.SetActive(false);
        for (int i=0; i < numberOfEnemy[numberOfCurrentWave]; i++)
        {
            if (!deadEnemyComposition[numberOfCurrentWave][i])
            {
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

    //�퓬�J�n���E�I�����̃t�F�[�h
    protected IEnumerator FadeIn(float fadeTime, Image image)
    {
        float waitTime = 0.1f;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 255.0f; alpha >= 0f; alpha -= alphaChangeAmount)
        {
            Color newColor = image.color;
            newColor.a = alpha / 255.0f;
            image.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
    }
    protected IEnumerator FadeOut(float fadeTime, Image image)
    {
        float waitTime = 0.1f;
        float alphaChangeAmount = 255.0f / (fadeTime / waitTime);
        for (float alpha = 0.0f; alpha <= 255.0f; alpha += alphaChangeAmount)
        {
            Color newColor = image.color;
            newColor.a = alpha / 255.0f;
            image.color = newColor;
            yield return new WaitForSeconds(waitTime);
        }
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
        yield return new WaitForSeconds(2);
        yield return StartCoroutine(FadeOut(2, blackImage));
        SceneLoad();
    }
    //�퓬�J�n���̐����N���[�Y
    public void Close()
    {
        explanation.SetActive(false);
    }
    //�e�V�[���ɂ����郍�[�h&�����X�L�b�v�p
    public abstract void SceneLoad();
}