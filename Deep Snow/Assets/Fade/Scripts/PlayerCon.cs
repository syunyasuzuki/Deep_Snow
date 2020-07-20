using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    enum MotionIndex
    {
        Stay,
        Walk,
        jump,
    }
    MotionIndex m_motionIndex = MotionIndex.Stay;

    private Rigidbody2D rigid2D;
    Animator animator;
    public float speed; 

    // Use this for initialization
    void Start()
    { 
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        
    }
    // Update is called once per frame
    void Update()
    {
        m_motionIndex = MotionIndex.Stay;
        switch (m_motionIndex)
        {
            case MotionIndex.Walk:
            case MotionIndex.jump:

                break;
        }

        animator.SetInteger("MotionIndex", (int)MotionIndex.Stay);
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * speed * Time.deltaTime;
            animator.SetInteger("MontionIndex", (int)MotionIndex.Walk);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * speed * Time.deltaTime;
            animator.SetInteger("MontionIndex", (int)MotionIndex.Walk);
        }
    }
}

