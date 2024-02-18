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
    private HingeJoint2D joint;

    private float _jointInitialRot;

    private bool _barricaded;

    //[SerializeField] private Transform _actualDoor;

    private void Awake()
    {
        _initialPos = transform.localPosition;
        rb2D = GetComponent<Rigidbody2D>();
        mmf = GetComponent<MMF_Player>();
		joint = GetComponent<HingeJoint2D>();
        _jointInitialRot = joint.jointAngle;
	}
    private void FixedUpdate() 
    {
        if (isLocked & rb2D.velocity.magnitude > 0.02f) { isLocked = false; mmf.PlayFeedbacks(); }
        if (!isLocked&&transform.localPosition.y * lastYPosition < 0) { rb2D.velocity = Vector2.zero; rb2D.angularVelocity = 0; isLocked = true; mmf.PlayFeedbacks(); }
        if (_barricaded == false) rb2D.mass = isLocked ? 200 : 50;
        lastYPosition = transform.localPosition.y;
    }

    public void LockDoor()
    {
		transform.localPosition = _initialPos;
		transform.localRotation = Quaternion.Euler(0, 0, _jointInitialRot);
		rb2D.velocity = Vector2.zero;
		rb2D.angularVelocity = 0;
        _barricaded = true;
        rb2D.mass = 1000;
		//joint.enabled = false;

		//isLocked = true;
		//transform.localPosition = _initialPos;
		//joint. = jointinitla; 
		//rb2D.velocity = Vector2.zero;
	}

    private void LateUpdate()
    {
    }
}
