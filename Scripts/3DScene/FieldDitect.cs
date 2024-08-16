using UnityEngine;

//‹­UŒ‚‚Ì’nŒ`Õ“Ë”»’è

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