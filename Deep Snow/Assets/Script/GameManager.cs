using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Fade fade = null;

    [SerializeField] Button title = null;
    [SerializeField] Button retry = null;
    [SerializeField] Button back = null;

    [SerializeField] GameObject menu;

    string now_scene;

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        fade.FadeIn(0.0f);
        now_scene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(true);
            fade.FadeOut(1.0f);
        }
    }

    public void Go_Title()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void Retry()
    {
        SceneManager.LoadScene(now_scene);
    }

    public void Back()
    { 
        fade.FadeIn(1.0f ,()=> {
            menu.SetActive(false);
        });
    }
}
