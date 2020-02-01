﻿using System.Collections;
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


    private List<AnimationStopper> toStopAnims = new List<AnimationStopper>();


    private MusicController musicController = null;

    private AnimationFinder[] animationFinders = null;

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

        animationFinders = GameObject.FindObjectsOfType<AnimationFinder>();
    }

    private void Update()
    {
        for (int i = 0; i < toStopAnims.Count; i++)
        {
            toStopAnims[i].stopIn -= Time.deltaTime;
            if (toStopAnims[i].stopIn <= 0f)
            {
                toStopAnims[i].animator.SetBool(toStopAnims[i].animationClip, false);
                toStopAnims.RemoveAt(i);
                i--;
            }
        }

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
                            TB_ActionSpeak actionSpeak = (TB_ActionSpeak)curAction;

                            interactor.ShowDialogText(actionSpeak.SpeakText, actionSpeak.PersonSpeakIndex == 0, actionSpeak.SpeakClip);
                        }
                        else if (curAction.GetType() == typeof(TB_ActionSound))
                        {
                            TB_ActionSound actionSound = (TB_ActionSound)curAction;

                            GameObject gamAudioSource = new GameObject("Sound " + actionSound.Clip.name);
                            gamAudioSource.AddComponent<AudioSource>();
                            AudioSource audioSource = gamAudioSource.GetComponent<AudioSource>();
                            audioSource.clip = actionSound.Clip;
                            audioSource.volume = actionSound.Volume;
                            audioSource.loop = false;
                            audioSource.Play();

                            Destroy(gamAudioSource, actionSound.Clip.length + 2f);
                        }
                        else if (curAction.GetType() == typeof(TB_ActionAnimation))
                        {
                            TB_ActionAnimation actionAnimation = (TB_ActionAnimation)curAction;

                            bool foundTarget = false;
                            for (int i = 0; i < animationFinders.Length; i++)
                            {
                                if (animationFinders[i].animatorName == actionAnimation.AnimatorTarget)
                                {
                                    foundTarget = true;

                                    if (actionAnimation.Duration == -2f)
                                    {
                                        animationFinders[i].GetComponent<Animator>().SetBool(actionAnimation.AnimationClipName, false);
                                    }
                                    else if (actionAnimation.Duration == -1f)
                                    {
                                        animationFinders[i].GetComponent<Animator>().SetBool(actionAnimation.AnimationClipName, true);
                                    }
                                    else
                                    {
                                        animationFinders[i].GetComponent<Animator>().SetBool(actionAnimation.AnimationClipName, true);

                                        AnimationStopper animStop = new AnimationStopper();
                                        animStop.animationClip = actionAnimation.AnimationClipName;
                                        animStop.animator = animationFinders[i].GetComponent<Animator>();
                                        animStop.stopIn = actionAnimation.Duration;

                                        toStopAnims.Add(animStop);
                                    }
                                }
                            }

                            if (!foundTarget)
                            {
                                Debug.LogError("Didn't find animation target. Is a AnimationFinder attached to the Animator object?");
                            }
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


public class AnimationStopper
{
    public Animator animator;
    public string animationClip = "";
    public float stopIn = 0f;
}