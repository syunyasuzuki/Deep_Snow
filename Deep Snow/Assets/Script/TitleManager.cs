using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    //--------タイトルロゴとスタートボタンのイメージを入れる変数----------
    [SerializeField] Image title_logo;
    [SerializeField] Image start_Button;
    //---------------------------------------------------------------

    //-----------ステージセレクトボタンのイメージを入れる変数------------
    [SerializeField] Image select_1;
    [SerializeField] Image select_2;
    [SerializeField] Image select_3;
    //---------------------------------------------------------------

    [SerializeField] AudioClip start_se;　　//スタートボタンを押したら鳴らすSE
    [SerializeField] AudioClip select_se;   //ステージセレクトボタンを押したら鳴らすSE

    AudioSource audio;

    bool gamestart_check;  //スタートボタンが押されたかどうかを判定する変数
    bool select_check;     //ステージセレクトボタンが押されたかどうかを判定する変数

    float alpha;   //タイトルロゴとスタートボタンのα値を変える変数
    float alpha2;  //ステージセレクトボタンのα値を変える変数

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
    /// １－１ボタンを押したらステージ１を呼び出す
    /// </summary>
    public void SelectStage1()
    {
        audio.Play();
        FadeCon.isFade1 = true;
        FadeCon.isFadeOut1 = true;
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
