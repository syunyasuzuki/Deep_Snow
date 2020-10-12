using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Image title_logo;
    [SerializeField] Image start_Button;

    [SerializeField] Image select_1;
    [SerializeField] Image select_2;
    [SerializeField] Image select_3;

    [SerializeField] AudioClip start_se;
    [SerializeField] AudioClip select_se;

    AudioSource audio;

    bool gamestart_check;
    bool select_check;

    float alpha;
    float alpha2;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

        audio.clip = start_se;

        alpha = 1.0f;
        alpha2 = 0.0f;
        gamestart_check = false;
        select_check = false;

        select_1.color = new Color(1.0f, 1.0f, 1.0f, alpha2);
        select_2.color = new Color(1.0f, 1.0f, 1.0f, alpha2);
        select_3.color = new Color(1.0f, 1.0f, 1.0f, alpha2);
    }

    // Update is called once per frame
    void Update()
    {
        if (gamestart_check == true)
        {
            GameStart();
        }

        if(select_check == true)
        {
            Invoke("OpenSelect", 0.2f);
        }
    }

    /// <summary>
    /// スタートボタンで使うメソッド
    /// </summary>
    public void StartClick()
    {
        audio.Play();
        gamestart_check = true;
    }

    /// <summary>
    /// スタートボタンが押されたら呼ばれる
    /// </summary>
    void GameStart()
    {
        alpha -= 1.0f * Time.deltaTime;
        title_logo.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        start_Button.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        if (alpha <= 0.0f)
        {
            select_check = true;
        }
    }

    /// <summary>
    /// ステージセレクトのボタンを表示する
    /// </summary>
    void OpenSelect()
    {
        audio.clip = select_se;
        alpha2 += 1.0f * Time.deltaTime;
        select_1.color = new Color(1.0f, 1.0f, 1.0f, alpha2);
        select_2.color = new Color(1.0f, 1.0f, 1.0f, alpha2);
        select_3.color = new Color(1.0f, 1.0f, 1.0f, alpha2);
    }
}
