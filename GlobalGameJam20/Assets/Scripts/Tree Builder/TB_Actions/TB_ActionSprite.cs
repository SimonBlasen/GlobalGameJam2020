using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[ExecuteInEditMode]
public class TB_ActionSprite : TB_Action
{

    [SerializeField]
    private string spriteName;
    [SerializeField]
    private Sprite sprite;

    private Sprite oldSprite = null;
    private string oldName = "";

    private SpriteRenderer spriteRenderer;
    private TextMeshPro tmp;

    public string SpriteName
    {
        get
        {
            return spriteName;
        }
    }

    public Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].name == "Sprite Renderer")
            {
                spriteRenderer = children[i].GetComponent<SpriteRenderer>();
            }
            else if (children[i].name == "Text")
            {
                tmp = children[i].GetComponent<TextMeshPro>();
            }
        }
    }

    // Update is called once per frame
    protected new void Update()
    {
        if (!TB_Execute.isRunning)
        {
            base.Update();
            if (spriteRenderer == null || tmp == null)
            {
                Start();
            }
            if (oldSprite != sprite)
            {
                oldSprite = sprite;

                spriteRenderer.sprite = sprite;
            }

            if (oldName != spriteName)
            {
                oldName = spriteName;
                tmp.text = spriteName;
            }
        }
    }
}
