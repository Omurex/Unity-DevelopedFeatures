using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExampleClass : MonoBehaviour
{
    // The code in the region below will allow grouping in the inspector
    // This can be used to hide reference slots so the component doesn't become cluttered
    // Variables currently in References class there for example

    #region References
    [System.Serializable]
    class References
    {
        [field: SerializeField] public Rigidbody rb { get; private set; }
        [field: SerializeField] public MeshRenderer meshRenderer { get; private set; }
        [field: SerializeField] public LineRenderer lineRenderer { get; private set; }
    }
    [SerializeField] References references;
    #endregion

    
}