using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private string personLeftName;
    [SerializeField]
    private string personRightName;
    [SerializeField]
    private MouseColliderTrigger mct;
    [SerializeField]
    private TextMeshProUGUI[] questionsTexts;
    [SerializeField]
    private TextMeshProUGUI speakText;
    [SerializeField]
    private TB_Execute tB_Execute;
    [SerializeField]
    private GameObject speakTextPanel;
    [SerializeField]
    private GameObject questionsPanel;

    // Start is called before the first frame update
    void Start()
    {
        mct.MouseClicked += Mct_MouseClicked;
    }

    private void Mct_MouseClicked()
    {
        tB_Execute.ContinueClick();
        speakTextPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDialogText(string text, bool personLeft, AudioClip clip = null)
    {
        speakTextPanel.SetActive(true);
        //speakText.text = (personLeft ? personLeftName : personRightName) +  ": " + text;
        GameObject tpw = new GameObject("Text Progressive Writer");
        tpw.AddComponent<TextProgressiveWriter>();
        tpw.GetComponent<TextProgressiveWriter>().textMesh = speakText;
        tpw.GetComponent<TextProgressiveWriter>().timePerCiffer = 0.05f;
        tpw.GetComponent<TextProgressiveWriter>().goalText = (personLeft ? personLeftName : personRightName) + ": " + text;


        if (clip != null)
        {
            GameObject gamAudioSource = new GameObject("Sound Speaker");
            gamAudioSource.AddComponent<AudioSource>();
            AudioSource audioSource = gamAudioSource.GetComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = 0.8f;
            audioSource.loop = false;
            audioSource.Play();

            Destroy(gamAudioSource, clip.length + 2f);
        }
    }

    public void QuestionClicked(int index)
    {
        //mct.GetComponent<Image>().enabled = true;

        if (index < askedQuestions.Length)
        {
            tB_Execute.AnswerQuestion(askedQuestions[index]);
        }
        else
        {
            Debug.Log("Clicked empty question");
        }

        questionsPanel.SetActive(false);
    }

    private string[] askedQuestions;

    public void ShowQuestions(string[] questions)
    {
        speakTextPanel.SetActive(false);
        questionsPanel.SetActive(true);
        askedQuestions = questions;
        //mct.GetComponent<Image>().enabled = false;
        for (int i = 0; i < questions.Length; i++)
        {
            questionsTexts[i].text = questions[i];
        }
        for (int i = questions.Length; i < questionsTexts.Length; i++)
        {
            questionsTexts[i].text = "";
        }
    }
}
