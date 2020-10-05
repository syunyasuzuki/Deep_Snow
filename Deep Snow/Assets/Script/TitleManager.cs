using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Image title_logo;
    [SerializeField] Image start_Button;

    [SerializeField] Camera main_camera;

    bool gamestart_check;

    float camera_pos_y;
    float alpha;

    // Start is called before the first frame update
    void Start()
    {
        alpha = 1.0f;
        gamestart_check = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gamestart_check == true)
        {
            GameStart2();
        }
    }

    /// <summary>
    /// スタートボタンで使うメソッド
    /// </summary>
    public void GameStart()
    {
        gamestart_check = true;
    }

    /// <summary>
    /// スタートボタンが押されたら呼ばれる
    /// </summary>
    void GameStart2()
    {
        alpha -= 1.0f * Time.deltaTime;
        title_logo.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        start_Button.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        main_camera.transform.position = new Vector3(0.0f, camera_pos_y, -10.0f);
        if (alpha <= 0.0f)
        {
            camera_pos_y -= 2.0f * Time.deltaTime;
        }
    }
}
