using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class FloatReference
{
    public bool UseConstant = false;
    public float ConstantValue;
    public FloatVariable Variable;

    //public bool UseConstant { get; private set; }
    //public float ConstantValue { get; private set; }
    //public FloatVariable Variable { get; private set; }

    public float Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}