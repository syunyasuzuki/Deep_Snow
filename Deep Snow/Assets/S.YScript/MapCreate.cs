using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class MapCreate : EditorWindow
{
    //画像ディレクトリ
    private Object imgDirectory;
    //出力先ディレクトリ
    private Object outputDirectory;
    //マップエディタのマスの数
    private int mapSize = 10;
    //グリッドの大きさ、小さいほど細かくなる
    private float gridSize = 50.0f;
    //出力ファイル名
    private string outputFileName;
    //選択した画像パス
    private string selectedImagePath;
    //サブウィンドウ
    private MapCreateSubWindow subWindow;

    [UnityEditor.MenuItem("Window/MapCreater")]
    static void ShowTestMainwindow()
    {
        //ウィンドウ表示＆取得
        EditorWindow.GetWindow(typeof(MapCreate));
    }

    void OnGUI()
    {
        // GUI
        // オブジェクト
        //横に並べて配置
        GUILayout.BeginHorizontal();
        //ラベル作成
        GUILayout.Label("Image : ", GUILayout.Width(110));
        //任意のオブジェクトの Type を表示するフィールドを作成
        imgDirectory = EditorGUILayout.ObjectField(imgDirectory, typeof(Object), true);
        //BeginHorizontal()終了
        GUILayout.EndHorizontal();
        //スペース
        EditorGUILayout.Space();

        //マップサイズ
        GUILayout.BeginHorizontal();
        GUILayout.Label("map size : ", GUILayout.Width(110));
        mapSize = EditorGUILayout.IntField(mapSize);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //グリッドサイズ
        GUILayout.BeginHorizontal();
        GUILayout.Label("grid size : ", GUILayout.Width(110));
        gridSize = EditorGUILayout.FloatField(gridSize);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //セーブファイル指定
        GUILayout.BeginHorizontal();
        GUILayout.Label("Save Directory : ", GUILayout.Width(110));
        outputDirectory = EditorGUILayout.ObjectField(outputDirectory, typeof(Object), true);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        //ファイル名指定
        GUILayout.BeginHorizontal();
        GUILayout.Label("Save filename : ", GUILayout.Width(110));
        outputFileName = EditorGUILayout.TextField(outputFileName);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        DrowImageParts();

        DrowSelectedImage();

        DrawMapWindowButton();
    }

    private void DrawImageParts()
    {
        //画像が入っているとき
        if(imgDirectory!=null)
        {
            float x = 0.0f;
            float y = 00.0f;
            float w = 50.0f;
            float h = 50.0f;
            float maxW = 300.0f;

            //画像のパスを取得
            string path = AssetDatabase.GetAssetPath(imgDirectory);
            //指定したディレクトリ内の指定した検索パターンに一致するファイル名 (パスを含む) を返す
            string[] names = Directory.GetFiles(path, "*.png");
            EditorGUILayout.BeginVertical();
            //配列やListの要素にアクセスするループ処理
            foreach(string d in names)
            {
                if(x>maxW)
                {
                    x = 0.0f;
                    y += h;
                    EditorGUILayout.EndHorizontal();
                }
                if(x==0.0f)
                {
                    EditorGUILayout.BeginHorizontal();
                }
                GUILayout.FlexibleSpace();
            }
        }
    }
}



//サブウィンドウの作成
public class MapCreateSubWindow:EditorWindow
{

}
