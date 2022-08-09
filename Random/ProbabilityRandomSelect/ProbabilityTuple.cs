using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ProbabilityTuple<T>
{
    public T obj;
    public float probability;


    public ProbabilityTuple(T _obj, float _probability)
    {
        obj = _obj; probability = _probability;
    }
}
