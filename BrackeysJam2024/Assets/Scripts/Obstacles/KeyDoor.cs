using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : MonoBehaviour
{
    private MiscTextManager miscText;
    private Rigidbody2D rb;
    private bool isUnlocked;
    // Start is called before the first frame update
    void Start()
    {
        miscText = GameObject.FindGameObjectWithTag("Misc Text").GetComponent<MiscTextManager>();
        rb = GetComponent<Rigidbody2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isUnlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInventory pi= collision.gameObject.GetComponent<PlayerInventory>();
            if (pi.CheckItemFromInventory(pi.key, 1))
            {
                Debug.Log("unlocked");
                if (!isUnlocked)
                {
                    miscText.UpdateText("Door unlocked!");
                }
                isUnlocked = true;
                rb.constraints &= ~RigidbodyConstraints2D.FreezeRotation; //ChatGPT gave me this syntax idk what it means;
            }
            else
            {
                Debug.Log("Key required!");
                miscText.UpdateText("Key required!");
            }
        }
    }
}
