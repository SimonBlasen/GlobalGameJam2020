using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TB_Action : MonoBehaviour
{
    public float time = 0f;

    protected float oldTime = 0f;

    protected TextMeshPro timeText;

    protected void Start()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name == "Text Time")
            {
                timeText = children[i].GetComponent<TextMeshPro>();
                break;
            }
        }
    }

    protected void Update()
    {
        if (oldTime != time)
        {
            if (timeText == null)
            {
                Start();
            }
            if (time == -1f)
            {
                timeText.text = "Click";
            }
            else
            {
                timeText.text = "Time: " + time.ToString("n1");
            }
        }
    }
}
