using Pwe.Core;
using System;
using UnityEngine;

public class HintsManager : MonoBehaviour
{
    [SerializeField] GameObject hint;
    [SerializeField] Transform hintsContainer;
    Vector2 hintPos;
    float dist = 1;
    [SerializeField] float offset = 120;

    private void Awake()
    {
        Events.OnHint += OnHint; 
        SetOff();
    }
    private void OnDestroy()
    {
        Events.OnHint -= OnHint;
    }
    private void OnHint(Vector2 pos)
    {
        hint.SetActive(true);
        hintPos = pos;
        //GameObject go = Instantiate(hint, hintsContainer);
        hint.transform.position = pos; 
    }
    private void Update()
    {
        if (hintPos == Vector2.zero) return;
        if (Input.GetMouseButtonDown(0))
        {
            float dist = Vector2.Distance(Input.mousePosition, hintPos);
            if (dist < offset)
                SetOff();
        }
    }
    void SetOff()
    {
        hint.SetActive(false);
        hintPos = Vector2.zero; 
    }
}
