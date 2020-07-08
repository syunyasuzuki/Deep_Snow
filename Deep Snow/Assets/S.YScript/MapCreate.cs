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
    }
}

//サブウィンドウの作成
public class MapCreateSubWindow:EditorWindow
{

}
