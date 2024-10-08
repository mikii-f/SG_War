using System.Collections;
using UnityEngine;

public class MedalManager : MonoBehaviour
{
    private Transform _transform;
    private Collider _collider;
    private AudioSource seSource;
    [SerializeField] AudioClip seMedal;

    void Start()
    {
        _transform = GetComponent<Transform>();
        _collider = GetComponent<Collider>();
        seSource = GetComponent<AudioSource>();
        seSource.volume = GameManager.instance.SeVolume;
        //StartCoroutine(Idling());
    }
    //あまり見た目的には分からないためカット
    /*private IEnumerator Idling()
    {
        while (true)
        {
            float timeCount = 0;
            while (timeCount< 1)
            {
                timeCount += Time.deltaTime;
                Vector3 temp = _transform.position;
                temp.y -= Time.deltaTime*0.1f;
                _transform.position = temp;
                yield return null;
            }
            timeCount = 0;
            while (timeCount < 1)
            {
                timeCount += Time.deltaTime;
                Vector3 temp = _transform.position;
                temp.y += Time.deltaTime * 0.1f;
                _transform.position = temp;
                yield return null;
            }
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _collider.enabled = false;
            StartCoroutine(GetMedal());
        }
    }
    private IEnumerator GetMedal()
    {
        seSource.clip = seMedal;
        seSource.Play();
        float timeCount = 0;
        //0.2秒で3回転
        while (timeCount < 0.2f)
        {
            timeCount += Time.deltaTime;
            Vector3 temp = _transform.rotation.eulerAngles;
            Vector3 temp2 = _transform.position;
            temp.y += 3600 * Time.deltaTime;
            temp2.y += 5 * Time.deltaTime;
            _transform.SetPositionAndRotation(temp2, Quaternion.Euler(temp));
            yield return null;
        }
        _transform.rotation = Quaternion.Euler(Vector3.zero);
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }
}
