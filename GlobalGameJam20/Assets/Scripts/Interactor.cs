using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private MouseColliderTrigger mct;
    [SerializeField]
    private TextMeshProUGUI[] questionsTexts;
    [SerializeField]
    private TB_Execute tB_Execute;

    // Start is called before the first frame update
    void Start()
    {
        mct.MouseClicked += Mct_MouseClicked;
    }

    private void Mct_MouseClicked()
    {
        tB_Execute.ContinueClick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDialogText(string text, bool personLeft)
    {

    }

    public void QuestionClicked(int index)
    {
        mct.GetComponent<Image>().enabled = true;

        if (index < askedQuestions.Length)
        {
            tB_Execute.AnswerQuestion(askedQuestions[index]);
        }
        else
        {
            Debug.Log("Clicked empty question");
        }
    }

    private string[] askedQuestions;

    public void ShowQuestions(string[] questions)
    {
        askedQuestions = questions;
        mct.GetComponent<Image>().enabled = false;
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
