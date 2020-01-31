using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TB_Questions : MonoBehaviour
{
    [SerializeField]
    private string[] outDialogues;

    private List<TB_Dialog> outTBDialogues = new List<TB_Dialog>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterDialog(TB_Dialog dialog)
    {
        if (outTBDialogues.Contains(dialog) == false)
        {
            outTBDialogues.Add(dialog);
        }

        updateOutDiagStringArray();
    }

    public void DeRegisterDialog(TB_Dialog dialog)
    {
        outTBDialogues.Remove(dialog);

        updateOutDiagStringArray();
    }

    private void updateOutDiagStringArray()
    {/*
        outDialogues = new string[outTBDialogues.Count];
        for (int i = 0; i < outTBDialogues.Count; i++)
        {
            outDialogues[i] = outTBDialogues[i].name;
            outTBDialogues[i].DiagTopTag = i.ToString();
        }*/
    }
}
