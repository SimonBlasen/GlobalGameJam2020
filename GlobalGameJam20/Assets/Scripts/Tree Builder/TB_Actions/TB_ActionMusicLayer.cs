using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TB_ActionMusicLayer : TB_Action
{
    [System.Serializable]
    public class MusicLayerProp
    {
        [HideInInspector]
        public string clipName;
        public bool onOff;
        public float volume = 1f;
    }



    [SerializeField]
    private MusicLayerProp[] layers;

    private bool[] oldName = new bool[0];

    private TextMeshPro tmp;

    public MusicLayerProp[] MusicLayerProps
    {
        get
        {
            return layers;
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
        if (!TB_Execute.isRunning)
        {
            base.Update();
            if (tmp == null)
            {
                Start();
            }

            if (oldName.Length != MusicLayers.Layers.Length)
            {
                oldName = new bool[MusicLayers.Layers.Length];
                for (int i = 0; i < layers.Length; i++)
                {
                    oldName[i] = layers[i].onOff;
                }
            }

            if (MusicLayers.Layers.Length != layers.Length)
            {
                Debug.Log("Reset action music");
                layers = new MusicLayerProp[MusicLayers.Layers.Length];
                for (int i = 0; i < layers.Length; i++)
                {
                    layers[i] = new MusicLayerProp();
                    layers[i].clipName = MusicLayers.Layers[i].name;
                }
                oldName = new bool[MusicLayers.Layers.Length];
            }
            else
            {
                bool oneDif = false;
                for (int i = 0; i < layers.Length; i++)
                {
                    if (layers[i].onOff != oldName[i])
                    {
                        oneDif = true;
                        oldName[i] = layers[i].onOff;
                        layers[i].clipName = MusicLayers.Layers[i].name;
                    }
                }

                if (oneDif)
                {
                    if (tmp == null)
                    {
                        Start();
                    }
                    tmp.text = "";
                    for (int i = 0; i < layers.Length; i++)
                    {
                        tmp.text += (layers[i].onOff ? "(X)  " : "       ") + MusicLayers.Layers[i].name + "\n";
                    }
                }
            }
        }
    }
}
