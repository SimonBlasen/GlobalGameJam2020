using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TB_Execute : MonoBehaviour
{
    private enum ExecState
    {
        QUESTION, QUESTION_WAIT, ACTION, ACTION_WAIT, 
    }

    public bool RUN = false;

    [SerializeField]
    private Interactor interactor;

    private bool wasRunning = false;
    private TB_Questions startQuestion = null;

    private TB_Questions curNode = null;
    private TB_Dialog curDiag = null;
    private int actionIndex = 0;
    private TB_Action curAction = null;

    private float waitTime = 0f;
    private ExecState state = ExecState.QUESTION;


    private MusicController musicController = null;

    private void Start()
    {
        musicController = GameObject.FindObjectOfType<MusicController>();
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

            state = ExecState.QUESTION;
            wasRunning = true;
        }
        else if (RUN && wasRunning)
        {
            switch (state)
            {
                case ExecState.QUESTION:

                    string[] toAsk = Utils<string>.Mix(curNode.Questions);
                    AskQuestion(toAsk);

                    state = ExecState.QUESTION_WAIT;

                    break;
                case ExecState.ACTION:

                    if (curAction != null)
                    {
                        curAction.IsGlowing = false;
                    }
                    else if (curNode != null)
                    {
                        curNode.IsGlowing = false;
                    }

                    if (actionIndex < curDiag.Actions.Length)
                    {
                        curAction = curDiag.Actions[actionIndex];
                        curAction.IsGlowing = true;
                        actionIndex++;

                        waitTime = curAction.Time;



                        // Actions
                        if (curAction.GetType() == typeof(TB_ActionMusicLayer))
                        {
                            TB_ActionMusicLayer actionMusicLayer = (TB_ActionMusicLayer)curAction;

                            musicController.PlayLayers(actionMusicLayer.MusicLayerProps);
                        }
                        else if (curAction.GetType() == typeof(TB_ActionSpeak))
                        {

                        }


                        state = ExecState.ACTION_WAIT;
                    }

                    // Was last action
                    else
                    {
                        curNode = curDiag.QuestionTo;
                        state = ExecState.QUESTION;
                    }

                    break;
                case ExecState.ACTION_WAIT:
                    if (waitTime != -1f)
                    {
                        waitTime -= Time.deltaTime;
                        if (waitTime <= 0f)
                        {
                            state = ExecState.ACTION;
                        }
                    }
                    break;
            }
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























    
    public void AskQuestion(string[] questions)
    {
        interactor.ShowQuestions(questions);
    }

    public void AnswerQuestion(string chosenQuestion)
    {
        for (int i = 0; i < curNode.Questions.Length; i++)
        {
            if (curNode.Questions[i] == chosenQuestion)
            {
                curDiag = curNode.OutDialogues[i];
                actionIndex = 0;
                state = ExecState.ACTION;

                break;
            }
        }
    }

    public void ContinueClick()
    {
        if (state == ExecState.ACTION_WAIT)
        {
            state = ExecState.ACTION;
        }
    }
}
