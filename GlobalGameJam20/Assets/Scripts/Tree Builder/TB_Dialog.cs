using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TB_Dialog : MonoBehaviour
{
    [SerializeField]
    private TB_Questions from;
    [SerializeField]
    private TB_Questions to;
    [SerializeField]
    private TextMeshPro textDiagIndex;

    private TB_Questions oldFrom = null;
    private TB_Questions oldTo = null;

    private Vector3 oldFromPos;
    private Vector3 oldToPos;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (oldFrom != from)
        {
            if (oldFrom != null)
            {
                oldFrom.DeRegisterDialog(this);
            }

            from.RegisterDialog(this);

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
            transform.localScale = new Vector3(0.9f, Vector3.Distance(posFrom, posTo), 0.9f);
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
