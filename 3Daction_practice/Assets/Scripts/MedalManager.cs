using System.Collections;
using UnityEngine;

public class MedalManager : MonoBehaviour
{
    private Animator _animator;
    private const float getTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Sword")){
            _animator.SetBool("GetMedal", true);
            StartCoroutine(GetMedal());
        }
    }

    private IEnumerator GetMedal()
    {
        yield return new WaitForSeconds(getTime);
        Destroy(gameObject);
    }
}
