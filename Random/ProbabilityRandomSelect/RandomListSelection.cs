using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


public static class RandomListSelection
{
    // Selects a random object taking into account the probabilities
    // associated with the elements.
    // ProbabilitySum, if passed in, will save the function from having to sum up all
    // probabilities. If null is passed in, probabilitySum will be assigned the calculated
    // sum in case caller needs to run function multiple times
    public static T SelectRandomObj<T>(IList<ProbabilityTuple<T>> pool, ref float? probabilitySum)
    {
        if(pool.Count == 0) { return default(T); }

        if(probabilitySum.HasValue == false)
        {
            probabilitySum = 0;

            foreach(ProbabilityTuple<T> p in pool)
            {
                probabilitySum += p.probability;
            }
        }

        float rand = Random.Range(0f, probabilitySum.Value);

        float tracker = 0;
        foreach(ProbabilityTuple<T> p in pool)
        {
            tracker += p.probability;

            if(tracker >= rand)
            {
                return p.obj;
            }
        }

        throw new System.Exception("SelectRandomObj should never fail");
    }


    public static T[] SelectRandomObjs<T>(IList<ProbabilityTuple<T>> pool, int numToSelect)
    {
        T[] selected = new T[numToSelect];
        float? sum = null;

        for(int i = 0; i < numToSelect; i++)
        {
            selected[i] = SelectRandomObj<T>(pool, ref sum);
        }

        return selected;
    }
}