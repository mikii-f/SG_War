using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1Manager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("3DGameSelectScene");
        }
    }
}
