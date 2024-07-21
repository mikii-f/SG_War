using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class StageManagerOrigin : MonoBehaviour
{
    [SerializeField] protected PlayerManager playerManager;
    [SerializeField] private TMP_Text medalCountText;
    protected int medalCount = 0;
    [SerializeField] protected TMP_Text timeText;
    protected float time = 0;
    [SerializeField] private TMP_Text lifeText;
    private int life = 3;
    protected bool clear = false;

    public void GotMedal()
    {
        medalCount++;
        medalCountText.text = medalCount.ToString();
    }
    public void Damage()
    {
        //スキップ含めクリア済みなら影響なし
        if (!clear)
        {
            life--;
            lifeText.text = "Life × " + life.ToString();
            if (life == 0)
            {
                GameOver();
            }
        }
    }
    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
