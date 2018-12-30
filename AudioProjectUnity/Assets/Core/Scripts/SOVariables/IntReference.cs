using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.SOVariables
{
    [CreateAssetMenu(menuName = "RJWS/Refs/Int", order = 1000)]
    public class IntReference : ScriptableObject
    {
        public bool useConstant = true;
        public int constant;
        public IntVariable variableValue;

        public int Value
        {
            get { return (useConstant) ? (constant) : (variableValue.Value);  }
        }
    }
}
