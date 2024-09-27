using UnityEngine;

//着地判定及び浮き床のため(地形に横から当たった場合に着地判定が出ないように足の幅を少し狭くした)

public class PlayerFootManager : MonoBehaviour
{
    public static PlayerFootManager instance;
    [SerializeField] PlayerManager playerManager;
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainField") || other.gameObject.CompareTag("FloatingField"))
        {
            StartCoroutine(playerManager.Squat());
        }
        if (other.gameObject.CompareTag("UpField"))
        {
            playerManager.OnUpField = true;
        }
        if (other.gameObject.CompareTag("LRField"))
        {
            playerManager.OnLRField = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MainField") || other.gameObject.CompareTag("FloatingField"))
        {
            playerManager.SetGround();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("UpField"))
        {
            playerManager.OnUpField = false;
        }
        if (other.gameObject.CompareTag("LRField"))
        {
            playerManager.OnLRField = false;
        }
    }
}