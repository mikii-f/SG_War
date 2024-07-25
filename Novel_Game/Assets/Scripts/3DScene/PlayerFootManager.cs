using UnityEngine;

//���n����y�ѕ������̂���(�n�`�ɉ����瓖�������ꍇ�ɒ��n���肪�o�Ȃ��悤�ɑ��̕���������������)

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
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MainField") || other.gameObject.CompareTag("FloatingField"))
        {
            playerManager.SetGround();
        }
    }
}