using UnityEngine;

//強攻撃時の地形衝突判定

public class FieldDitect : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MainField"))
        {
            playerManager.FieldDitected();
        }
    }
}