using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.SOVariables
{
    [CreateAssetMenu(menuName = "RJWS/Refs/Bool", order = 1000)]
    public class BoolReference : ScriptableObject
    {
        public bool useConstant = true;
        public bool constant;
        public BoolVariable variableValue;

        public bool Value
        {
            get { return (useConstant) ? (constant) : (variableValue.Value);  }
        }
    }
}
