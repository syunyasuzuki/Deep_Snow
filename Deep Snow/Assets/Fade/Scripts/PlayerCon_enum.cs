using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon_enum : MonoBehaviour
{
    enum MotionIndex 
    {
        Wait = 0,
        Walk = 1,
        jump = 2,
    }
    

    private Rigidbody2D rigid2D;
    Animator animator;
    public float speed;
    int key = 0;

    // Use this for initialization
    void Start()
    { 
        rigid2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
       
        
    }
    // Update is called once per frame
    void Update()
    {
        // m_motionIndex = MotionIndex.Stay;
        //switch (m_motionIndex)
        //{
        //    case MotionIndex.Walk:
        //    case MotionIndex.jump:

        //        break;
        //}
      
        if (Input.GetKey(KeyCode.D))
        {
            animator.SetInteger("MontionIndex", (int)MotionIndex.Walk);
            transform.position += transform.right * speed * Time.deltaTime;                    
            key = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetInteger("MontionIndex", (int)MotionIndex.Walk);
            transform.position -= transform.right * speed * Time.deltaTime;
            key = -1;
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            animator.SetInteger("MotionIndex", (int)MotionIndex.Wait);
        }
        else if (Input.GetKey(KeyCode.W))
        {         
              animator.SetInteger("MotoionIndex", (int)MotionIndex.jump);
        }
        if(key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }
        
    }
}

