using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public float fadeTime = 2f;

    private AudioSource[] sources = null;

    private MusicLayers musicLayers = null;

    private float[] volumesDest = null;

    // Start is called before the first frame update
    void Start()
    {
        musicLayers = GameObject.FindObjectOfType<MusicLayers>();
        sources = new AudioSource[musicLayers.layerClips.Length];

        volumesDest = new float[sources.Length];

        for (int i = 0; i < sources.Length; i++)
        {
            GameObject aSource = new GameObject("MusicLayer_" + i.ToString());
            aSource.AddComponent<AudioSource>();
            sources[i] = aSource.GetComponent<AudioSource>();
            sources[i].playOnAwake = true;
            sources[i].volume = 0f;
            sources[i].loop = true;
            sources[i].clip = musicLayers.layerClips[i];
            sources[i].Play();

            volumesDest[i] = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].volume = Vector2.MoveTowards(new Vector2(sources[i].volume, 0f), new Vector2(volumesDest[i], 0f), Time.deltaTime / fadeTime).x;
        }
    }


    public void PlayLayers(TB_ActionMusicLayer.MusicLayerProp[] musicLayerProps)
    {
        for (int i = 0; i < musicLayerProps.Length; i++)
        {
            if (musicLayerProps[i].onOff)
            {
                volumesDest[i] = musicLayerProps[i].volume * 0.35f;
            }
            else
            {
                volumesDest[i] = 0f;
            }
        }
    }
}
