using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextProgressiveWriter : MonoBehaviour
{
    private float s = 0f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        s += Time.deltaTime;
        while (s >= timePerCiffer)
        {
            s -= timePerCiffer;

            if (goalText.Length > 0)
            {
                textMesh.text += goalText.Substring(0, 1);
                goalText = goalText.Substring(1);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public string goalText;
    public TextMeshProUGUI textMesh;
    public float timePerCiffer = 0.1f;
}
