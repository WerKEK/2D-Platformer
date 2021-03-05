using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B2M : MonoBehaviour
{
    public GameObject[] block;
    public Sprite btnDown;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "MarkBox")
        {
            GetComponent<SpriteRenderer>().sprite = btnDown;
            GetComponent<CircleCollider2D>().enabled = false;
             
            foreach(GameObject obj in block)
            {
                Destroy(obj);
            }
        }
    }

}
