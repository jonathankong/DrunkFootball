using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class IntReference
{
    public bool UseConstant = false;
    public int ConstantValue;
    public IntVariable Variable;

    //public bool UseConstant { get; private set; }
    //public float ConstantValue { get; private set; }
    //public FloatVariable Variable { get; private set; }

    public float Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}