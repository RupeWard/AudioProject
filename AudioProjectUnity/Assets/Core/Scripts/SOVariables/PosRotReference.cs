using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.SOVariables
{
    [CreateAssetMenu(menuName = "RJWS/Refs/PosRot", order = 1000)]
    public class PosRotReference : ScriptableObject
    {
        public bool useConstant = true;
        public Maths.PosRot constant;
        public PosRotVariable variableValue;

        public Maths.PosRot Value
        {
            get { return (useConstant) ? (constant) : (variableValue.Value);  }
        }
    }
}
