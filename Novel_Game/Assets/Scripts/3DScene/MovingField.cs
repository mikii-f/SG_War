using UnityEngine;

public class MovingField : MonoBehaviour
{
    [SerializeField] private int moveDirection = 0;
    private Transform _transform;
    private Rigidbody _rb;
    [SerializeField] private float distance = 10;
    private const float speed = 11f;
    private float moveTime;
    private Vector3 moveSpeed;
    bool move = false;
    
    void Start()
    {
        _transform = GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Foot"))
        {
            move = true;
        }
    }
}