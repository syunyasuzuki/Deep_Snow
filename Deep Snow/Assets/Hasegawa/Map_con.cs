using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_con : MonoBehaviour
{
    /////<summary></summary>

    //テスト用(後で消す)
    [SerializeField] int test_loadworld = 0;
    [SerializeField] int test_loadstage = 0;

    //ワールドの数
    /// <summary>ワールドの数</summary>
    const int World_num = 1;
    /// <summary>一つのワールドのステージ数</summary>
    const int Stage_num = 10;
    //----------ファイル関係--------------------
    /// <summary>ファイルのパス（アクセスはResourcesフォルダーから）</summary>
    string[] FilePath = null;
    /// <summary>ファイルのパスを設定</summary>
    void Set_path() {
        FilePath = new string[World_num] { "mapdata" };
    }
    //----------マップ関係--------------------
    //マップサイズ
    /// <summary>マップサイズ　X</summary>
    const int Mapsize_x = 32;
    /// <summary>マップサイズ　Y</summary>
    const int Mapsize_y = 18;
    /// <summary>マップの大きさを返す</summary>
    public void Read_mapsize(ref int x, ref int y) {
        x = Mapsize_x;
        y = Mapsize_y;
    }
    /// <summary>全てのマップデータ</summary>
    int[,,,] map = new int[World_num, Stage_num, Mapsize_y, Mapsize_x];

    /// <summary>一括で全ステージを読み込む（データがリソースファイルにある前提）</summary>
    void Read_all_maps() {
        for (int w = 0; w < World_num; ++w) {
            string text_data = (Resources.Load(FilePath[w], typeof(TextAsset)) as TextAsset).text;
            string[] text_line = text_data.Split('\n');
            for (int s = 0; s < Stage_num; ++s) {
                for (int y = 0; y < Mapsize_y; ++y) {
                    string[] strsplr = text_line[Mapsize_y * s + y + 1].Split(',');
                    for (int x = 0; x < Mapsize_x; ++x) {
                        map[w, s, y, x] = int.Parse(strsplr[x]);
                    }
                }
            }
        }
    }
    //素材
    ///<summary>ノーマルブロック</summary>
    [SerializeField] Sprite[] Normal_block = new Sprite[3];
    ///<summary>雪の積もってるブロック</summary>
    [SerializeField] Sprite[] Falled_block = new Sprite[4];
    /// <summary>各マップチップの素材</summary>
    [SerializeField] Sprite[] Blocks = new Sprite[11];
    ///<summary>各プレハブ</summary>
    [SerializeField] GameObject[] Gos = new GameObject[3];
    /// <summary>マップチップを入れる親オブジェクト</summary>
    GameObject Map_mother = null;
    ///<summary>一回でもマップを生成したか</summary>
    bool create_map = false;
    ///<summary>現在使っているワールド番号</summary>
    int now_world = 0;
    ///<summary>現在使っているステージ番号</summary>
    int now_stage = 0;
    /// <summary>現在使っているマップ</summary>
    int[,] now_map = new int[Mapsize_y, Mapsize_x]{
        { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{ 0,0,0,0,0,0,0,0,0,2,2,2,2,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{ 13,0,0,0,0,0,0,11,0,0,0,0,0,0,0,0,11,0,0,0,0,0,0,0,0,0,0,0,0,0,0,12},
{ 2,2,2,2,2,2,2,2,2,10,10,10,10,10,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
{ 1,1,1,1,1,1,1,1,1,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
{ 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    };
    ///<summary>現在使っているマップの指定されたマップ情報を返す</summary>
    public int Read_mapchip(int x,int y){
        return now_map[y, x];
    }
    ///<summary>現在使っているマップの棘の数</summary>
    int spike_num = 0;
    ///<summary>現在使っている棘の位置</summary>
    Vector3[] spike_pos = new Vector3[500];
    ///<summary>棘の数を返す</summary>
    public int Read_spike(){
        return spike_num;
    }
    ///<summary>棘の位置を返す</summary>
    public Vector3 Read_spike_pos(int x){
        return spike_pos[x];
    }
    ///<summary>積もる雪が生成できるブロックの数</summary>
    int fed_snw = 0;
    ///<summary>積もる雪の位置</summary>
    Vector3[] fed_snw_pos = new Vector3[500];
    ///<summary>積もる位置の雪の番号</summary>
    int[] fed_snw_num = new int[500];
    ///<summary>積もる雪の位置を返す</summary>
    public Vector3 Read_fed_snw_pos(int x){
        return fed_snw_pos[x];
    }
    ///<summary>積もる場所の番号を返す</summary>
    public int Read_fed_snw_num(int x){
        return fed_snw_num[x];
    }
    ///<summary>積もる雪の数、雪の位置を割り出す</summary>
    void Sarch_fed_snw(int w,int s){
        fed_snw = 0;
        for(int na = 0; na < Mapsize_x; ++na){
            for(int lu = 0; lu < Mapsize_y; ++lu){
                if (map[w, s, lu, na] != 0){
                    if (lu - 1 >= 0){
                        if (map[w, s, lu - 1, na] == 0 || map[w, s, lu - 1, na] == 10){
                            if (map[w, s, lu, na] == 10 || (map[w, s, lu, na] >= 3 && map[w, s, lu, na] <= 6) || map[w, s, lu, na] == 11 || map[w, s, lu, na] == 12 || map[w, s, lu, na] == 13)
                            {
                                fed_snw_pos[fed_snw] = new Vector3(na, -lu - 0.5f, 0.0f);
                                fed_snw_num[fed_snw] = map[w, s, lu, na];
                            }
                            else{
                                fed_snw_pos[fed_snw] = new Vector3(na, -lu + 1 - 0.5f, 0.0f);
                                fed_snw_num[fed_snw] = map[w, s, lu - 1, na];
                            }
                            ++fed_snw;
                            break;
                        }
                    }
                }
            }
        }
    }
    /// <summary>指定されたチップを生成</summary>
    /// <param name="n">生成するオブジェクトの番号</param>
    public void Create_chip(int x, int y, int n){
        switch (n){
            case 0:
                break;
            case 1:
                int rnd = Random.Range(0, 2);
                GameObject n_bl = new GameObject("nb_bl " + x + " - " + y);
                n_bl.AddComponent<SpriteRenderer>().sprite = Normal_block[rnd];
                n_bl.transform.position = new Vector3(x, -y, 0.0f);
                n_bl.transform.parent = Map_mother.transform;
                break;
            case 2:
                int rnd2 = Random.Range(0, 3);
                GameObject f_bl = new GameObject("f_bl " + x + " - " + y);
                f_bl.AddComponent<SpriteRenderer>().sprite = Falled_block[rnd2];
                f_bl.transform.position = new Vector3(x, -y, 0.0f);
                f_bl.transform.parent = Map_mother.transform;
                break;
            case 10:
                GameObject s_bl = new GameObject("s_bl " + x + " - " + y);
                s_bl.AddComponent<SpriteRenderer>().sprite = Blocks[10];
                s_bl.transform.position = new Vector3(x, -y, 0.0f);
                s_bl.transform.parent = Map_mother.transform;
                spike_pos[spike_num] = new Vector3(x, -y, 0.0f);
                ++spike_num;
                break;
            case 11:
                //敵
                GameObject enemy = Instantiate(Gos[0]);
                enemy.transform.position = new Vector3(x, -y, 0.0f);
                enemy.transform.parent = Map_mother.transform;
                break;
            case 12:
                //ゴール
                GameObject goal = Instantiate(Gos[1]);
                goal.transform.position = new Vector3(x, -y, 0.0f);
                goal.transform.parent = Map_mother.transform;
                break;
            case 13:
                //プレイヤー
                GameObject player = Instantiate(Gos[2]);
                player.transform.position = new Vector3(x, -y, 0.0f);
                player.transform.parent = Map_mother.transform;
                break;
            default:
                GameObject bl = new GameObject("bl " + x + " - " + y);
                bl.AddComponent<SpriteRenderer>().sprite = Blocks[n];
                bl.transform.position = new Vector3(x, -y, 0.0f);
                bl.transform.parent = Map_mother.transform;
                break;
        }
    }
    /// <summary>指定されたマップを生成</summary>
    /// <param name="w">生成するワールド</param>
    /// <param name="s">生成するステージ</param>
    public void Create_map(int w, int s){
        Snow_con snc = GetComponent<Snow_con>();
        if (create_map){
            Destroy(Map_mother.gameObject);
            snc.Destroy_allsnow();
        }
        Map_mother = new GameObject("map_mother");
        spike_num = 0;
        for (int lu = 0; lu < Mapsize_y; ++lu){
            for (int na = 0; na < Mapsize_x; ++na){
                Create_chip(na, lu, now_map[lu,na]);
            }
        }
        Sarch_fed_snw(w, s);
        snc.Set_all_snows();
        create_map = true;
    }
    /// <summary>現在使われているマップを消す</summary>
    public void Delete_map(){
        GameObject map_mother = GameObject.Find("map_mother");
        Destroy(map_mother.gameObject);
    }

    // Start is called before the first frame update
    void Start(){
        Set_path();
        Read_all_maps();
    }

    // Update is called once per frame
    void Update(){
        //Debug_mode();
    }

    /// <summary>デバッグ用（あとで消す！！！）</summary>
    void Debug_mode(){
        if (Input.GetKeyDown(KeyCode.D)) Create_map(test_loadworld, test_loadstage);
    }
}
