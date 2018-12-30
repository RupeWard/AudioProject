using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.SOVariables
{
    [CreateAssetMenu(menuName = "RJWS/Refs/Float", order = 1000)]
    public class FloatReference : ScriptableObject
    {
        public bool useConstant = true;
        public float constant;
        public FloatVariable variableValue;

        public float Value
        {
            get { return (useConstant) ? (constant) : (variableValue.Value);  }
        }
    }
}
