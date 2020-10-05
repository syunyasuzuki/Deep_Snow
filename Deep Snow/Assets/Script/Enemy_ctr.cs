using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ctr : MonoBehaviour
{
    float move_x;
    float pos_y;

    Animator anima;

    // Start is called before the first frame update
    void Start()
    {
        anima = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(move_x, 0.0f, 0.0f);
        anima.SetTrigger("Enemy_Walk_Trigger");
        move_x += 1.0f * Time.deltaTime;
    }
}
