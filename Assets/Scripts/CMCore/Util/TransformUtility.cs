using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CMCore.Util
{
    public static class TransformUtility
    {
        public static Vector3 Center(this List<Transform> transforms)
        {
            var bound = new Bounds(transforms[0].position, Vector3.zero);
            for (int i = 1; i < transforms.Count; i++)
            {
                bound.Encapsulate(transforms[i].position);
            }

            return bound.center;
        }
        
        public static void CenterOnChildren(this Transform aParent)
        {
            var childs = aParent.Cast<Transform>().ToList();
            var pos = Vector3.zero;
            foreach(var C in childs)
            {
                pos += C.position;
                C.parent = null;
            }
            pos /= childs.Count;
            aParent.position = pos;
            foreach(var C in childs)
                C.parent = aParent;
        }   
        public static Vector3 Center(this Transform[] transforms)
        {
            var bound = new Bounds(transforms[0].position, Vector3.zero);
            for (int i = 1; i < transforms.Length; i++)
            {
                bound.Encapsulate(transforms[i].position);
            }

            return bound.center;
        }
        public static Vector3[] AsVector3Array(this List<Transform> transforms)
        {
            var newVectorList = new List<Vector3>();


            for (int i = 0; i < transforms.Count; i++)
            {
                newVectorList.Add(transforms[i].position);
            }

            return newVectorList.ToArray();
        }
    
        public static void SortByDistance(this List<Transform> transforms, Vector3 measureFrom)
        {
            transforms.OrderBy(x => Vector3.Distance(x.position, measureFrom));
        }
    }
}
