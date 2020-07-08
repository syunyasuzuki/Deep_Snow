using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : Button_ctr
{
    public GameObject menu;
    public GameObject player;

    [SerializeField] Button List1;
    [SerializeField] Button List2;
    [SerializeField] Button List3;

	// Use this for initialization
	void Start ()
    {
        menu.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(true);
            List1.Select();
            Time.timeScale = 0.0f;
        }
	}
}
