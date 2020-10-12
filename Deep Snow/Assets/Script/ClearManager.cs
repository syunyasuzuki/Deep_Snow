using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClearManager : MonoBehaviour
{
    public static bool Clear_check;

    public GameObject panel;
    public Image clear_Logo;
    public Image clear_menu;
    public Image Next;
    public Image title;

    float alpha;
    float Y = 4.0f;

    // Use this for initialization
    void Start()
    {
        alpha = 0.0f;
        clear_menu.rectTransform.localPosition = new Vector3(-30.0f, -500.0f, 0.0f);
        Next.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        title.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        panel.SetActive(false);
        Clear_check = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Clear_check = true;
        }
        if (Clear_check == true)
        {
            GameClear();
        }
    }

    public void GameClear()
    {
        panel.SetActive(true);
        Invoke("GameClear2", 1.0f);
    }

    public void GameClear2()
    {
        if (clear_Logo.rectTransform.localPosition.y <= 130)
        {
            clear_Logo.rectTransform.localPosition += new Vector3(0.0f, Y, 0.0f);
        }
        else
        {
            clear_menu.rectTransform.localPosition = new Vector3(-10.0f, -40.0f, 0.0f);
            alpha += 0.05f;
            Next.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            title.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            if (alpha >= 1.0f)
            {
                Clear_check = false;
            }
        }
    }
}
