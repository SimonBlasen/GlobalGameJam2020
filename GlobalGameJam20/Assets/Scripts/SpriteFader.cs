using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFader : MonoBehaviour
{
    private float s = 0f;
    private bool fadeOut = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        s += Time.deltaTime / fadeTime;
        s = Mathf.Clamp(s, 0f, 1f);

        if (fadeOut)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f - s);
            spriteRendererDest.color = new Color(spriteRendererDest.color.r, spriteRendererDest.color.g, spriteRendererDest.color.b, s);
            if (s >= 1f)
            {
                spriteRenderer.sprite = fadeToSprite;
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
                spriteRendererDest.color = new Color(spriteRendererDest.color.r, spriteRendererDest.color.g, spriteRendererDest.color.b, 0f);
                spriteRendererDest.sprite = null;
                s = 0f;
                fadeOut = false;
                Destroy(gameObject);
            }
        }
    }

    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteRendererDest;
    public Sprite fadeToSprite;
    public float fadeTime = 5f;

    public void StartAnim()
    {
        spriteRendererDest.color = new Color(spriteRendererDest.color.r, spriteRendererDest.color.g, spriteRendererDest.color.b, 0f);
        spriteRendererDest.sprite = fadeToSprite;
    }

}
