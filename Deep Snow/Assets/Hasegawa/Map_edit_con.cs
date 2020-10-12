using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class Map_edit_con : MonoBehaviour
{

    ///// <summary></summary>

    /// <summary>ワールドの数</summary>
    const int Max_world = 5;
    /// <summary>一つのワールドのステージの数</summary>
    const int Max_stage = 10;
    /// <summary>フォルダへのパス</summary>
    string fopath;
    /// <summary>ファイルへのパス</summary>
    string[] fipath = null;
    /// <summary>フォルダとファイルへのパスを設定</summary>
    void Set_path(){
        fopath = Application.dataPath + @"\Resources";
        fipath = new string[5] { "World1.txt", "World2.txt", "World3.txt", "World4.txt", "World5.txt" };
    }
    /// <summary>ファイルが存在するか確認する、存在しない場合作成する</summary>
    void Folder_file_check_andCreate(){
        //テキストの書き込み用
        StreamWriter datafile = null;
        //フォルダが存在するか確認
        if (!Directory.Exists(fopath)){
            Debug.Log("そんなフォルダないよ");
            try{
                Debug.Log("ないからフォルダ作るね");
                Directory.CreateDirectory(fopath);
                if (!Directory.Exists(fopath)){
                    Debug.Log("フォルダ作れたよ");
                }
            }
            catch{
                Debug.Log("フォルダ作成失敗したよ");
            }
        }
        else{
            Debug.Log("フォルダあるよ");
        }
        //ファイルが存在するか確認
        for(int lu = 0; lu < Max_world; ++lu){
            string f = Path.Combine(fopath, fipath[lu]);
            if (!File.Exists(f)){
                Debug.Log("そんなファイルないよ");
                try{
                    Debug.Log("ないからファイル作るね");
                    datafile = File.CreateText(f);
                    if (!File.Exists(f)){
                        Debug.Log("ファイル作れたよ");
                    }
                }
                catch{
                    Debug.Log("ファイル作成失敗したよ");
                }
                finally{
                    datafile.Dispose();
                }
            }
            else{
                Debug.Log("ファイルあるよ");
            }
        }
    }

    Camera maincam;
    /// <summary>カメラの位置をZ</summary>
    float camera_z = -10;
    /// <summary>カメラの設定をする</summary>
    void Set_camera(){
        maincam = Camera.main;
        float camera_x = 15.5f;
        float camera_y = -7;
        float camera_size = 11;
        maincam.transform.position = new Vector3(camera_x, camera_y, camera_z);
        maincam.orthographicSize = camera_size;
    }
    /// <summary>カメラサイズを変更する速度</summary>
    float Move_camera_size = 1.0f;
    /// <summary>カメラの移動速度</summary>
    float Move_camera_xy = 10.0f / 60f;
    /// <summary>カメラの位置を調整する</summary>
    void Camera_task(){
        float scr = Input.GetAxisRaw("Mouse ScrollWheel");
        float camsize = Move_camera_size * scr;
        maincam.orthographicSize = maincam.orthographicSize + camsize;
        int x = 0, y = 0;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { x -= 1; }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { x += 1; }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { y += 1; }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { y -= 1; }
        maincam.transform.position = new Vector3(maincam.transform.position.x + Move_camera_xy * x, maincam.transform.position.y + Move_camera_xy * y, camera_z);
    }

    /// <summary>ライトを作成する</summary>
    void Create_light(){
        GameObject light = new GameObject("light");
        light.AddComponent<Light>();
        Light lht = light.GetComponent<Light>();
        lht.type = LightType.Directional;
    }

    /// <summary>生成するマウスカーソル</summary>
    GameObject mouse_point;
    /// <summary>マウスカーソルを生成</summary>
    void Create_mouse_point(){
        mouse_point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mouse_point.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    //マップの最大サイズ
    /// <summary>マップの大きさX</summary>
    const int Maxsize_x = 32;
    /// <summary>マップの大きさY</summary>
    const int Maxsize_y = 18;

    /// <summary>ノーマルブロック素材</summary>
    [SerializeField] Sprite[] Normal_block = new Sprite[3];
    /// <summary>雪の積もったブロック素材</summary>
    [SerializeField] Sprite[] Falled_block = new Sprite[4];
    /// <summary>その他マップの素材</summary>
    [SerializeField] Sprite[] Blocks = new Sprite[12];

    /// <summary>現在読み込み中のワールド番号</summary>
    int world_num = 0;
    /// <summary>現在読み込み中のステージ番号</summary>
    int stage_num = 0;
    /// <summary>現在選択中のブロック</summary>
    int now_block = 0;
    /// <summary>マップ</summary>
    int[,,,] map;
    /// <summary>各ワールドのステージ数</summary>
    int[] stage_nums;
    /// <summary>マップチップを格納する親オブジェクト</summary>
    GameObject mapmother = null;

    /// <summary>桁数を返す</summary>
    int Int_length(int n){
        return (int)Mathf.Log10(n) + 1;
    }

    /// <summary>配列を確保する</summary>
    void First_setting(){
        map = new int[Max_world, Max_stage, Maxsize_y, Maxsize_x];
        stage_nums = new int[Max_world];
        for(int lu = 0; lu < Max_world; ++lu){
            stage_nums[lu] = 1;
        }
    }

    /// <summary>マップの外枠を作成</summary>
    void Create_Mapgrid(){
        float def_hw = 0.4f;
        GameObject mapgr = new GameObject("mapgr");
        int w_size = Maxsize_x;
        float w_pos = (Maxsize_x - 1) / 2.0f;
        GameObject loof = GameObject.CreatePrimitive(PrimitiveType.Cube);
        loof.transform.localScale = new Vector3(w_size, def_hw, 1);
        loof.transform.position = new Vector3(w_pos, 0.7f, 0);
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.localScale = new Vector3(w_size, def_hw, 1);
        floor.transform.position = new Vector3(w_pos, -Maxsize_y + 0.3f, 0);
        int h_size = Maxsize_y;
        float h_pos = (Maxsize_y - 1) / 2.0f * -1;
        GameObject left = GameObject.CreatePrimitive(PrimitiveType.Cube);
        left.transform.localScale = new Vector3(def_hw, h_size, 1);
        left.transform.position = new Vector3(-0.7f, h_pos, 0);
        GameObject right = GameObject.CreatePrimitive(PrimitiveType.Cube);
        right.transform.localScale = new Vector3(def_hw, h_size, 1);
        right.transform.position = new Vector3(Maxsize_x - 0.3f, h_pos, 0);
        loof.transform.parent = mapgr.transform;
        floor.transform.parent = mapgr.transform;
        left.transform.parent = mapgr.transform;
        right.transform.parent = mapgr.transform;
    }
    /// <summary>指定されたマップを生成する</summary>
    void Create_Map(int w,int s){
        mapmother = new GameObject("mapchip");
        for (int i = 0; i < Maxsize_y; i++){
            for (int lu = 0; lu < Maxsize_x; lu++){
                switch (map[w, s, i, lu]){
                    case 0:
                        break;
                    case 1:
                        int rnd1 = Random.Range(0, 2);
                        GameObject n_bl = new GameObject("chip_" + i + "_" + lu);
                        n_bl.AddComponent<SpriteRenderer>().sprite = Normal_block[rnd1];
                        n_bl.transform.position = new Vector3(lu, -i, 0.0f);
                        n_bl.transform.parent = mapmother.transform;
                        break;
                    case 2:
                        int rnd2 = Random.Range(0, 3);
                        GameObject f_bl = new GameObject("chip_" + i + "_" + lu);
                        f_bl.AddComponent<SpriteRenderer>().sprite = Falled_block[rnd2];
                        f_bl.transform.position = new Vector3(lu, -i, 0.0f);
                        f_bl.transform.parent = mapmother.transform;
                        break;
                    default:
                        GameObject subgo = new GameObject("chip_" + -i + "_" + lu);
                        subgo.AddComponent<SpriteRenderer>().sprite = Blocks[now_block];
                        subgo.transform.position = new Vector3(lu, -i, 0);
                        subgo.transform.parent = mapmother.transform;
                        break;
                }
            }
        }
    }
    //マップを一括削除する
    void Delete_Map(){
        Destroy(mapmother.gameObject);
        mapmother = new GameObject("mapmother");
    }
    /// <summary>ファイルをすべて読み込んで配列へ変換する</summary>
    void Read_data(){
        for(int w = 0; w < Max_world; ++w){
            string[] alldata = File.ReadAllLines(Path.Combine(fopath, fipath[w]), Encoding.GetEncoding("Shift_JIS"));
            if (alldata.Length >= Maxsize_y + 1){
                stage_nums[w] = int.Parse(alldata[0]);
                if (stage_nums[w] > Max_stage){
                    stage_nums[w] = Max_stage;
                }
                for(int s = 0; s < stage_nums[w]; ++s){
                    for(int y = 0; y < Maxsize_y; ++y){
                        string strline = alldata[y + Maxsize_y * s + 1];
                        string[] strspl = strline.Split(',');
                        for(int x = 0; x < Maxsize_x; ++x){
                            map[w, s, y, x] = int.Parse(strspl[x]);
                        }
                    }
                }
            }
        }
        Create_Map(0, 0);
    }
    /// <summary>
    /// ファイルに書き出す（全て上書き）
    /// ゲーム開始時にフォルダとファイルがあることは確認済みのため省略
    /// </summary>
    void Write_all_data(){
        Debug.Log("ファイルに書き出し");
        for(int w = 0; w < Max_world; ++w){
            int Write_num = Maxsize_y * (stage_nums[w]) + 1;
            string[] write_str = new string[Write_num];
            write_str[0] = "" + stage_nums[w];
            for(int s = 0; s < stage_nums[w]; ++s){
                for(int y = 0; y < Maxsize_y; ++y){
                    string strline = "";
                    for(int x = 0; x < Maxsize_x; ++x){
                        strline = strline + map[w, s, y, x] + ",";
                    }
                    strline = strline.Substring(0, strline.Length - 1);
                    write_str[Maxsize_y * s + y + 1] = strline;
                }
            }
            File.WriteAllLines(Path.Combine(fopath, fipath[w]), write_str);
        }
        Debug.Log("書き出し完了");
    }
    //UIで使うフォントのサイズ
    int Def_fontsize = 15;
    int Max_fontsize = 20;
    //一つ目のボタンの位置
    int px = 400;
    int px_num = 0;
    //現在選択されているボタン
    void Set_UIpos(int x){
        px_num = x;
    }
    //入力処理用
    string text_w_num = "0";
    string text_s_num = "0";
    //各最大入力数
    int max_text_w_num;
    int max_text_s_num;
    //最大入力数を設定
    void GUI_setting(){
        max_text_w_num = Int_length(Max_world);
        max_text_s_num = Int_length(Max_stage);
    }
    //入力処理の確定
    void Set_text_num(){
        int sub_w;
        try{
            sub_w = int.Parse(text_w_num);
        }
        catch{
            Debug.LogError("文字列の変換に失敗しました、ワールド番号を確認してください");
            return;
        }
        if (sub_w < 0 || sub_w >= Max_world) return;
        int sub_s;
        try{
            sub_s = int.Parse(text_s_num);
        }
        catch{
            Debug.LogError("文字列の変換に失敗しました、ステージ番号を確認してください");
            return;
        }
        if (sub_s < 0 || sub_s >= Max_stage) return;
        if (sub_w == world_num && sub_s == stage_num) return;
        Delete_Map();
        world_num = sub_w;
        stage_num = sub_s;
        if (stage_num <= Max_stage){
            stage_nums[world_num] = stage_num;
        }
        else{
            stage_num = Max_stage;
        }
        Create_Map(world_num, stage_num);
    }

    /// <summary>切り抜く用のテクスチャ</summary>
    [SerializeField] Texture2D map_texture = null;
    /// <summary>ボタン用の棘のテクスチャ</summary>
    [SerializeField] Texture2D spike = null;
    /// <summary>ボタン用の壊れる床</summary>
    [SerializeField] Texture2D ice = null;
    /// <summary>切り抜いたテクスチャ</summary>
    Texture2D[] Sliced_texture = new Texture2D[12];
    /// <summary>ボタンに表示する文字</summary>
    string[] texture_name = new string[12]{
        "空白","","","","","","","","","","",""
    };
    /// <summary>指定された位置からテクスチャを切り抜いて返す</summary>
    Texture2D Slice_Texture2D(Texture2D texture,int x,int y){
        Color[] pix = texture.GetPixels(x, y, 25, 25);
        Color[] pix2 = new Color[50 * 50];
        for(int lu = 0; lu < 50; ++lu){
            for(int na = 0; na < 25; ++na){
                pix2[lu * 50 + na * 2] = pix[25 * (lu / 2) + na];
                pix2[lu * 50 + na * 2 + 1] = pix[25 * (lu / 2) + na];
            }
        }
        Texture2D slice_tex = new Texture2D(50, 50);
        slice_tex.SetPixels(pix2);
        slice_tex.Apply();
        return slice_tex;
    }
    /// <summary>テクスチャを切り抜く位置を指定</summary>
    void Set_textures(){
        Sliced_texture[0] = Slice_Texture2D(map_texture, 0, 25);
        Sliced_texture[1] = Slice_Texture2D(map_texture, 50, 0);
        Sliced_texture[2] = Slice_Texture2D(map_texture, 0, 0);
        Sliced_texture[3] = Slice_Texture2D(map_texture, 75, 25);
        Sliced_texture[4] = Slice_Texture2D(map_texture, 25, 25);
        Sliced_texture[5] = Slice_Texture2D(map_texture, 150, 25);
        Sliced_texture[6] = Slice_Texture2D(map_texture, 125, 25);
        Sliced_texture[7] = Slice_Texture2D(map_texture, 75, 0);
        Sliced_texture[8] = Slice_Texture2D(map_texture, 125, 0);
        Sliced_texture[9] = Slice_Texture2D(map_texture, 25, 0);
        Sliced_texture[10] = spike;
        Sliced_texture[11] = Slice_Texture2D(ice, 0, 0);
    }
    //UIをプログラムから作成
    void OnGUI(){
        //画面の比率に応じてUIのサイズが変わるようにする
        float widthsize = (float)Screen.width / 1925;
        float heightsize = (float)Screen.height / 868;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(widthsize, heightsize, 1));
        //テキストの設定を作成
        GUIStyle Set_label = GUI.skin.label;
        Set_label.fontSize = Def_fontsize;
        Set_label.normal.textColor = Color.white;
        //テキストUIを表示
        GUI.Label(new Rect(20, 20, 250, 40), "選択中のワールド：" + world_num +" ステージ"+ stage_num);
        GUI.Label(new Rect(50, 50, 250, 40), "W　　　S");
        GUI.Label(new Rect(px + (50 * px_num), 10, 100, 100), " 選択中\nブロック\n　 ▼", Set_label);
        //ボタンの設定を作成する
        GUIStyle Set_button = GUI.skin.button;
        Set_button.fontSize = Def_fontsize;
        Set_button.normal.textColor = Color.white;
        Set_button.normal.background = null;
        //ボタンを表示する
        if (GUI.Button(new Rect(200, 70, 40, 40), "決定")){
            Set_text_num();
        }
        if (GUI.Button(new Rect(250, 70, 40, 40), "書出")){
            Write_all_data();
        }
        Set_button.fontSize = Max_fontsize;
        for(int lu = 0; lu < 12;++lu){
            Set_button.normal.background = Sliced_texture[lu];
            if (GUI.Button(new Rect(px + (50 * lu), 60, 50, 50), texture_name[lu])){
                Set_UIpos(lu);
                now_block = lu;
            }
        }
        //入力テキストの設定を作成
        GUIStyle Set_textfield = GUI.skin.textField;
        Set_textfield.fontSize = Max_fontsize;
        //入力テキストを作成
        text_w_num = GUI.TextField(new Rect(50, 70, 40, 40), text_w_num, max_text_w_num);
        text_s_num = GUI.TextField(new Rect(125, 70, 40, 40), text_s_num, max_text_s_num);
    }

    void Awake(){
        Set_path();
        Folder_file_check_andCreate();
        Create_light();
        Set_camera();
        First_setting();
        Read_data();
        GUI_setting();
        Create_mouse_point();
        Create_Mapgrid();
        Set_textures();
    }
    void Start()
    {
    }

    /// <summary>マウスの処理</summary>
    void Mouse_task(){
        //マウスの位置を取得
        Vector3 sub3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(camera_z));
        //ワールド座標に変換
        Vector3 sub31 = Camera.main.ScreenToWorldPoint(sub3);
        //マップ座標に最適化
        int px = Mathf.FloorToInt(sub31.x + 0.5f), py = Mathf.FloorToInt(sub31.y + 0.5f);
        mouse_point.transform.position = new Vector3(px, py, -1);
        if (Input.GetMouseButton(0)){
            //範囲外を除外
            if (px >= 0 && px < Maxsize_x && -py >= 0 && -py < Maxsize_y){
                //mapに確認
                if (map[world_num,stage_num, -py, px] != now_block){
                    if (map[world_num, stage_num, -py, px] != 0){
                        GameObject so = GameObject.Find("chip_" + -py + "_" + px);
                        Destroy(so.gameObject);
                    }
                    switch (now_block){
                        case 0:
                            map[world_num, stage_num, -py, px] = 0;
                            break;
                        case 1:
                            int rnd1 = Random.Range(0, 2);
                            GameObject n_bl = new GameObject("chip_" + -py + "_" + px);
                            n_bl.AddComponent<SpriteRenderer>().sprite = Normal_block[rnd1];
                            n_bl.transform.position = new Vector3(px, py, 0.0f);
                            n_bl.transform.parent = mapmother.transform;
                            map[world_num, stage_num, -py, px] = 1;
                            break;
                        case 2:
                            int rnd2 = Random.Range(0, 3);
                            GameObject f_bl = new GameObject("chip_" + -py + "_" + px);
                            f_bl.AddComponent<SpriteRenderer>().sprite = Falled_block[rnd2];
                            f_bl.transform.position = new Vector3(px, py, 0.0f);
                            f_bl.transform.parent = mapmother.transform;
                            map[world_num, stage_num, -py, px] = 2;
                            break;
                        default:
                            GameObject subgo = new GameObject("chip_" + -py + "_" + px);
                            subgo.AddComponent<SpriteRenderer>().sprite = Blocks[now_block];
                            subgo.transform.position = new Vector3(px, py, 0);
                            subgo.transform.parent = mapmother.transform;
                            map[world_num, stage_num, -py, px] = now_block;
                            break;
                    }
                }
            }
        }
    }

    void Update(){
        Mouse_task();
        Camera_task();
    }
}
