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

    string now_scene;

    // Start is called before the first frame update
    void Start()
    {
        fade.FadeIn(0.0f);
        now_scene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            fade.FadeOut(1.0f);
        }
    }

    public void Go_Title()
    {
        SceneManager.LoadScene("Title");
    }

    public void Retry()
    {
        SceneManager.LoadScene(now_scene);
    }

    public void Back()
    {
        fade.FadeIn(1.0f);
    }
}
