using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo_player2 : MonoBehaviour
{

    const int Mapsize_x = 10;
    const int Mapsize_y = 6;
    public GameObject player;//Player
    bool Abilityswitch = true;//能力onの判定
    Animator animator;   
    float alpha =1;
    void Set_camera()
    {
        Camera cam = Camera.main;
        cam.transform.position = new Vector3((Mapsize_x - 1) / 2.0f, -(Mapsize_y - 1) / 2.0f, -10.0f);
    }
    private int[,] mapdata = new int[Mapsize_y, Mapsize_x]{
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0},
        {0,0,1,0,0,0,0,0,0,1},
        {0,1,1,1,0,1,0,0,1,1}
    };
    void Set_map()
    {
        GameObject mapm = new GameObject("mapm");
        for (int lu = 0; lu < Mapsize_y; ++lu)
        {
            for (int na = 0; na < Mapsize_x; ++na)
            {
                if (mapdata[lu, na] == 1)
                {
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.transform.position = new Vector3(na, -lu, 0.0f);
                    go.transform.parent = mapm.transform;
                    Destroy(go.GetComponent<BoxCollider>());
                }
            }
        }
    }
    int Read_chip(int x, int y)
    {
        if (x < 0 || x >= Mapsize_x || y < 0 || y >= Mapsize_y) { return 1; }
        return mapdata[y, x];
    }
    const int spike_num2 = 3;
    public int Read_spike()
    {
        return spike_num2;
    }
    Vector3[] spike_pos2 = new Vector3[spike_num2]
    {
        new Vector3(0.0f,0.0f,0.0f),
        new Vector3(0.0f,0.0f,0.0f),
        new Vector3(0.0f,0.0f,0.0f)
    };
     
  public Vector3 Read_spike_pos(int x)
    {
        return spike_pos2[x];
    }
     int spike_num;
     Vector3[] spike_pos;
    void Set_spikes()
    {
        spike_num = Read_spike();
        spike_pos = new Vector3[spike_num];
        for (int i = 0; i < spike_num; ++i)
        {
            spike_pos[i] = Read_spike_pos(i);
        }
    }
    
    void Spike_task()
    {
        Vector3 p_pos = transform.position;
        for(int i = 0; i < spike_num; ++i)
        {
            if(Mathf.Abs(p_pos.x - spike_pos[i].x) > 0.5f && Mathf.Abs(p_pos.y - spike_pos[i].y) < 0.5f)
            {
                //死亡処理               
                animator.SetTrigger("DieTrigger");
                gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, alpha);
                alpha -= 1.0f * Time.deltaTime;
                if (alpha <= 0) {  
                player.transform.position = new Vector3(0,0,0);
                    alpha = 1;
                    }
            }
            Vector3 s_pos = spike_pos[i] - p_pos;
        }
    }

    //プレイヤーの大きさ、初期位置
    Vector3 Player_size = new Vector3(0.8f, 0.8f, 0.8f);
    Vector2 Player_pos = new Vector3(0.0f, 0.0f, 0.0f);
    float herf_y = 0.4f;//プレイヤーの大きさの半分
    //落下orジャンプ
    int now_fase = 0;//0 = f ,1 = j
    //落下関連
    const float Fall_spd = 2.0f / 60;//落下速度
    int fall_count = 0;//落下してる時間
    //ジャンプ関連
     float Jump_spd = 8.0f / 60;//ジャンプ速度
     int Max_jump_count = 15;//ジャンプできる時間
    int jump_count = 0;//ジャンプしてる時間
    bool jump_set = false;//ジャンプできるか
    //移動関連
    float Move_spd = 2.0f / 60;//移動速度
  

   
    void Set_player()
    {
        player.transform.localScale = Player_size;
        player.transform.position = Player_pos;
        int n_x = Mathf.FloorToInt(Player_pos.x + 0.5f), n_y = Mathf.FloorToInt(Player_pos.y + 0.5f);
        int down = Read_chip(n_x, -n_y + 1);
        if (down == 1)
        {
            jump_set = false;
            now_fase = 0;
        }
    }
    void Player_task()
    {
        Vector3 now_p = player.transform.position;
        //総移動量
        Vector3 all_move = new Vector3(0.0f, 0.0f);
        //探索処理
        int n_x = Mathf.FloorToInt(now_p.x + 0.5f), n_y = Mathf.FloorToInt(now_p.y + 0.5f);
        //ジャンプ
        if (jump_set)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift))
            {               
                jump_set = false;
                jump_count = 0;
                now_fase = 1;                 
            }
        }
        switch (now_fase)
        {
            case 0:
                //落下処理
                ++fall_count;
                int down = Read_chip(n_x, -n_y + 1);
                int down_right = Read_chip(n_x + 1, -n_y + 1);
                int down_left = Read_chip(n_x - 1, -n_y + 1);
                if (down == 1)
                {
                    if (now_p.y - herf_y - Fall_spd * fall_count / 2 > n_y - 0.5f)
                    {
                        all_move += new Vector3(0.0f, -Fall_spd * fall_count / 2);
                    }
                    else
                    {
                        fall_count = 0;
                        player.transform.position = new Vector3(now_p.x, n_y + herf_y - 0.5f);
                        jump_set = true;
                    }
                }
                else
                {
                    if (now_p.x + herf_y > n_x + 1 - 0.5f)
                    {
                        if (down_right == 1)
                        {
                            if (now_p.y - herf_y - Fall_spd * fall_count / 2 > n_y - 0.5f)
                            {
                                all_move += new Vector3(0.0f, -Fall_spd * fall_count / 2);
                            }
                            else
                            {
                                fall_count = 0;
                                player.transform.position = new Vector3(now_p.x, n_y + herf_y - 0.5f);
                                jump_set = true;
                            }
                        }
                    }
                    else if (now_p.x - herf_y < n_x - 1 + 0.5f)
                    {
                        if (down_left == 1)
                        {
                            if (now_p.y - herf_y - Fall_spd * fall_count / 2 > n_y - 0.5f)
                            {
                                all_move += new Vector3(0.0f, -Fall_spd * fall_count / 2);
                            }
                            else
                            {
                                fall_count = 0;
                                player.transform.position = new Vector3(now_p.x, n_y + herf_y - 0.5f);
                                jump_set = true;
                            }
                        }
                    }
                    all_move += new Vector3(0.0f, -Fall_spd * fall_count / 2);
                }
                break;
            case 1:
                //ジャンプ処理
                ++jump_count;
                int up = Read_chip(n_x, -n_y - 1);
                int up_right = Read_chip(n_x + 1, -n_y - 1);
                int up_left = Read_chip(n_x - 1, -n_y - 1);
                if (up == 1)
                {                    
                    if (now_p.y + herf_y + Jump_spd < n_y + 0.5f)
                    {
                        all_move += new Vector3(0.0f, Jump_spd);
                    }
                    else
                    {
                        jump_count = Max_jump_count;
                        player.transform.position = new Vector3(now_p.x, n_y - herf_y + 0.5f);
                    }
                }
                else
                {
                    if (now_p.x + herf_y > n_x + 1 - 0.5f)
                    {
                        //all_move += new Vector3(0.0f, Jump_spd);
                        if (up_right == 1)
                        {
                            if (now_p.y + herf_y + Jump_spd < n_y + 0.5f)
                            {
                                all_move += new Vector3(0.0f, Jump_spd);
                            }
                            else
                            {
                                jump_count = Max_jump_count;
                                player.transform.position = new Vector3(now_p.x, n_y - herf_y + 0.5f);
                            }
                        }
                    }
                    else if (now_p.x - herf_y < n_x - 1 + 0.5f)
                    {
                        if (up_left == 1)
                        {
                            if (now_p.y + herf_y + Jump_spd < n_y + 0.5f)
                            {
                                all_move += new Vector3(0.0f, Jump_spd);
                            }
                            else
                            {
                                jump_count = Max_jump_count;
                                player.transform.position = new Vector3(now_p.x, n_y - herf_y + 0.5f);
                            }
                        }
                    }
                    all_move += new Vector3(0.0f, Jump_spd);

                }
                if (jump_count >= Max_jump_count)
                {
                    now_fase = 0;
                    fall_count = 0;
                }
                break;
            default:
                break;
        }
        //移動処理
        //左右入力
        float m_x = Input.GetAxisRaw("Horizontal");
        int side_s = 0;
        if (m_x < 0) { side_s = -1;}
        if (m_x > 0) { side_s = 1;}

        if (m_x != 0)
        {            
            int side = Read_chip(n_x + side_s, -n_y);
            if (side == 1)
            {
                if ((side_s == 1 && now_p.x + (herf_y + Move_spd) * side_s < n_x + 0.5f * side_s) || 
                   (side_s == -1 && now_p.x + (herf_y + Move_spd) * side_s > n_x + 0.5f * side_s))
                {
                    all_move += new Vector3(Move_spd * side_s, 0.0f);
                }
            }
            else
            {
                all_move += new Vector3(Move_spd * side_s, 0.0f);
            }         
        }
        //座標変換
        player.transform.position += all_move;
        //反転
        float Key = 0;
        if (m_x > 0)
        {
            Key = 0.8f;          
        }
       else if (m_x < 0)
        {
            Key = -0.8f;
           
        }
        if (Key != 0)
            player.transform.localScale = new Vector3(Key, 0.8f, 0.8f);                       
    }

  

    // Start is called before the first frame update
    void Start()
    {
        Set_camera();
        Set_map();
        Set_player();
        animator =  GetComponent<Animator>();
      
    }

    // Update is called once per frame
    void Update()
    {
        Player_task();       
        //能力 on of     
        if (Input.GetMouseButtonDown(0))
        {            
            Move_spd = 0;
            Max_jump_count = 0;
            Jump_spd = 0;                     
        }
        if (Input.GetMouseButtonUp(0))
        {
            Move_spd = 2.0f / 60;
           Max_jump_count = 15;
            Jump_spd = 8.0f / 60;
        }
        //if (Input.GetKey(KeyCode.Z))
        //{
        //    animator.SetTrigger("DieTrigger");
        //    gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, alpha);
        //    alpha -= 1.0f * Time.deltaTime;
        //    Debug.Log(alpha);
        //    if (alpha <= 0)
        //    {
                
        //        player.transform.position = new Vector3(0, 0, 0);
        //        alpha = 1;
        //    }
        //}       
    }
}
