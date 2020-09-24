using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud_ctr : MonoBehaviour
{
    GameObject[] clouds;

    float cloudmove_x = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(Random.Range(-5.0f, 5.0f), Random.Range(-3.0f, 3.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
