using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class DoorHingeHelper : MonoBehaviour
{
    private Vector2 _initialPos;
    private float lastYPosition;
    private Rigidbody2D rb2D;
    private MMF_Player mmf;
    private bool isLocked=true;

    private void Awake()
    {
        _initialPos = transform.position;
        rb2D = GetComponent<Rigidbody2D>();
        mmf = GetComponent<MMF_Player>();
    }
    private void FixedUpdate()
    {
        if (isLocked & rb2D.velocity.magnitude > 0.02f) { isLocked = false; mmf.PlayFeedbacks(); }
        if (!isLocked&&transform.localPosition.y * lastYPosition < 0) { rb2D.velocity = Vector2.zero; rb2D.angularVelocity = 0; isLocked = true; mmf.PlayFeedbacks(); }
        rb2D.mass = isLocked ? 200 : 50;
        lastYPosition = transform.localPosition.y;
    }
    private void LateUpdate()
    {
    }
}
