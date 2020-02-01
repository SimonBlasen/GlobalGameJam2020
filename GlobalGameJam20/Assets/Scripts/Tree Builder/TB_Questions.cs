using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TB_Questions : MonoBehaviour
{
    [SerializeField]
    [TextArea(1, 3)]
    private string question0;
    [SerializeField]
    [TextArea(1, 3)]
    private string question1;
    [SerializeField]
    [TextArea(1, 3)]
    private string question2;
    [SerializeField]
    [TextArea(1, 3)]
    private string question3;
    [SerializeField]
    private string[] outDialogues;

    private string[] oldOutDialogues;

    private string[] oldQuestions = new string[4];

    private List<string> questions = new List<string>();

    private List<TB_Dialog> outTBDialogues = new List<TB_Dialog>();

    private TextMeshPro tmpQuestions;

    // Start is called before the first frame update
    void Start()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name == "Text Questions")
            {
                tmpQuestions = children[i].GetComponent<TextMeshPro>();
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < outDialogues.Length; i++)
        {
            if (oldOutDialogues[i] != outDialogues[i])
            {
                outTBDialogues[i].DiagTopTag = outDialogues[i];
                outTBDialogues[i].dialogName = outDialogues[i];
                oldOutDialogues[i] = outDialogues[i];
            }
        }

        bool needClear = false;
        if (oldQuestions[0] != question0)
        {
            needClear = true;
            oldQuestions[0] = question0;
        }
        if (oldQuestions[1] != question1)
        {
            needClear = true;
            oldQuestions[1] = question1;
        }
        if (oldQuestions[2] != question2)
        {
            needClear = true;
            oldQuestions[2] = question2;
        }
        if (oldQuestions[3] != question3)
        {
            needClear = true;
            oldQuestions[3] = question3;
        }


        if (needClear)
        {
            needClear = false;
            questions.Clear();
            questions = new List<string>();

            if (question0.Length > 0)
            {
                questions.Add(question0);
            }
            if (question1.Length > 0)
            {
                questions.Add(question1);
            }
            if (question2.Length > 0)
            {
                questions.Add(question2);
            }
            if (question3.Length > 0)
            {
                questions.Add(question3);
            }

            if (tmpQuestions == null)
            {
                Start();
            }
            tmpQuestions.text = "";
            for (int i = 0; i < questions.Count; i++)
            {
                tmpQuestions.text += "Q" + i.ToString() + ": " + questions[i] + "\n";
            }

            Debug.Log("Amount questions: " + questions.Count);
        }

    }

    public void RegisterDialog(TB_Dialog dialog)
    {
        Debug.Log("Register. Was: " + outTBDialogues.Count);
        if (outTBDialogues.Contains(dialog) == false)
        {
            outTBDialogues.Add(dialog);
        }
        Debug.Log("Register. Is: " + outTBDialogues.Count);

        UpdateOutDiagStringArray();
    }

    public void DeRegisterDialog(TB_Dialog dialog)
    {
        Debug.Log("De-Register. Is: " + outTBDialogues.Count);
        outTBDialogues.Remove(dialog);

        UpdateOutDiagStringArray();
    }

    public void UpdateOutDiagStringArray()
    {
        for (int i = 0; i < outTBDialogues.Count; i++)
        {
            if (outTBDialogues[i] == null)
            {
                outTBDialogues.RemoveAt(i);
                i--;
            }
        }
        outDialogues = new string[outTBDialogues.Count];
        oldOutDialogues = new string[outTBDialogues.Count];
        for (int i = 0; i < outTBDialogues.Count; i++)
        {
            outDialogues[i] = outTBDialogues[i].dialogName;
            outTBDialogues[i].DiagTopTag = outTBDialogues[i].dialogName;
            oldOutDialogues[i] = outTBDialogues[i].dialogName;
        }
    }
}
