using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[ExecuteInEditMode]
public class TB_ActionSpeak : TB_Action
{
    private enum Person
    {
        LEFT, RIGHT
    }
    [SerializeField]
    private Person person;
    [SerializeField]
    private AudioClip speakClip;
    [SerializeField]
    private string animation;
    [SerializeField]
    [TextArea(4, 4)]
    private string text = "";

    private string oldText = "";
    private Person oldPerson = Person.LEFT;

    private Transform cubeTransform;
    private TextMeshPro tmp;

    public string SpeakText
    {
        get
        {
            return text;
        }
    }

    public AudioClip SpeakClip
    {
        get
        {
            return speakClip;
        }
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name == "Text Sentence")
            {
                tmp = children[i].GetComponent<TextMeshPro>();
            }
            else if (children[i].name == "Cube Parent")
            {
                cubeTransform = children[i];
            }
        }
    }

    // Update is called once per frame
    new void Update()
    {
        if (!TB_Execute.isRunning)
        {
            base.Update();
            if (oldText != text)
            {
                if (tmp == null)
                {
                    Start();
                }
                oldText = text;
                tmp.text = text;
            }

            if (oldPerson != person)
            {
                if (cubeTransform == null)
                {
                    Start();
                }
                oldPerson = person;
                if (person == Person.LEFT)
                {
                    cubeTransform.localPosition = new Vector3(-2.8f * 0f, 0f, 0f);
                }
                else
                {
                    cubeTransform.localPosition = new Vector3(2.8f * 0f, 0f, 0f);
                }
                if (person == Person.LEFT)
                {
                    tmp.alignment = TextAlignmentOptions.TopLeft;
                }
                else
                {
                    tmp.alignment = TextAlignmentOptions.TopRight;
                }
            }
        }
        
    }

    public string SpeakAnimation
    {
        get
        {
            return animation;
        }
    }

    public int PersonSpeakIndex
    {
        get
        {
            return person == Person.LEFT ? 0 : 1;
        }
        set
        {
            if (value == 0)
            {
                person = Person.LEFT;
            }
            else
            {
                person = Person.RIGHT;
            }
        }
    }
}
