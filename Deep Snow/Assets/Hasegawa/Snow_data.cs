using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Ist;
using System.IO;



//こちらで描画するのは積もった雪のみ

public class Snow_data : MonoBehaviour{
    //処理する範囲
    public struct Range{
        public int begin;
        public int end;
    }

    //パスの保存先
    string fopath;
    string fipath;
    //マップの情報が保存されているファイルからテキストをもらう
    string text_data;
    void Read_textdata(){
        text_data = (Resources.Load("mapdata", typeof(TextAsset)) as TextAsset).text;
    }
    //マップのパスを指定
    void Set_path(){
        fopath = Application.dataPath + @"\Resources";
        fipath = Path.Combine(fopath, "mapdata.txt");
    }
    //マップの最大サイズを指定する
    [SerializeField] [Header("マップの最大数")] int Maxmap_num = 10;
    [SerializeField] [Header("マップの最大の大きさX")] int Maxsize_x = 50;
    [SerializeField] [Header("マップの最大の大きさY")] int Maxsize_y = 50;
    [SerializeField] [Header("マップの分割数")] int Mapslice = 10;
    //読み込んだマップを保存するとこ
    int[,] map;
    //積もった雪を保存するマップ（0=0,1=雪,2=不動）
    int[] snow_map;
    int[] L_x;
    int[] L_y;
    //保存されてるマップの数
    int map_n;
    //使うマップを指定する
    [SerializeField] [Header("使うマップの番号")] int map_num = 0;
    //マップ読み込みの補助
    string[] all_data;
    //パスの指定、配列の確保
    void First_setting(){
        Read_textdata();
        map = new int[Maxmap_num, Maxsize_y * Maxsize_x];
        //現在の雪を含むマップの状態
        snow_map = new int[Maxsize_y * Maxsize_x * 10];
        L_x = new int[Maxsize_x];
        L_y = new int[Maxsize_y];
    }
    //読み込んだデータから空白を消す
    void Clear_readdata(){
        for (int i = 0; i < all_data.Length; i++){
            while (all_data[i].Substring(0, 1) == " "){
                all_data[i] = all_data[i].Substring(1);
            }
        }
    }
    //マップ番号ごとのステージの大きさを読み込む短縮
    void Read_L_size(int n){
        string[] sub_x = all_data[1].Split(',');
        string[] sub_y = all_data[2].Split(',');
        for (int i = 0; i < n; i++){
            L_x[i] = int.Parse(sub_x[i]);
            L_y[i] = int.Parse(sub_y[i]);
        }
    }
    //マップ本体を配列に変換する
    void Read_M_line(int n){
        for (int i = 0; i < n; i++){
            //変換するためのデータを入れるとこ
            string str1;
            for (int lu = 0; lu < L_y[i]; lu++){
                str1 = all_data[5 + lu + (Maxsize_y + 2) * i];
                //必要部分だけを抜き取る
                string str2 = str1.Substring(1, str1.Length - 3);
                //文字列をばらして配列に入れる
                string[] str3 = str2.Split(',');
                //文字列をint型に変換してマップに書き込み
                for (int na = 0; na < L_x[i]; na++){
                    map[i, lu * L_x[map_num] + na] = int.Parse(str3[na]);
                }
            }
        }
    }
    //雪用のデータを作成する
    void Create_snowmap(){
        for(int i = 0; i < map.Length; ++i){
            switch (map[map_num, i]){
                case 1:
                    for(int lu = 0; lu < Mapslice; ++lu){
                        snow_map[i * Mapslice + lu] = 2;
                    }
                    break;
                case 2:
                    break;
                default:
                    break;
            }
        }
    }
    //読み込んだファイルデータをもとに現在保存されてるマップ情報を作成する
    void Set_mapdata(){
        //テキストデータの順序
        //1行目マップの数、
        map_n = int.Parse(all_data[0]);
        if (map_n > Maxmap_num) { map_n = Maxmap_num; }
        //2行目3行目、各マップの大きさ(x,y)
        Read_L_size(map_n);
        //４～以降配列データ
        Read_M_line(map_n);
    }
    //読み込んだマップを分割する
    void Map_read(){
        all_data = text_data.Split(char.Parse("\n"));
        if (all_data.Length > 0){
            Clear_readdata();
            Set_mapdata();
        }
    }

    //マップを読み込む（マップがない場合エラーとして吐き出す）
    void Map_read(int n){
        //フォルダが存在するか確認
        if (!Directory.Exists(fopath)){
            Debug.LogError("フォルダなし");
        }
        else{
            Debug.Log("フォルダあり");
        }
        //ファイルが存在するか確認
        if (!File.Exists(fipath)){
            Debug.LogError("ファイルなし");
        }
        else{
            Debug.Log("ファイルあり");
            Debug.Log("読み込み開始");
            all_data = File.ReadAllLines(fipath, System.Text.Encoding.GetEncoding("Shift_JIS"));
            if (all_data.Length > 0){
                Clear_readdata();
                Set_mapdata();
                Debug.Log("読み込み完了");
            }
            else{
                Debug.Log("マップデータがありません");
            }
        }
    }

    //マップの大きさ
    public int m_num_draw = 1024;

    BatchRenderer m_renderer;//神


    Vector3[] m_instance_t;//雪の位置を保存する
    Vector3[] m_instance_scale;//雪の大きさを保存する
    Color[] m_instance_color;//雪の色を保存する
    float m_time;//ゲームを起動してから経過した時間
    int m_num_active_tasks;//
    int player_mode = 0;

    void Awake(){
        First_setting();
        Map_read();
        //Instantiate(snow);
        m_renderer = GetComponent<BatchRenderer>();
        //int num = m_renderer.GetMaxInstanceCount();
        int num = L_x[map_num] * L_y[map_num] * Mapslice;
        Debug.Log(num);
        m_instance_t = new Vector3[num];
        m_instance_scale = new Vector3[num];
        m_instance_color = new Color[num];
        for (int i = 0; i < num; ++i){
            m_instance_t[i] = new Vector3(i % (L_x[map_num] * Mapslice) * 0.1f + 0.05f - L_x[map_num] / 2.0f, -i / (L_x[map_num] * Mapslice) + L_y[map_num] / 2.0f, 0);
            m_instance_scale[i] = new Vector3(0.1f, 1.0f, 1.0f);
            switch (snow_map[i]){
                case 0:
                    m_instance_color[i] = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    break;
                case 1:
                    m_instance_color[i] = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    break;
                case 2:
                    m_instance_color[i] = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    break;
                default:
                    m_instance_color[i] = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    break;
            }
        }
    }

    void Update(){
        m_num_draw = Mathf.Min(m_num_draw, m_instance_t.Length);
        m_time = Time.realtimeSinceStartup;
        int num = m_num_draw;{
            Interlocked.Increment(ref m_num_active_tasks);
            UpdateTask(new Range { begin = 0, end = num });
        }
        m_renderer.Flush();
    }

    void UpdateTask(System.Object arg){
        Range r = (Range)arg;
        int num = r.end - r.begin;
        {
            int reserved_index;
            int reserved_num;
            BatchRenderer.InstanceData data = m_renderer.ReserveInstance(num, out reserved_index, out reserved_num);
            System.Array.Copy(m_instance_t, r.begin, data.translation, reserved_index, reserved_num);
            System.Array.Copy(m_instance_color, r.begin, data.color, reserved_index, reserved_num);
            System.Array.Copy(m_instance_scale, r.begin, data.scale, reserved_index, reserved_num);
        }
        Interlocked.Decrement(ref m_num_active_tasks);
    }
}
