using CMCore.Util;
using UnityEngine;

namespace CMCore.Data.Object
{
    [RequireComponent(typeof(OPrefabInfo))]
    [AddComponentMenu("Transform Data")]
    public class OTransformData : OData<TransformData>
    {

        public override void SetData(TransformData newData)
        {
            base.SetData(newData);

            SetPosition();
            SetRotation();
            SetScale();
        }

        public override TransformData GetData()
        {
            var t = transform;
            return new TransformData(t.position, t.localRotation, t.lossyScale);
        }

        private void SetScale() =>transform.localScale = Data.scale;
        private void SetRotation() =>transform.localRotation = Data.rotation;
        private void SetPosition() =>transform.localPosition = Data.position;

        public void SetDataAroundPos(TransformData transformData)
        {
            var rand = new System.Random();
            var pos = Extensions.GeneratePositionAbove(transformData.position, 0.5f, 0.2f,45, 90, rand);

            var newTransformData = new TransformData(pos, transformData.rotation, transformData.scale);
            
            SetData(newTransformData);
        }
    }


    [System.Serializable]
    public struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }
}