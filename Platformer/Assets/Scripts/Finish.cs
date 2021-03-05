using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public Sprite finSprite;
    public Main main;
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = finSprite;
            main.Win();
        }
    }


}
