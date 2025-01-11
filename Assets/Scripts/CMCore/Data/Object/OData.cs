using NaughtyAttributes;
using UnityEngine;

namespace CMCore.Data.Object
{
    public class OData<T> : MonoBehaviour where T : struct // a generic class to store several types of data.
    {
        [field: SerializeField]
        protected T Data { get; set; }
        
        public virtual T GetData()
        {
            return Data;
        }

        public virtual void SetData(T newData)
        {
            Data = newData;
        }
    }
}