using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySegment : MonoBehaviour
{
    public Transform followTarget;

    [SerializeField] private float _jointLength = 1;
    [SerializeField] private float _movementSpeed = 1;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackKnockback;
    [SerializeField] private Animator animator;
    private Vector2 lastFramePos;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        var dir = (Vector2)followTarget.position - lastFramePos;

        var vel = (Vector2)transform.position - lastFramePos;
        var angle = 90 + Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Debug.Log("Angle: " + angle);
        animator.speed = vel.magnitude / Time.deltaTime / 1700;
        Debug.Log("Speed: " + Mathf.RoundToInt(vel.magnitude / Time.deltaTime / 1700));
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (CalculateDistSqr(transform.position, followTarget.position) > _jointLength * _jointLength)
        {
            transform.position = Vector3.MoveTowards(transform.position, followTarget.position, _movementSpeed * Time.deltaTime);
        }
        lastFramePos = transform.position;

    }
    public static float CalculateDistSqr(Vector2 a, Vector2 b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.TryGetComponent<HealthManager>(out HealthManager hm))
            {
                Debug.Log("Damaged enemy: " + collision.gameObject.name);
                hm.ChangeHealth(_attackDamage);
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.gameObject.transform.position - transform.position).normalized * _attackKnockback);
            }
        }
    }
}
