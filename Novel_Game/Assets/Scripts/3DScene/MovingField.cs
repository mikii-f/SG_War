using System.Collections;
using UnityEngine;

public class MovingField : MonoBehaviour
{
    [SerializeField] private int moveDirection = 0;
    private Transform _transform;
    private Rigidbody _rb;
    private GameObject block;
    private Transform blockTransform;
    [SerializeField] private float distance = 10;
    private const float speed = 11f;
    private float moveTime;
    private Vector3 moveSpeed;
    bool move = false;
    bool blocked = false;
    
    void Start()
    {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        block = _transform.GetChild(0).gameObject;
        blockTransform = block.GetComponent<Transform>();
        moveTime = distance / speed;
        if (moveDirection == 0)
        {
            moveSpeed = new(0, speed, 0);
        }
        else if (moveDirection == 1)
        {
            moveSpeed = new(speed, 0, 0);
        }
        else
        {
            moveSpeed = new(-speed, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        if (move && moveTime > 0)
        {
            moveTime -= Time.deltaTime;
            _rb.MovePosition(_transform.position + moveSpeed * Time.deltaTime);
        }
        else if (!blocked && moveTime <= 0)
        {
            blocked = true;
            StartCoroutine(Block());
        }
    }

    //ˆÚ“®I—¹Œã‚Í–€ŽC0°‚ð’†‚©‚ç‰Ÿ‚µo‚·
    private IEnumerator Block()
    {
        Vector3 temp = blockTransform.localPosition;
        while (temp.y < 0.69)
        {
            temp = blockTransform.localPosition;
            temp.y += 0.022f;
            blockTransform.localPosition = temp;
            yield return null;
        }
        blockTransform.localPosition = new(0, 0.695f, 0);
        blockTransform.localScale = new(1.006f, 0.02f, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Foot"))
        {
            move = true;
        }
    }
}