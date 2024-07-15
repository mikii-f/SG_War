using System.Collections;
using UnityEngine;

public class MedalManager : MonoBehaviour
{
    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        StartCoroutine(Idling());
    }

    private IEnumerator Idling()
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(GetMedal());
        }
    }
    private IEnumerator GetMedal()
    {
        float timeCount = 0;
        //0.2•b‚Å3‰ñ“]
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
