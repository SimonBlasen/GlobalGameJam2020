using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewspaperFlyer : MonoBehaviour
{
    [SerializeField]
    private Transform goal;
    [SerializeField]
    private Transform start;
    [SerializeField]
    private float flyTime = 1f;
    [SerializeField]
    private AnimationCurve curveRotation;
    [SerializeField]
    private AnimationCurve curveScale;
    [SerializeField]
    private Material[] newsPapersTextures;

    private MeshRenderer meshRenderer;

    private bool flying = false;
    private float s = 0f;

    private Vector3 startScale = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        startScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (flying)
        {
            s += Time.deltaTime / flyTime;

            s = Mathf.Clamp(s, 0f, 1f);

            transform.position = Vector3.Lerp(start.position, goal.position, s);
            transform.rotation = goal.rotation * Quaternion.Euler(0f, curveRotation.Evaluate(s), 0f);
            transform.localScale = new Vector3(curveScale.Evaluate(s) * startScale.x, curveScale.Evaluate(s) * startScale.y, curveScale.Evaluate(s) * startScale.z);
        }
    }

    public void Fly(int newspaperIndex)
    {
        flying = true;

        meshRenderer.sharedMaterial = newsPapersTextures[newspaperIndex];

        transform.position = start.position;
    }
}
