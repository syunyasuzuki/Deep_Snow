using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFade_ctr : MonoBehaviour
{
    [SerializeField] Fade fade = null;

    //スタートメソッドより先に起動する
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        fade.FadeOut(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
