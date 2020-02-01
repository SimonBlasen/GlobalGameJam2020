using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseColliderTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool mouseOver = false;
    public delegate void Clicked();
    public event Clicked MouseClicked;
    public delegate void Hovered();
    public event Hovered MouseHovered;
    public delegate void Dehovered();
    public event Dehovered MouseDehovered;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Pointer enter");
        mouseOver = true;

        MouseHovered?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Pointer exit");
        mouseOver = false;

        MouseDehovered?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Click");

        MouseClicked?.Invoke();
    }

    public bool IsHovered
    {
        get
        {
            return mouseOver;
        }
        set
        {
            mouseOver = value;
        }
    }
}
