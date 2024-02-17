using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayDoor : MonoBehaviour
{
    private Transform door;
    //private HingeJoint2D hj;

    private Coroutine coroutine;

    private MiscTextManager miscText;
    
    // Start is called before the first frame update
    void Start()
    {
        /*
        \hj = transform.parent.GetChild(0).gameObject.GetComponent<HingeJoint2D>();

        JointAngleLimits2D limits=hj.limits;
        limits.min = -89f;
        limits.max = 0f;
        hj.limits = limits;
        */

        door = transform.parent;

        miscText = GameObject.FindGameObjectWithTag("Misc Text").GetComponent<MiscTextManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(door.rotation.z);
        if ((door.rotation.z >= 0.2f || door.rotation.z<=-0.2f))
        {
            coroutine=StartCoroutine(CloseDoor());
        }
    }

    private IEnumerator CloseDoor()
    {
        Debug.Log("started");
        yield return new WaitForSeconds(4f);
        Debug.Log("e");
        door.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //TODO UI indication
            Debug.Log("Door cannot be opened from this side!");
            miscText.UpdateText("Door cannot be opened from this side!");
        }
    }
}
