using System.Collections.Generic;
using UnityEngine;

namespace CMCore.Data
{
    [CreateAssetMenu(fileName = "MaterialLibrary", menuName = "CMCore/Material Library")]
    public class MaterialLibrary : ScriptableObject
    {
        public List<Material> cubeMaterials;
    }
}