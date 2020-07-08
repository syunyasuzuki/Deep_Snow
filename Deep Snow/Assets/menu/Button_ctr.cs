using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//継承用スクリプト

public class Button_ctr : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //ボタンを押したら呼び出すメソッド
    public void Go_select()
    {
        //セレクトシーンに移動
        SceneManager.LoadScene("SelectScene");
        Time.timeScale = 1.0f;
    }

    //ボタンを押したら呼び出すメソッド
    public void Go_Title()
    {
        //タイトルシーンに移動
        SceneManager.LoadScene("TitleScene_E");
        Time.timeScale = 1.0f;
    }

    //ボタンを押したら呼び出すメソッド
    public void Back()
    {
        //ゲームに戻る
        GetComponent<GameManager>().menu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    //ボタンを押したら呼び出すメソッド
    //public void menu()
    //{
    //    //メニューを表示
    //    GetComponent<GameManager>().menu.SetActive(true);
    //    Time.timeScale = 0.0f;
    //}

    //-----------------------GameScene---------------------------//
    //ボタンを押したら呼び出すメソッド
    public void Game1()
    {
        //ステージ1-1に移動
        SceneManager.LoadScene("Stage1");
        Time.timeScale = 1.0f;
    }

    //ボタンを押したら呼び出すメソッド
    public void Game2()
    {
        //ステージ1-2に移動
        SceneManager.LoadScene("Stage2");
        Time.timeScale = 1.0f;
    }

    //ボタンを押したら呼び出すメソッド
    public void Game3()
    {
        //ステージ1-3に移動
        SceneManager.LoadScene("Stage3");
        Time.timeScale = 1.0f;
    }

    public void Game4()
    {
        SceneManager.LoadScene("Stage4");
        Time.timeScale = 1.0f;
    }

    public void Game5()
    {
        SceneManager.LoadScene("stage99");
        Time.timeScale = 1.0f;
    }
    //--------------------------------------------------------------//
}
