using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class airPatrol : MonoBehaviour
{
    public Transform Point1;
    public Transform Point2;
    public float speed = 2f;
    public float waitTime = 2f;
    bool canGo = true;


    void Start()
    {
        gameObject.transform.position = new Vector3(Point1.position.x, Point1.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(canGo)
        transform.position = Vector3.MoveTowards(transform.position, Point1.position, speed * Time.deltaTime);

        if(transform.position == Point1.position)
        {
            Transform t = Point1;
            Point1 = Point2;
            Point2 = t;
            canGo = false;
            StartCoroutine(Waiting());
        }
    }



    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(waitTime);
        if(transform.rotation.y == 0)
        transform.eulerAngles = new Vector3(0,  180, 0);
        else
        transform.eulerAngles = new Vector3(0, 0, 0);

        canGo = true;
    }





}
