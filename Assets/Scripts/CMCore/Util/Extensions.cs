using System;
using System.Collections.Generic;
using System.Linq;
using CMCore.Data.Object;
using CMCore.Gameplay.Cube;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace CMCore.Util
{
    public static class Extensions
    {
        public static T GetRandomElement<T>(this T[] array) => array[Random.Range(0, array.Length)];
        public static T GetRandomElement<T>(this List<T> list) => list[Random.Range(0, list.Count)];

        public static List<T> GetRandomElements<T>(this List<T> list, int elementsCount) =>
            list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();

        public static T[] GetRandomElements<T>(this T[] array, int elementsCount) =>
            array.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToArray();

        public static int UniqueCount<T>(this List<T> list) => list.Distinct().Count();
        public static int UniqueCount<T>(this T[] array) => array.Distinct().Count();
        public static int DuplicateCount<T>(this List<T> list) => list.Count - list.Distinct().Count();
        public static int DuplicateCount<T>(this T[] array) => array.Length - array.Distinct().Count();
        public static bool HasDuplicates<T>(this List<T> list) => list.Count != list.Distinct().Count();
        public static bool HasDuplicates<T>(this T[] array) => array.Length != array.Distinct().Count();

        public static void
            DelayedAction(this MonoBehaviour holdLive, Action action, float delay, bool realtime = false) =>
            CoroutineHelper.DelayedAction(holdLive, action, delay, realtime);


        public static void CenterOnChildred(this Transform aParent)
        {
            var childs = aParent.Cast<Transform>().ToList();
            var pos = Vector3.zero;
            foreach (var C in childs)
            {
                pos += C.position;
                C.parent = null;
            }

            pos /= childs.Count;
            aParent.position = pos;
            foreach (var C in childs)
                C.parent = aParent;
        }

        public static void Add<T>(this T[] array, T add)
        {
            array = array.Append(add).ToArray();
        }

        public static int NearestMultipleOf(this int i, int multiple)
        {
            int nearestMultiple =
                (int)Math.Round(
                    (i / (double)multiple),
                    MidpointRounding.AwayFromZero
                ) * multiple;

            return nearestMultiple;
        }

        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            var queue = new Queue<Transform>();
            queue.Enqueue(aParent);
            while (queue.Count > 0)
            {
                var c = queue.Dequeue();
                if (c.name == aName)
                    return c;
                foreach (Transform t in c)
                    queue.Enqueue(t);
            }

            return null;
        }

        public static bool IsPointerOverUIObject()
        {
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        public static void ChangeLayer(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
        }


        public static void ChangeLayer(this GameObject gameObject, string layer)
        {
            gameObject.layer = LayerMask.NameToLayer(layer);
        }

        public static void ChangeToRandomLayer(this GameObject gameObject, List<string> layers)
        {
            var randomLayer = layers.GetRandomElement();

            gameObject.layer = LayerMask.NameToLayer(randomLayer);
        }


        public static (int, int, int) GetMinutesSecondsMilliseconds(this float s)
        {
            var minutes = (int)s / 60;
            var seconds = (int)s % 60;
            var milliseconds = (int)(s * 1000) % 1000;

            return (minutes, seconds, milliseconds);
        }

        public static Vector3 GeneratePositionAbove(Vector3 given, float radius, float height, float angle1,
            float angle2, System.Random random)
        {
            angle1 = (float)random.NextDouble() * 360f;
            angle2 = (float)random.NextDouble() * 360f;

            var x = radius * Mathf.Sin(angle2) * Mathf.Cos(angle1);
            var y = radius * Mathf.Cos(angle2);
            var z = radius * Mathf.Sin(angle2) * Mathf.Sin(angle1);

            y += height;

            x += given.x;
            y += given.y;
            z += given.z;

            return new Vector3(x, y, z);
        }


        public static CubeBehavior FindFurthestCube(this CubeBehavior[] cubes, Vector3 fromPosition)
        {
            CubeBehavior furthestCube = null;
            var furthestDistance = 0f;

            foreach (var cube in cubes)
            {
                var currentPosition = cube.transform.position;
                var currentDistance = Vector3.Distance(fromPosition, currentPosition);

                if (!(currentDistance > furthestDistance)) continue;

                furthestCube = cube;
                furthestDistance = currentDistance;
            }

            return furthestCube;
        }

        public static CubeBehavior FindClosestCube(this CubeBehavior[] cubes, Vector3 fromPosition)
        {
            CubeBehavior closestCube = null;
            var closestDistance = Mathf.Infinity;

            foreach (var cube in cubes)
            {
                var currentPosition = cube.transform.position;
                var currentDistance = Vector3.Distance(fromPosition, currentPosition);

                if (!(currentDistance < closestDistance)) continue;
                closestCube = cube;
                closestDistance = currentDistance;
            }

            return closestCube;
        }
        
        public static CubeBehavior FindAtLeastNCloseOnes(this CubeBehavior[] cubes, float threshold, int number)
        {
            CubeBehavior closest = null;
            var minDistance = float.MaxValue;

            foreach (var cube in cubes)
            {
                var count = 0;
                foreach (var other in cubes)
                {
                    if (cube == other) continue;
                    if (Vector3.Distance(cube.transform.position, other.transform.position) < threshold)
                    {
                        count++;
                        if (count >= number)
                        {
                            return cube;
                        }
                    }
                }
            }

            return null;
        }
    }
}