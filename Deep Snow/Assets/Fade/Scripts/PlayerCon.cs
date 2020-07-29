using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    // Start is called before the first frame update   
    Rigidbody2D rig2D;
    Animator animator;
    public float Jump;
    public float walk;
    float maxspeed = 2.0f;

    void Start()
    {
        this.rig2D = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && this.rig2D.velocity.y == 0)
        {
            this.rig2D.AddForce(transform.up * this.Jump);
        }
        //左右移動
        int key = 0;
        if (Input.GetKey(KeyCode.A))//左
        {
            key = -1;
        }
        else if (Input.GetKey(KeyCode.D))//右
        {
            key = 1;
        }
        if (Input.GetKeyUp(KeyCode.A)||Input.GetKeyUp(KeyCode.D))
        {
            rig2D.velocity = Vector3.zero;          
        }

        //プレイヤー速度
        float speedx = Mathf.Abs(this.rig2D.velocity.x);

        //スピード制限
        if(speedx < this.maxspeed)
        {
            this.rig2D.AddForce(transform.right * key * this.walk);
        }
        //反転
        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }
        this.animator.speed = speedx / 2.0f;
    }

}
