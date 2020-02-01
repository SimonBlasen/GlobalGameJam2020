using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[ExecuteInEditMode]
public class TB_ActionAnimation : TB_Action
{
    [SerializeField]
    private string animationName;
    [SerializeField]
    private string target;
    [SerializeField]
    private float duration;

    private string oldName = "";
    private string oldTarget = "";
    private float oldDuration = 0f;
    
    private TextMeshPro tmp;
    private TextMeshPro tmp2;
    private TextMeshPro tmp3;

    public string AnimationClipName
    {
        get
        {
            return animationName;
        }
    }

    public string AnimatorTarget
    {
        get
        {
            return target;
        }
    }

    public float Duration
    {
        get
        {
            return duration;
        }
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name == "Text")
            {
                tmp = children[i].GetComponent<TextMeshPro>();
            }
            else if (children[i].name == "Text 2")
            {
                tmp2 = children[i].GetComponent<TextMeshPro>();
            }
            else if (children[i].name == "Text 3")
            {
                tmp3 = children[i].GetComponent<TextMeshPro>();
            }
        }
    }

    // Update is called once per frame
    new void Update()
    {
        if (!TB_Execute.isRunning)
        {
            base.Update();
            if (tmp2 == null || tmp == null || tmp3 == null)
            {
                Start();
            }

            if (oldName != animationName)
            {
                oldName = animationName;
                tmp.text = animationName;
            }
            if (oldTarget != target)
            {
                oldTarget = target;
                tmp2.text = target;
            }
            if (oldDuration != duration)
            {
                oldDuration = duration;
                if (duration == -1f)
                {
                    tmp3.text = "Start...";
                }
                else if (duration == -2f)
                {
                    tmp3.text = "...Stop";
                }
                else
                {
                    tmp3.text = "Duration: " + duration.ToString("n2");
                }
            }
        }
        
    }
}
