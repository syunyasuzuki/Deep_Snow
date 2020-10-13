using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ctr : MonoBehaviour
{
    float move_x;
    float pos_y;
    float pos_x;

    bool move_invert;

    Vector2 now_pos;

    Animator anima;

    Map_con mc;

    void map_date()
    {
        GameObject GM = GameObject.Find("GameMaster");
        mc = GM.GetComponent<Map_con>();

        //マップ生成
        mc.Create_map(0, 0);

        mc.Create_map(0, 1);
    }


    // Start is called before the first frame update
    void Start()
    {
        anima = GetComponent<Animator>();

        move_invert = false;

        now_pos = transform.position;
        pos_y = now_pos.y;
        pos_x = now_pos.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(pos_x + move_x, pos_y, 0.0f);
        
        if(move_invert == true)
        {
            move_x -= 1.0f * Time.deltaTime;  //敵の移動速度

            anima.SetTrigger("Enemy_Walk_Trigger");

            transform.rotation = new Quaternion(0.0f, 180.0f, 0.0f, 0.0f);   //敵の向きを反転

            if (move_x <= -3.0f)
            {
                move_invert = false;
            }
        }
        else
        {
            move_x += 1.0f * Time.deltaTime;  //敵の移動速度

            anima.SetTrigger("Enemy_Walk_Trigger");

            transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);   //敵の向きを反転

            if (move_x >= 3.0f)
            {
                move_invert = true;
            }
        }
    }
}
