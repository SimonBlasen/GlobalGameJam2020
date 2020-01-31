using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TB_Dialog : MonoBehaviour
{
    [SerializeField]
    public string dialogName = "New Name";
    [SerializeField]
    private TB_Questions from;
    [SerializeField]
    private TB_Questions to;
    [SerializeField]
    private TB_Action[] actions;
    [SerializeField]
    private TextMeshPro textDiagIndex;

    private TB_Questions oldFrom = null;
    private TB_Questions oldTo = null;

    private Vector3 oldFromPos;
    private Vector3 oldToPos;

    private List<Transform> attachedTrans = new List<Transform>();

    private int oldActionsCount = 0;
    private Transform cube;


    private string oldName = "";
    

    // Start is called before the first frame update
    void Start()
    {
        attachedTrans.Add(textDiagIndex.transform);
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name == "Cube")
            {
                cube = children[i];
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (actions != null && oldActionsCount != actions.Length)
        {
            oldActionsCount = actions.Length;
            attachedTrans.Clear();
            attachedTrans = new List<Transform>();
            attachedTrans.Add(textDiagIndex.transform);
            for (int i = 0; i < actions.Length; i++)
            {
                attachedTrans.Add(actions[i].transform);
            }

            updateSelfRenderPos();
        }
        if (oldFrom != from)
        {
            if (oldFrom != null)
            {
                oldFrom.DeRegisterDialog(this);
            }
            
            if (from != null)
            {
                from.RegisterDialog(this);
            }

            oldFrom = from;

            updateSelfRenderPos();
        }
        if (oldTo != to)
        {
            oldTo = to;

            updateSelfRenderPos();
        }

        if (from != null)
        {
            if (oldFromPos != from.transform.position)
            {
                oldFromPos = from.transform.position;
                updateSelfRenderPos();
            }
            if (oldName != dialogName)
            {
                oldName = dialogName;
                from.UpdateOutDiagStringArray();
            }
        }
        if (to != null)
        {
            if (oldToPos != to.transform.position)
            {
                oldToPos = to.transform.position;
                updateSelfRenderPos();
            }
        }
    }

    private void updateSelfRenderPos()
    {
        if (to != null && from != null && to != from)
        {
            Vector3 posFrom = from.transform.position;
            Vector3 posTo = to.transform.position;

            transform.position = Vector3.Lerp(posFrom, posTo, 0.5f);
            transform.up = posFrom - posTo;
            cube.localScale = new Vector3(0.9f, Vector3.Distance(posFrom, posTo), 0.9f);


            Vector3 pos0 = posFrom + (posTo - posFrom).normalized * 2.5f;
            Vector3 pos1 = posTo + (posFrom - posTo).normalized * 2.5f;
            for (int i = 0; i < attachedTrans.Count; i++)
            {
                float s = 0f;
                if (attachedTrans.Count <= 1)
                {
                    s = 0f;
                }
                else
                {
                    s = i / ((float)(attachedTrans.Count));
                }

                attachedTrans[i].position = Vector3.Lerp(pos0, pos1, s) + new Vector3(0f, 0f, -1.01f);
                attachedTrans[i].rotation = Quaternion.identity;
            }
        }

    }

    public string DiagTopTag
    {
        get
        {
            return textDiagIndex.text;
        }
        set
        {
            textDiagIndex.text = value;
        }
    }
}
