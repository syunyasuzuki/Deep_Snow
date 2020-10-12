using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Snow_con : MonoBehaviour
{
    /////<summary></summary>

    /// <summary>マップデータを受け取る参照先</summary>
    Map_con Map_ob = null;

    /// <summary>降る雪の数</summary>
    const int Max_fallsnows = 100;
    /// <summary>積もる雪の分割数</summary>
    const int Sprit_falledsnow = 25;
    /// <summary>雲の大きさ</summary>
    const float CloudSize_x = 1.0f;

    /// <summary>生成する雪</summary>
    [SerializeField] GameObject ob_snow = null;
    /// <summary>生成する雲</summary>
    [SerializeField] Sprite spr_cloud = null;

    ///<summary>ループ処理を止める</summary>
    bool snow_task = false;
    /// <summary>生成した積もる雪</summary>
    GameObject[] falled_snow = null;
    /// <summary>生成した積もる雪を入れる親オブジェクト</summary>
    GameObject Master_falled_snow = null;
    /// <summary>生成した降る雪</summary>
    GameObject[] fall_snow = new GameObject[Max_fallsnows];
    ///<summary>各降る雪の落下速度</summary>
    float[] fall_spd = new float[Max_fallsnows];
    ///<summary>各降る雪の状態（0 = 停止 , 1 = 落下 , 2 = 停止待機）</summary>
    int[] fall_mode = new int[Max_fallsnows];
    /// <summary>生成した降る雪を入れる親オブジェクト</summary>
    GameObject Master_fall_snow = null;
    /// <summary>生成した雲</summary>
    GameObject Cloud = null;

    //マップの大きさ
    /// <summary>マップの大きさ　X</summary>
    private int map_size_x = 32;
    /// <summary>マップの大きさ　Y</summary>
    private int map_size_y = 18;
    ///<summary>積もる雪を置く位置</summary>
    int[,] snw_pos = new int[4, Sprit_falledsnow]{
        {25,25,24,23,22,20,19,19,17,17,16,15,14,13,12,11,10,9,8,6,6,5,4,2,1 },
        {1,2,3,4,5,7,8,9,10,11,12,13,14,15,16,17,17,19,19,20,22,23,24,25,25 },
        {14,15,15,16,16,17,17,17,18,18,19,19,20,21,21,21,22,22,23,23,24,24,24,25,25 },
        {1,1,2,3,3,3,4,4,5,6,7,7,7,8,8,9,9,10,10,11,11,11,12,13,14 }
    };
    ///<summary>積もる雪の数</summary>
    int fed_snw;
    ///<summary>積もる雪の位置</summary>
    Vector3[] fed_snw_pos;
    ///<summary>積もる雪を置く地点の番号</summary>
    int[] fed_snw_num;
    /// <summary>マップから必要なデータをもらう</summary>
    void Set_Map_con(){
        Map_ob = GetComponent<Map_con>();
    }
    /// <summary>積もる雪を準備</summary>
    void Set_falledSnows(){
        fed_snw = Map_ob.Read_fed_snw();
        fed_snw_pos = new Vector3[fed_snw];
        fed_snw_num = new int[fed_snw];
        for(int lu = 0; lu < fed_snw; ++lu){
            fed_snw_pos[lu] = Map_ob.Read_fed_snw_pos(lu);
            fed_snw_num[lu] = Map_ob.Read_fed_snw_num(lu);
        }
        Master_falled_snow = new GameObject("falled_snow");
        falled_snow = new GameObject[fed_snw * Sprit_falledsnow];
        for(int lu = 0; lu < fed_snw; ++lu){
            for(int na = 0; na < Sprit_falledsnow; ++na){
                falled_snow[lu * Sprit_falledsnow + na] = Instantiate(ob_snow);
                falled_snow[lu * Sprit_falledsnow + na].transform.parent = Master_falled_snow.transform;
                float sub = 1.0f / Sprit_falledsnow;
                falled_snow[lu * Sprit_falledsnow + na].transform.localScale = new Vector3(sub, sub, 0.0f);
                switch (fed_snw_num[lu]){
                    case 0:
                        falled_snow[lu * Sprit_falledsnow + na].transform.position = new Vector3(fed_snw_pos[lu].x - 0.5f + sub * na + sub / 2, fed_snw_pos[lu].y, 0.0f);
                        break;
                    case 10:
                        falled_snow[lu * Sprit_falledsnow + na].transform.position = new Vector3(fed_snw_pos[lu].x - 0.5f + sub * na + sub / 2, fed_snw_pos[lu].y, 0.0f);
                        break;
                    default:
                        falled_snow[lu * Sprit_falledsnow + na].transform.position = new Vector3(fed_snw_pos[lu].x - 0.5f + sub * na + sub / 2, fed_snw_pos[lu].y + sub * snw_pos[fed_snw_num[lu] - 3, na], 0.0f);
                        break;
                }
            }
        }
    }
    /// <summary>雲と降る雪を準備</summary>
    void Set_fallsnows(){
        Cloud = new GameObject("cloud");
        Cloud.AddComponent<SpriteRenderer>().sprite = spr_cloud;
        Cloud.GetComponent<SpriteRenderer>().sortingOrder = 1;
        Cloud.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        Master_fall_snow = new GameObject("fall_snows");
        for(int lu = 0; lu < Max_fallsnows; ++lu){
            fall_snow[lu] = Instantiate(ob_snow);
            fall_snow[lu].transform.position = new Vector3(-1.0f, 1.0f, 0.0f);
            fall_snow[lu].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            fall_snow[lu].transform.parent = Master_fall_snow.transform;
            fall_spd[lu] = Random.Range(0.05f, 0.1f);
            fall_mode[lu] = 0;
        }
    }

    /// <summary>積もる雪と降る雪と雲を消す</summary>
    public void Destroy_allsnow(){
        Destroy(Master_fall_snow.gameObject);
        Destroy(Master_falled_snow.gameObject);
        Destroy(Cloud.gameObject);
        snow_task = false;
    }
    /// <summary>降る雪の処理</summary>
    Vector3 plus_snow = new Vector3(0.0f, 0.05f, 0.0f);
    /// <summary>降る雪の処理</summary>
    void Fallsnows_task(){
        Vector3 c_pos = Cloud.transform.position;
        for(int lu = 0; lu < Max_fallsnows; ++lu){
            if (fall_mode[lu] != 0){
                fall_snow[lu].transform.position -= new Vector3(0.0f, fall_spd[lu], 0.0f);
                if (fall_snow[lu].transform.position.y < -map_size_y){
                    if (fall_mode[lu] == 2){
                        fall_snow[lu].transform.position = new Vector3(-1.0f, 1.0f, 0.0f);
                        fall_mode[lu] = 0;
                    }
                    else{
                        fall_snow[lu].transform.position = new Vector3(c_pos.x + Random.Range(-CloudSize_x, CloudSize_x), c_pos.y, 0.0f);
                    }
                }
                
            }
        }
    }
    ///<summary>雪を降らす</summary>
    void Start_fall_snows(){
        Vector3 c_pos = Cloud.transform.position;
        for(int lu = 0; lu < Max_fallsnows; ++lu){
            fall_snow[lu].transform.position = new Vector3(c_pos.x + Random.Range(-CloudSize_x, CloudSize_x), c_pos.y, 0.0f);
            fall_mode[lu] = 1;
        }
    }
    ///<summary>雪を止ます（今降っている分は降り続ける）</summary>
    void  Stop_fall_snows(){
        for(int lu = 0; lu < Max_fallsnows; ++lu){
            fall_mode[lu] = 2;
        }
    }
    ///<summary>雪を止める</summary>
    void Stop_fall_snows_c(){
        for(int lu = 0; lu < Max_fallsnows; ++lu){
            fall_mode[lu] = 0;
        }
    }
    ///<summary>雪をリセット</summary>
    public void Reset_fall_snows(){
        for(int lu = 0; lu < Max_fallsnows; ++lu){
            fall_snow[lu].transform.position = new Vector3(-1.0f, 1.0f, 0.0f);
            fall_mode[lu] = 0;
        }
    }
    ///<summary>カメラ</summary>
    Camera maincam;
    ///<summary>カメラを取得</summary>
    void Set_camera(){
        maincam = Camera.main;
    }
    /// <summary>雲を動かす処理</summary>
    void Move_clouds_task(){
        Vector3 m_position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(maincam.transform.position.z));
        Vector3 w_position = Camera.main.ScreenToWorldPoint(m_position);
        int m_x = Mathf.FloorToInt(w_position.x), m_y = Mathf.FloorToInt(w_position.y + 0.5f);
        Cloud.transform.position = new Vector3(m_x + 0.5f , m_y, 0.0f);
        if (Input.GetMouseButtonDown(0)) { Start_fall_snows(); }
        if (Input.GetMouseButtonUp(0)) { Stop_fall_snows(); }
    }
    ///<summary>マップ作成時に実行</summary>
    public void Set_all_snows(){
        Set_falledSnows();
        Set_fallsnows();
        snow_task = true;
    }
    // Start is called before the first frame update
    void Start(){
        Set_Map_con();
        Set_camera();
    }

    // Update is called once per frame
    void Update(){
        if (snow_task){
            Fallsnows_task();
            Move_clouds_task();
        }
        if (Input.GetKeyDown(KeyCode.H)) { Debug.Log(falled_snow.Length); }
    }
}
