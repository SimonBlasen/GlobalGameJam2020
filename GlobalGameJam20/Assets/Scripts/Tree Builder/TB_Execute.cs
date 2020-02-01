using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TB_Execute : MonoBehaviour
{
    public bool RUN = false;

    private bool wasRunning = false;
    private TB_Questions startQuestion = null;

    private TB_Questions curNode = null;

    private void Start()
    {
        TB_Questions[] questions = GameObject.FindObjectsOfType<TB_Questions>();
        for (int i = 0; i < questions.Length; i++)
        {
            if (questions[i].IsStartNode)
            {
                startQuestion = questions[i];
                break;
            }
        }
    }

    private void Update()
    {
        if (RUN && wasRunning == false)
        {
            resetAllGlows();

            curNode = startQuestion;
            curNode.IsGlowing = true;

            wasRunning = true;
        }
        else if (RUN && wasRunning)
        {

        }
    }


    private void resetAllGlows()
    {
        TB_Questions[] questions = GameObject.FindObjectsOfType<TB_Questions>();
        for (int i = 0; i < questions.Length; i++)
        {
            if (questions[i].IsStartNode)
            {
                questions[i].IsGlowing = false;
            }
        }
    }
}
