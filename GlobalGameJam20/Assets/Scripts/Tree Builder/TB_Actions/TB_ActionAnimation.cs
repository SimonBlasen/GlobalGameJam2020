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
    
    private string oldName = "";
    private string oldTarget = "";
    
    private TextMeshPro tmp;
    private TextMeshPro tmp2;

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
        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (tmp2 == null || tmp == null)
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
    }
}
