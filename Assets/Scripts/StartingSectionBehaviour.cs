using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingSectionBehaviour : MonoBehaviour
{
    private SectionPool pool;

    private float speed = 20f;

    private void Start()
    {
        pool = FindObjectOfType<SectionPool>();
    }

    private void Update()
    {
        if (transform.position.z <= -30)
        {
            //pool.ActivateARandomSection(new Vector3(0, 0, 60));
            Destroy(gameObject);
        }
        
        Move();
        
    }

    private void Move()
    {
        transform.Translate(Vector3.back * Time.deltaTime * speed);
    }
}
