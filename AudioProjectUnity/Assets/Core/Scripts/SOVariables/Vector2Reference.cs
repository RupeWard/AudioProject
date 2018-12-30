using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.SOVariables
{
    [CreateAssetMenu(menuName = "RJWS/Refs/Vector2", order = 1000)]
    public class Vector2Reference : ScriptableObject
    {
        public bool useConstant = true;
        public Vector2 constant;
        public Vector2Variable variableValue;

        public Vector2 Value
        {
            get { return (useConstant) ? (constant) : (variableValue.Value);  }
        }
    }
}
