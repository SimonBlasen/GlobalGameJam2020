using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
public class TB_ActionSound : TB_Action
{
    
    [SerializeField]
    private AudioClip clip;
    [SerializeField]
    private float volume;
    [Space]
    [SerializeField]
    private bool PLAY = false;

    private AudioClip oldName = null;

    private TextMeshPro tmp;#

    public AudioClip Clip
    {
        get
        {
            return clip;
        }
    }

    public float Volume
    {
        get
        {
            return volume;
        }
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name == "Text")
            {
                tmp = children[i].GetComponent<TextMeshPro>();
            }
        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (tmp == null)
        {
            Start();
        }

        if (oldName != clip)
        {
            oldName = clip;
            tmp.text = clip.name;
        }

        if (PLAY)
        {
            PLAY = false;/*
            GetComponentInChildren<AudioSource>().clip = clip;
            GetComponentInChildren<AudioSource>().volume = 1f;
            GetComponentInChildren<AudioSource>().loop = false;
            GetComponentInChildren<AudioSource>().Play();*/
            PublicAudioUtil.PlayClip(clip);
        }
    }
}


public static class PublicAudioUtil
{

    public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
    {
        System.Reflection.Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
        System.Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
        System.Reflection.MethodInfo method = audioUtilClass.GetMethod(
            "PlayClip",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
            null,
            new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
            null
        );
        method.Invoke(
            null,
            new object[] { clip, startSample, loop }
        );
    }

} // class PublicAudioUtil