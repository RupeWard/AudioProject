using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.SOVariables
{
    [CreateAssetMenu(menuName = "RJWS/Refs/Vector3", order = 1000)]
    public class Vector3Reference : ScriptableObject
    {
        public bool useConstant = true;
        public Vector3 constant;
        public Vector3Variable variableValue;

        public Vector3 Value
        {
            get { return (useConstant) ? (constant) : (variableValue.Value);  }
        }
    }
}
