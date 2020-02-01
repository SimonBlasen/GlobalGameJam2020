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

    private MeshRenderer cubeMeshRenderer = null;

    private Color glowDefaultColor = Color.black;

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
            else if (children[i].name == "Cube")
            {
                cubeMeshRenderer = children[i].GetComponent<MeshRenderer>();
                glowDefaultColor = cubeMeshRenderer.sharedMaterial.color;
            }
        }
    }

    protected void Update()
    {
        if (!TB_Execute.isRunning)
        {
            if (oldTime != time)
            {
                oldTime = time;
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

    public float Time
    {
        get
        {
            return time;
        }
    }

    private bool isGlowing = false;
    public bool IsGlowing
    {
        get
        {
            return isGlowing;
        }
        set
        {
            isGlowing = value;

            if (isGlowing)
            {
                Material copy = new Material(cubeMeshRenderer.sharedMaterial);
                copy.color = Color.green;
                cubeMeshRenderer.sharedMaterial = copy;
            }
            else
            {
                Material copy = new Material(cubeMeshRenderer.sharedMaterial);
                copy.color = glowDefaultColor;
                cubeMeshRenderer.sharedMaterial = copy;
            }
        }
    }
}
