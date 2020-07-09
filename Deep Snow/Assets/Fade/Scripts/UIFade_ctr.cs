using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFade_ctr : MonoBehaviour
{
    [SerializeField] Fade fade = null;

    // Start is called before the first frame update
    void Start()
    {
        fade.FadeIn(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            fade.FadeOut(1.5f);
        }
    }
}
