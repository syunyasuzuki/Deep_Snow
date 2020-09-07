using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam_control : MonoBehaviour {
    //カメラ本体
    [SerializeField] GameObject cam;

    //カメラの引きを調整するか
    [SerializeField] bool Move_z = true;
    //カメラの引き具合の最小値最大値
    [SerializeField] float Max_camera_size = -10;
    [SerializeField] float Min_camera_size = -50;
    //カメラの引きの速度
    [SerializeField] float Move_size_spd = 10.0f;

    //カメラを移動させるか
    [SerializeField] bool Move_xy = true;
    //カメラの移動速度
    [SerializeField] float Move_xy_spd = 1.0f;

    //カメラを回転させるか
    [SerializeField] bool Rotate_cam = true;
    //カメラを回転させる速度
    [SerializeField] float Rotate_spd = 2.0f;
    //カメラを回転させる中心となる座標
    [SerializeField] Transform lookpos;
    Vector3 poso;

    private bool move_z, move_xy, rotate_cam;
    //カメラの初期設定
    void First_set(){
        //カメラ入れてなかったら探す
        if (cam == null) { cam = GameObject.Find("Main Camera"); }
        move_z = Move_z;
        move_xy = Move_xy;
        rotate_cam = Rotate_cam;
    }

    //カメラの引き
    void Set_size(){
        float fl = Input.GetAxisRaw("Mouse ScrollWheel");
        float size = cam.GetComponent<Camera>().orthographicSize + fl * Move_size_spd;
        if (size > Max_camera_size) { size = Max_camera_size; }
        if (size < Min_camera_size) { size = Min_camera_size; }
        cam.GetComponent<Camera>().orthographicSize = size;
    }
    //カメラの移動
    void Set_move(){
        int x = 0, y = 0;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { x -= 1; }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { x += 1; }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { y += 1; }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { y -= 1; }
        cam.transform.position = new Vector3(cam.transform.position.x + Move_xy_spd * x, cam.transform.position.y + Move_xy_spd * y, cam.transform.position.z);
    }
    //カメラの回転関係
    //カメラの回転のスイッチ
    void Reset_cam_r(){
        if (Input.GetKeyDown(KeyCode.K)) rotate_cam = rotate_cam ? false : true;
    }
    //カメラの回転をリセット
    void Reset_cam(){
        if (Input.GetKeyDown(KeyCode.L)){
            cam.transform.position = new Vector3(0.0f, 0.0f, -10.0f);
            cam.transform.rotation = new Quaternion(0.0f, 0.0f, -10.0f, 0.0f);
            cam.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            if (lookpos == null) poso = new Vector3(0.0f, 0.0f, 0.0f);
            else poso = lookpos.position;
        }
    }
    Transform trans;
    Vector3 vec3;
    void Set_rotate(){
        Reset_cam_r();
        Reset_cam();
        if (!rotate_cam) return;
        Vector3 l_pos;
        if (lookpos == null) l_pos = new Vector3(0.0f, 0.0f, 0.0f);
        else l_pos = lookpos.transform.position;
        trans = cam.transform;
        vec3 = trans.position - l_pos;
        if (rotate_cam){
            vec3 = Quaternion.Euler(0.0f, Time.deltaTime * Rotate_spd, 0) * vec3;
        }
        trans.position = vec3 + l_pos;
        poso += (l_pos - poso);
        trans.transform.LookAt(poso);
    }

    void Awake(){
        First_set();
    }
	
	// Update is called once per frame
	void Update () {
        Set_size();
        Set_move();
        Set_rotate();
	}
}
