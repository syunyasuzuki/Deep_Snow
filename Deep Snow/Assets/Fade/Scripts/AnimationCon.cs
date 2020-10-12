using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCon : MonoBehaviour
{   
    Animator animator;
    bool animationflag = true;
    void Start()
    {
       animator = GetComponent<Animator>();     
    }

    // Update is called once per frame
    void Update()
    {
        float m_x = Input.GetAxisRaw("Horizontal");
        if (animationflag == true)
        {
            if (m_x > 0)
            {
                animator.SetBool("walk", true);
                if (Input.GetMouseButtonDown(0))  animator.SetTrigger("AbilityTrigger");
            }
            else if (m_x < 0)
            {
                animator.SetBool("walk", true);
                if (Input.GetMouseButtonDown(0)) animator.SetTrigger("AbilityTrigger");
            }
            else animator.SetBool("walk", false);
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                animator.SetTrigger("jumpTrigger");
            }            
            if (Input.GetMouseButton(0))                       
                animationflag = false;
            
            //animator.SetTrigger("DieTrigger");
        }
        if (Input.GetMouseButtonDown(0))
            animator.SetTrigger("AbilityTrigger");
        if (animationflag == false)
        {
            if (Input.GetMouseButtonUp(0))
                animationflag = true;           
        }
        //Debug.Log(animationflag);
    }

}
