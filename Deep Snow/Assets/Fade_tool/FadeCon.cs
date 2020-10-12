using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeCon : MonoBehaviour
{
    public Image FadeImage1;

    public static bool isFade1;
    public static bool isFadeOut1;
    public static bool isFadeIn1;

    public static float alpha1;

    // Use this for initialization
    void Start ()
    {
        alpha1 = 1.0f;

        isFade1 = true;
        isFadeIn1 = true;
        isFadeOut1 = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isFade1)
        {
            if (isFadeIn1)
            {
                FadeIn1();
            }
            else if (isFadeOut1)
            {
                FadeOut1();
            }
        }
    }

    public void FadeIn1()
    {
        alpha1 -= 0.02f;
        FadeImage1.color = new Color(1.0f, 1.0f, 1.0f, alpha1);
        if (alpha1 <= 0.0f)
        {
            isFadeIn1 = false;
            isFade1 = false;
        }
    }

    public void FadeOut1()
    {
        alpha1 += 0.02f;
        FadeImage1.color = new Color(1.0f, 1.0f, 1.0f, alpha1);
        if (alpha1 >= 1.0f)
        {
            isFadeOut1 = false;
            isFade1 = false;
        }
    }
}
