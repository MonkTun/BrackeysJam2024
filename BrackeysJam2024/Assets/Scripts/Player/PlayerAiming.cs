using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    [SerializeField] private Transform _rotateRoot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		Vector2 dir = (mousePosition - transform.position).normalized;


        _rotateRoot.rotation = Quaternion.Euler(0,0,Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

	}
}
