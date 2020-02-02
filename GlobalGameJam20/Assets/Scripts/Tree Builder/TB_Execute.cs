using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TB_Execute : MonoBehaviour
{
    public string[] allHats = new string[] { "papst", "soviet", "geldgeil", "latex", "america", "blumen", "heilig" };

    private enum ExecState
    {
        QUESTION, QUESTION_WAIT, ACTION, ACTION_WAIT, 
    }

    public bool RUN = false;

    public NewspaperFlyer newsPaperFlyer;

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
    private SpriteFinder[] spriteFinders = null;

    public void StartGame()
    {
        RUN = true;
    }

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
        spriteFinders = GameObject.FindObjectsOfType<SpriteFinder>();
    }

    private bool runningSpeakAnim = false;
    private bool runningSpeakAnimLeftPerson = true;
    private string runningSpeakAnimName = "";

    public static bool isRunning = false;
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
        if (RUN == false && isRunning)
        {
            isRunning = false;
        }
        if (RUN && wasRunning == false)
        {
            resetAllGlows();

            curNode = startQuestion;
            curNode.IsGlowing = true;

            state = ExecState.QUESTION;
            wasRunning = true;

            //isRunning = true;
        }
        else if (RUN && wasRunning)
        {
            switch (state)
            {
                case ExecState.QUESTION:
                    
                    curNode.IsGlowing = true;
                    if (curNode.endingIndex != -1)
                    {
                        newsPaperFlyer.Fly(curNode.endingIndex);
                    }
                    else
                    {
                        if (curNode.isFactorNode)
                        {
                            Debug.Log("Factor question node");
                            Debug.Log("Factors: " + StaticParameters.factorMoneyFamily.ToString() + ", " + StaticParameters.factorIntimicy + ", " + StaticParameters.factorConvProg + ", " + StaticParameters.factorTrustFun);

                            string[] toAsk = curNode.Questions;
                            float maxVal = Mathf.Max(StaticParameters.factorConvProg, StaticParameters.factorIntimicy, StaticParameters.factorMoneyFamily, StaticParameters.factorTrustFun);
                            if (maxVal == StaticParameters.factorConvProg)
                            {
                                AnswerQuestion("ConvProg");
                            }
                            else if (maxVal == StaticParameters.factorMoneyFamily)
                            {
                                AnswerQuestion("MoneyFamily");
                            }
                            else if (maxVal == StaticParameters.factorIntimicy)
                            {
                                AnswerQuestion("Intimicy");
                            }
                            else
                            {
                                AnswerQuestion("TrustFun");
                            }

                        }
                        else
                        {
                            string[] toAsk = Utils<string>.Mix(curNode.Questions);
                            AskQuestion(toAsk);

                            Debug.Log("Asking questions: " + toAsk.ToString());

                            state = ExecState.QUESTION_WAIT;



                            // H_ATS
                            if (curNode.hat.Length > 0)
                            {
                                bool foundTarget = false;
                                for (int i = 0; i < animationFinders.Length; i++)
                                {
                                    if (animationFinders[i].animatorName == "LeftPerson")
                                    {
                                        foundTarget = true;

                                        if (actionAnimation.Duration == -2f)
                                        {
                                            animationFinders[i].GetComponent<Animator>().SetBool(actionAnimation.AnimationClipName, false);
                                        }
                                        else if (actionAnimation.Duration == -1f)
                                        {
                                            animationFinders[i].GetComponent<Animator>().SetBool("hat_" + curNode.hat, true);
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
                            }
                        }

                    }

                    break;
                case ExecState.ACTION:

                    if (curAction != null)
                    {
                        curAction.IsGlowing = false;
                    }
                    if (curNode != null)
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
                            Debug.Log("Action Music Layer");
                            TB_ActionMusicLayer actionMusicLayer = (TB_ActionMusicLayer)curAction;

                            musicController.PlayLayers(actionMusicLayer.MusicLayerProps);
                        }
                        else if (curAction.GetType() == typeof(TB_ActionSpeak))
                        {
                            Debug.Log("Action Speak");
                            TB_ActionSpeak actionSpeak = (TB_ActionSpeak)curAction;

                            if (actionSpeak.SpeakAnimation.Length > 0)
                            {
                                for (int i = 0; i < animationFinders.Length; i++)
                                {
                                    if (animationFinders[i].animatorName == (actionSpeak.PersonSpeakIndex == 0 ? "Left" : "Right") + "Person")
                                    {
                                        animationFinders[i].GetComponent<Animator>().SetBool(actionSpeak.SpeakAnimation, true);
                                        runningSpeakAnim = true;
                                        runningSpeakAnimLeftPerson = actionSpeak.PersonSpeakIndex == 0;
                                        runningSpeakAnimName = actionSpeak.SpeakAnimation;
                                    }
                                }
                            }

                            interactor.ShowDialogText(actionSpeak.SpeakText, actionSpeak.PersonSpeakIndex == 0, actionSpeak.SpeakClip);
                        }
                        else if (curAction.GetType() == typeof(TB_ActionSound))
                        {
                            Debug.Log("Action Sound");
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
                            Debug.Log("Action Animation");
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
                        else if (curAction.GetType() == typeof(TB_ActionSprite))
                        {
                            TB_ActionSprite actionSprite = (TB_ActionSprite)curAction;

                            bool foundTarget = false;
                            for (int i = 0; i < spriteFinders.Length; i++)
                            {
                                if (spriteFinders[i].spriteName == actionSprite.SpriteName)
                                {
                                    foundTarget = true;

                                    SpriteRenderer[] children = spriteFinders[i].GetComponentsInChildren<SpriteRenderer>();
                                    GameObject spriteFader = new GameObject("Sprite Fader");
                                    spriteFader.AddComponent<SpriteFader>();
                                    spriteFader.GetComponent<SpriteFader>().fadeTime = 5f;
                                    spriteFader.GetComponent<SpriteFader>().fadeToSprite = actionSprite.Sprite;
                                    spriteFader.GetComponent<SpriteFader>().spriteRenderer = children[0].sprite == null ? children[1] : children[0];
                                    spriteFader.GetComponent<SpriteFader>().spriteRendererDest = children[1].sprite == null ? children[1] : children[0];
                                    spriteFader.GetComponent<SpriteFader>().StartAnim();

                                    //spriteFinders[i].GetComponent<SpriteRenderer>().sprite = actionSprite.Sprite;
                                }
                            }

                            if (!foundTarget)
                            {
                                Debug.LogError("Didn't find sprite target. Is a SpriteFinder attached to the SpriteRenderer object?");
                            }
                        }


                        state = ExecState.ACTION_WAIT;
                    }

                    // Was last action
                    else
                    {
                        Debug.Log("Last Action");
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
                bool foundTag = false;
                for (int j = 0; j < curNode.OutDialogues.Length; j++)
                {
                    if (curNode.OutDialogues[j].DiagTopTag == i.ToString())
                    {
                        curDiag = curNode.OutDialogues[j];
                        if (curNode.factors.Length > i)
                        {
                            StaticParameters.factorMoneyFamily += curNode.factors[i].moneyFamily;
                            StaticParameters.factorConvProg += curNode.factors[i].convProg;
                            StaticParameters.factorIntimicy += curNode.factors[i].intimicy;
                            StaticParameters.factorTrustFun += curNode.factors[i].trustFun;
                        }
                        actionIndex = 0;
                        state = ExecState.ACTION;
                        foundTag = true;
                        break;
                    }
                }

                if (!foundTag)
                {
                    Debug.LogError("Didnt find following dialog");
                }

                break;
            }
        }
    }

    public void ContinueClick()
    {
        if (state == ExecState.ACTION_WAIT)
        {
            if (runningSpeakAnim)
            {
                for (int i = 0; i < animationFinders.Length; i++)
                {
                    if (animationFinders[i].animatorName == (runningSpeakAnimLeftPerson ? "Left" : "Right") + "Person")
                    {
                        animationFinders[i].GetComponent<Animator>().SetBool(runningSpeakAnimName, false);
                        runningSpeakAnim = false;
                    }
                }
            }



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