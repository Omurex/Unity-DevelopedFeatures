using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AMR_Project
{
    // Allows non-MonoBehavior scripts to draw gizmos
    public class GizmoDrawer : MonoBehaviour
    {
        public static GizmoDrawer instance { get; private set; }


        // Item1: Positions    Item2: Color    Item3: Radius
        public Dictionary<dynamic, (Vector3[], Color, float)> spheresToDraw = new Dictionary<dynamic, (Vector3[], Color, float)>();
        public Dictionary<dynamic, (List<(Vector3, Vector3)>, Color)> linesToDraw = new Dictionary<dynamic, (List<(Vector3, Vector3)>, Color)>();


        void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }


        void OnDestroy()
        {
            if(this == instance)
            {
                instance = null;
            }
        }


        void OnDrawGizmos()
        {
            foreach((Vector3[], Color, float) ls in spheresToDraw.Values)
            {
                Gizmos.color = ls.Item2;
                foreach(Vector3 p in ls.Item1)
                {
                    Gizmos.DrawSphere(p, ls.Item3);
                }
            }

            foreach((List<(Vector3, Vector3)>, Color) ls in linesToDraw.Values)
            {
                Gizmos.color = ls.Item2;
                foreach((Vector3, Vector3) p in ls.Item1)
                {
                    Gizmos.DrawLine(p.Item1, p.Item2);
                }
            }
        }
    }
}
