using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLayers : MonoBehaviour
{
    public AudioClip[] layerClips;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static void instantiate()
    {
        if (inst == null)
        {
            inst = GameObject.FindObjectOfType<MusicLayers>();
        }
    }

    private static MusicLayers inst = null;
    public static AudioClip[] Layers
    {
        get
        {
            instantiate();
            return inst.layerClips;
        }
    }
}
