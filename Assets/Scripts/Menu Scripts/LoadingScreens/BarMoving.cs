using System;
using UnityEngine;

public class BarMoving : MonoBehaviour
{
    private RectTransform _rectTransform;
    public float loadSpeed = 0.5f;

    public float currentProg = 0.0f;
    private void Start()
    {
        SetProgress(0f);
        
    }

    private void Update()
    {
        if (currentProg < 1f)
        {
            currentProg += Time.deltaTime * loadSpeed;
            SetProgress(currentProg);
        }
    }

    public void SetProgress(float prog)
    {
        prog = Mathf.Clamp01(prog);
        _rectTransform.localScale = new Vector3(prog, 1, 1);
    }
    
}
