using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private Transform _rotateRoot;
    [SerializeField] private float _aimSpeed = 30;
    public Vector2 currentPos => transform.position + _rotateRoot.right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (GameManager.Instance.canPlayerControl == false)
		{
			return;
		}

		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		Vector2 dir = (mousePosition - transform.position).normalized;


        var newRot = Quaternion.Euler(0,0,Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        _rotateRoot.rotation = Quaternion.Lerp(_rotateRoot.rotation, newRot, Time.deltaTime * _aimSpeed);

	}
}
