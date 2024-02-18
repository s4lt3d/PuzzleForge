using System.Collections.Generic;
using UnityEngine;

public enum AnimatorParameterTypes
{
    Bool,
    Float,
    Int,
    Trigger 
}

[System.Serializable] // This makes AnimatorParameters visible in the Inspector
public class AnimatorParameters
{
    public string eventName;
    public AnimatorParameterTypes parameterType;
    public string animatorParameterName;
    public bool boolValue;
    public int intValue;
    public float floatValue;
}

/// <summary>
/// Helps set animator functions with multiple parameters from unity events in the inspector without code. 
/// </summary>
public class PuzzleForgeAnimatorHelper : MonoBehaviour
{
    public List<AnimatorParameters> parameters;
    
    protected Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        InitializeAnimatorParameters();
    }

    private void InitializeAnimatorParameters()
    {
        
        foreach (var param in parameters)
        {
            switch (param.parameterType)
            {
                case AnimatorParameterTypes.Float:
                    SetFloat(param.animatorParameterName, param.floatValue);
                    break;
                case AnimatorParameterTypes.Int:
                    SetInt(param.animatorParameterName, param.intValue);
                    break;
                case AnimatorParameterTypes.Bool:
                    SetBool(param.animatorParameterName, param.boolValue);
                    break;
                case AnimatorParameterTypes.Trigger:

                    break;
            }
        }
    }

    public void SetAnimatorParameter(string eventName)
    {
        var param = parameters.Find(p => p.eventName == eventName);
        if (param == null) 
            return; 

        switch (param.parameterType)
        {
            case AnimatorParameterTypes.Float:
                SetFloat(param.animatorParameterName, param.floatValue);
                break;
            case AnimatorParameterTypes.Int:
                SetInt(param.animatorParameterName, param.intValue);
                break;
            case AnimatorParameterTypes.Bool:
                SetBool(param.animatorParameterName, param.boolValue);
                break;
            case AnimatorParameterTypes.Trigger:
                SetTrigger(param.animatorParameterName);
                break;
        }
    }

    private void SetFloat(string parameterName, float value)
    {
        if (animator != null)
        {
            animator.SetFloat(parameterName, value);
        }
    }

    private void SetInt(string parameterName, int value)
    {
        if (animator != null)
        {
            animator.SetInteger(parameterName, value);
        }
    }

    private void SetBool(string parameterName, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(parameterName, value);
        }
    }

    private void SetTrigger(string parameterName)
    {
        if (animator != null)
        {
            animator.SetTrigger(parameterName);
        }
    }
    
}
