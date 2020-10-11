using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class map_edit2 : MonoBehaviour{

    //フォルダへのパス
    string fopath;
    //ファイルへのパス
    string fipath;
    //フォルダとファイルへのパスを設定
    void Set_path(){
        fopath = Application.dataPath + @"\Resources";
        fipath = Path.Combine(fopath, "mapdata.txt");
    }
    //フォルダーとファイルが存在するか確認する、存在しない場合作成する
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
        if (!File.Exists(fipath)){
            Debug.Log("そんなファイルないよ");
            try{
                Debug.Log("ないからファイル作るね");
                datafile = File.CreateText(fipath);
                if (!File.Exists(fipath)){
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

    Camera maincam;
    //カメラのpositionのZ座標
    float camera_z = -10;
    //取得する
    void Set_camera_z(){
        maincam = GetComponent<Camera>();
        camera_z = transform.position.z;
        float camera_x = (Maxsize_x - 1) / 2.0f;
        float camera_y = (Maxsize_y - 1) / 2.0f * -1;
        int min = Mathf.Min(Maxsize_x, Maxsize_y);
        float camera_size = min / 2.0f;
        maincam.transform.position = new Vector3(camera_x, camera_y, camera_z);
        maincam.orthographicSize = camera_size;
    }
    //カメラの引き具合
    float Move_camera_size = 1.0f;
    //カメラの移動速度
    float Move_camera_xy = 0.05f;
    //カメラの位置を調整する
    void Set_camera(){
        float scr = Input.GetAxisRaw("Mouse ScrollWheel");
        float camsize = Move_camera_size * scr;
        maincam.orthographicSize = maincam.orthographicSize + camsize;
        int x = 0, y = 0;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { x -= 1; }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { x += 1; }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { y += 1; }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { y -= 1; }
        transform.position = new Vector3(transform.position.x + Move_camera_xy * x, transform.position.y + Move_camera_xy * y, transform.position.z);
    }
    //ライトを作成する
    void Create_light(){
        GameObject light = new GameObject("light");
        light.AddComponent<Light>();
        Light lht = light.GetComponent<Light>();
        lht.type = LightType.Directional;
    }

    //カーソルの補助
    GameObject mouse_point;
    void Create_mouse_point(){
        mouse_point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mouse_point.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    //最大で保存できるマップの数
    const int Maxmap_num = 10;
    //マップの最大サイズ
    const int Maxsize_x = 50;
    const int Maxsize_y = 50;

    //読み込んだファイルデータ
    string[] all_data;
    int map_n = 0;
    //現在読み込み中のマップ番号(x-1=n)
    int map_num = 0;
    //現在選択中のブロック
    int now_block = 0;
    //マップの情報を格納
    int[,,] map;

    //int型の桁数を調べる
    int Int_length(int n){
        return (int)Mathf.Log10(n) + 1;
    }

    //配列の確保
    void First_setting(){
        map = new int[Maxmap_num, Maxsize_y, Maxsize_x];
    }
    //配列を再度確保しなおす
    void Reset_setting(){

    }
    //読み込んだデータから空白を消す
    void Clear_readdata(){
        for (int i = 0; i < all_data.Length; i++){
            while (all_data[i].Substring(0, 1) == " "){
                all_data[i] = all_data[i].Substring(1);
            }
        }
    }
    //マップ本体を配列に変換する
    void Read_M_line(int n){
        for (int i = 0; i < n; i++){
            //変換するためのデータを入れるとこ
            string[] str1 = new string[Maxsize_y];
            for (int lu = 0; lu < Maxsize_y; lu++){

                str1[lu] = all_data[lu + Maxsize_y * i + 1];

                //文字列をばらして配列に入れる
                string[] str3 = str1[lu].Split(',');
                //文字列をint型に変換してマップに書き込み
                for (int na = 0; na < Maxsize_x; na++){
                    map[i, lu, na] = int.Parse(str3[na]);
                }
            }
        }
    }

    //デバッグ用
    void Debug_mapdata(){
        //マップの最大サイズを表示
        Debug.Log("Maxsize_x:" + Maxsize_x + "　Maxsize_y:" + Maxsize_y);
        //保存されているマップの数を表示
        Debug.Log("map_n:" + map_n);
        //保存されているマップのマップ番号と各大きさとマップを表示
        for (int i = 0; i < map_n; i++){
            for (int lu = 0; lu < Maxsize_y; lu++){
                string str1 = "";
                for (int na = 0; na < Maxsize_x; na++){
                    str1 = str1 + " " + map[i, lu, na];
                }
                Debug.Log(str1);
            }
        }
    }

    //ファイルがなかった時の初期設定
    void Setup_firstmapdata(){
        map_num = 0;
        map_n = 1;
    }
    //読み込んだファイルデータをもとに現在保存されてるマップ情報を作成する
    void Set_mapdata(){
        //テキストデータの順序
        //1行目マップの数、
        map_n = int.Parse(all_data[0]);
        if (map_n > Maxmap_num) { map_n = Maxmap_num; }
        //２～以降配列データ
        Read_M_line(map_n);
    }
    //マップの範囲がわかるように枠を表示する
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
    //マップ情報をもとにマップを作成する
    void Create_Map(){
        GameObject mapmother = new GameObject("mapchip");
        for (int i = 0; i < Maxsize_y; i++){
            for (int lu = 0; lu < Maxsize_x; lu++){
                if (map[map_num, i, lu] == 1){
                    GameObject subgo = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    subgo.transform.position = new Vector3(lu, -i, 0);
                    subgo.name = "chip_" + i + "_" + lu;
                    subgo.transform.parent = mapmother.transform;
                }
            }
        }
    }
    //マップを一括削除する
    void Delete_Map(){
        GameObject deathmother = GameObject.Find("mapchip");
        Destroy(deathmother.gameObject);
    }
    //ファイルを一括で読み込む
    void Read_data(){
        all_data = File.ReadAllLines(fipath, Encoding.GetEncoding("Shift_JIS"));
        Debug.Log(all_data.Length);
        if (all_data.Length != 0){
            Debug.Log("読み込み完了");
            //Clear_readdata();
            Set_mapdata();
            Debug_mapdata();
            Create_Map();
        }
        else{
            Debug.Log("マップデータが存在しません");
            Setup_firstmapdata();
            Create_Map();
        }
        Debug.Log("初期処理終了");
    }

    //ファイルに書き出す（すべて上書き）
    //ゲーム開始時にフォルダとファイルがあることは確認済みのため省略
    void Write_all_data(){
        Debug.Log("ファイルに書き出し");
        //ファイルに書き出す行数を確定
        
        int Write_num = Maxsize_y * map_n + 1;
        
        string[] write_str = new string[Write_num];

        //マップの数、マップの大きさを確定
        write_str[0] = "" + map_n;

        //マップ情報を確定

        for (int i = 0; i < map_n; i++){
            int subint = Maxsize_y * i;
            for (int lu = 0; lu < Maxsize_y; lu++){
                string sub_x = "";
                for (int na = 0; na < Maxsize_x; na++){
                    sub_x = sub_x + map[i, lu, na] + ",";
                }
                sub_x = sub_x.Substring(0, sub_x.Length - 1);
                write_str[subint + lu + 1] = sub_x;
            }
        }
        //テキストに書き出し
        File.WriteAllLines(fipath, write_str);
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
    string text_num = "0";
    //各最大入力数
    int max_text_num;
    //最大入力数を設定
    void GUI_setting(){
        max_text_num = Int_length(Maxmap_num);
    }
    //入力処理を書き換え
    void Set_text_num(int n){
        text_num = "" + n;
    }
    //入力処理の確定
    void Set_text_num(){
        //同じマップを指定した場合大きさを変える
        //違うマップを指定された場合移動するだけ
        int sub_n = 0;
        try{
            sub_n = int.Parse(text_num);
        }
        catch{
            Debug.LogError("文字列の変換に失敗しました、マップ番号を確認してください");
            return;
        }
        if(map_num != sub_n){
            Delete_Map();
            map_num = sub_n;
            Create_Map();
        }
    }
    //UIをプログラムから作成
    void OnGUI(){
        //画面の比率に応じてUIのサイズが変わるようにする
        float widthsize = (float)Screen.width / 1280;
        float heightsize = (float)Screen.height / 720;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(widthsize, heightsize, 1));
        //テキストの設定を作成
        GUIStyle Set_text = GUI.skin.label;
        Set_text.fontSize = Def_fontsize;
        //テキストUIを表示
        GUI.Label(new Rect(20, 20, 250, 40), "現在のマップ：" + map_num);
        GUI.Label(new Rect(50, 50, 250, 40), "N");
        GUI.Label(new Rect(px + (50 * px_num), 10, 100, 100), " 選択中\nブロック\n　 ▼", Set_text);
        //ボタンの設定を作成する
        GUIStyle Set_button = GUI.skin.button;
        Set_button.fontSize = Def_fontsize;
        Set_button.normal.textColor = Color.white;
        //ボタンを表示する
        if (GUI.Button(new Rect(200, 70, 40, 40), "決定")){
            Set_text_num();
        }
        if (GUI.Button(new Rect(250, 70, 40, 40), "書出")){
            Write_all_data();
        }
        Set_button.fontSize = Max_fontsize;
        if (GUI.Button(new Rect(px + (50 * 0), 60, 50, 50), "空白")){
            Set_UIpos(0);
            now_block = 0;
        }
        if (GUI.Button(new Rect(px + (50 * 1), 60, 50, 50), "四角")){
            Set_UIpos(1);
            now_block = 1;
        }
        //入力テキストの設定を作成
        GUIStyle Set_textfield = GUI.skin.textField;
        Set_textfield.fontSize = Max_fontsize;
        //入力テキストを作成
        text_num = GUI.TextField(new Rect(50, 70, 40, 40), text_num, max_text_num);
    }

    void Awake(){
        Set_path();
        Folder_file_check_andCreate();
        Create_light();
        Set_camera_z();
        First_setting();
        Read_data();
        GUI_setting();
        Set_text_num(map_num);
        Create_mouse_point();
        Create_Mapgrid();
    }
    void Start(){
    }

    //マウスの処理
    void Mouse_task(){
        //マウスの位置を取得
        Vector3 sub3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(camera_z));
        //ワールド座標に変換
        Vector3 sub31 = Camera.main.ScreenToWorldPoint(sub3);
        //マップ座標に最適化
        Vector3 sub32 = new Vector3(Mathf.FloorToInt(sub31.x), Mathf.FloorToInt(sub31.y), 0);
        mouse_point.transform.position = new Vector3(sub32.x, sub32.y, -1);
        if (Input.GetMouseButton(0)){
            //Debug.Log(sub3);
            //Debug.Log(sub31);
            //Debug.Log((int)sub32.x + " " + (int)sub32.y);
            //範囲外を除外
            if (sub32.x >= 0 && sub32.x < Maxsize_x && -sub32.y >= 0 && -sub32.y < Maxsize_y){
                //mapに確認
                if (map[map_num, (int)-sub32.y, (int)sub32.x] != now_block){
                    switch (now_block){
                        case 0:
                            GameObject dego = GameObject.Find("chip_" + (int)-sub32.y + "_" + (int)sub32.x);
                            Destroy(dego.gameObject);
                            map[map_num, (int)-sub32.y, (int)sub32.x] = 0;
                            break;
                        default:
                            GameObject subgo = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            subgo.name = "chip_" + (int)-sub32.y + "_" + (int)sub32.x;
                            subgo.transform.position = new Vector3((int)sub32.x, (int)sub32.y, 0);
                            GameObject mother = GameObject.Find("mapchip");
                            subgo.transform.parent = mother.transform;
                            map[map_num, (int)-sub32.y, (int)sub32.x] = now_block;
                            break;
                    }
                }
            }
        }
    }

    void Update(){
        Mouse_task();
        Set_camera();
    }
}
