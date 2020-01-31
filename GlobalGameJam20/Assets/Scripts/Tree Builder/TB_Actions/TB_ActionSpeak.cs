﻿using System.Collections;
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
    [TextArea(4, 4)]
    private string text = "";

    private string oldText = "";
    private Person oldPerson = Person.LEFT;

    private Transform cubeTransform;
    private TextMeshPro tmp;

    // Start is called before the first frame update
    void Start()
    {
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
    void Update()
    {
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
                cubeTransform.localPosition = new Vector3(-2.8f, 0f, 0f);
            }
            else
            {
                cubeTransform.localPosition = new Vector3(2.8f, 0f, 0f);
            }
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