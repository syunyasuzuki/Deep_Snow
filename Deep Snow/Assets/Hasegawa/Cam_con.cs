using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_con : MonoBehaviour
{
    /// <summary>シーン開始時のカメラの位置</summary>
    [SerializeField] Vector3 position = new Vector3(0.0f, 0.0f, -10.0f);
    /// <summary>シーン開始時カメラのサイズ</summary>
    [SerializeField] float size = 5.0f;

    /// <summary>アプリを閉じる</summary>
    void Exit_game() { if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); } }


    // Start is called before the first frame update
    void Start(){
        Camera cam = Camera.main;
        cam.transform.position = position;
        cam.orthographicSize = size;
    }

    // Update is called once per frame
    void Update(){
        Exit_game();
    }
}
