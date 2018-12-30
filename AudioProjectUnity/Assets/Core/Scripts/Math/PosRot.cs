using UnityEngine;
using RJWS.Core.DebugDescribable;
using System.Text;

namespace RJWS.Core.Maths
{
    [System.Serializable]
    public class PosRot : IDebugDescribable
    {
        public Vector3 position;

		[SerializeField, EulerAngles]
        public Quaternion rotation;

        public PosRot(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

        public static PosRot Identity
        {
            get { return new PosRot(Vector3.zero, Quaternion.identity); }
        }

        public Vector3 Forward
        {
            get { return rotation * Vector3.forward; }
        }
        public Vector3 Up
        {
            get { return rotation * Vector3.up; }
        }
        public Vector3 Right
        {
            get { return rotation * Vector3.right; }
        }

        public static implicit operator PosRot(Transform transform)
        {
            return new PosRot(transform.position, transform.rotation);
        }

        public static PosRot operator +(PosRot a, PosRot b)
        {
            return new PosRot(a.position + a.rotation * b.position, (a.rotation * b.rotation).normalized);
        }
        public static PosRot operator -(PosRot a, PosRot b)
        {
            Quaternion q = a.rotation * Quaternion.Inverse(b.rotation);
            return new PosRot(a.position - q * b.position, q.normalized);
        }
        public static PosRot WorldAdd(PosRot a, PosRot b)
        {
            return new PosRot(a.position + b.position, (a.rotation * b.rotation).normalized);
        }

        public static PosRot WorldToLocal(PosRot child, PosRot parent)
        {
            return new PosRot(Quaternion.Inverse(parent.rotation) * (child.position - parent.position), (Quaternion.Inverse(parent.rotation) * child.rotation).normalized);
        }

        public static PosRot Lerp(PosRot a, PosRot b, float t)
        {
            Vector3 pos = Vector3.Lerp(a.position, b.position, t);
            Quaternion rot = Quaternion.Slerp(a.rotation, b.rotation, t);
            return new PosRot(pos, rot);
        }

        public static void Draw(PosRot posRot, float size = 1f)
        {
            Draw(posRot, size, Color.white);
        }
        public static void Draw(PosRot posRot, float size, Color color)
        {
            Vector3[] vs = new Vector3[] { new Vector3(0, 0, 0),new Vector3(1, 0, 0) , new Vector3(0, 1, 0) , new Vector3(0, 0, 1) , new Vector3(1, 1, 0),
        new Vector3(1, 0, 1) ,new Vector3(0, 1, 1) ,new Vector3(1, 1, 1)  };

            for (int i = 0; i < vs.Length; i++)
            {
                vs[i] = posRot.position + posRot.rotation * (vs[i] * size * 0.2f);
            }

            Debug.DrawLine(vs[0], vs[1], color);
            Debug.DrawLine(vs[1], vs[4], color);
            Debug.DrawLine(vs[4], vs[2], color);
            Debug.DrawLine(vs[2], vs[0], color);
            Debug.DrawLine(vs[3], vs[5], color);
            Debug.DrawLine(vs[5], vs[7], color);
            Debug.DrawLine(vs[7], vs[6], color);
            Debug.DrawLine(vs[6], vs[3], color);
            Debug.DrawLine(vs[0], vs[3], color);
            Debug.DrawLine(vs[1], vs[5], color);
            Debug.DrawLine(vs[2], vs[6], color);
            Debug.DrawLine(vs[4], vs[7], color);

            Debug.DrawRay(posRot.position, posRot.Right * size, Color.red);
            Debug.DrawRay(posRot.position, posRot.Up * size, Color.green);
            Debug.DrawRay(posRot.position, posRot.Forward * size, Color.blue);
        }

        #region IDebugDescribable

        void IDebugDescribable.DebugDescribe(StringBuilder sb)
        {
            sb.Append("[P=").Append(position.ToString("F3")).Append(" R=").Append(rotation.eulerAngles.ToString("F3")).Append("]");
        }

        #endregion IDebugDescribable

    }

    namespace PosRotExtensions
    {
        public static class PosRotExtensions
        {
            public static PosRot ToPosRot(this Transform t, bool worldSpace)
            {
                if (worldSpace)
                {
                    return new PosRot(t.position, t.rotation);
                }
                else
                {
                    return new PosRot(t.localPosition, t.localRotation);
                }
            }

            public static PosRot ToWorldPosRot( this Transform t)
            {
                return t.ToPosRot(true);
            }

            public static PosRot ToLocalPosRot(this Transform t)
            {
                return t.ToPosRot(false);
            }

            public static void SetPose(this Transform t, PosRot p, bool worldSpace)
            {
                if (worldSpace)
                {
                    t.position = p.position;
                    t.rotation = p.rotation;
                }
                else
                {
                    t.localPosition = p.position;
                    t.localRotation = p.rotation;
                }
            }

            public static void SetLocalPose(this Transform t, PosRot p)
            {
                t.SetPose(p, false);
            }

            public static void SetWorldPose(this Transform t, PosRot p)
            {
                t.SetPose(p, true);
            }

            public static void SetTo(this PosRot pr, Transform t, bool worldSpace)
            {
                if (worldSpace)
                {
                    pr.position = t.position;
                    pr.rotation = t.rotation;
                }
                else
                {
                    pr.position = t.localPosition;
                    pr.rotation = t.localRotation;
                }
            }

            public static void SetToLocal(this PosRot pr, Transform t)
            {
                pr.SetTo(t, false);
            }

            public static void SetToWorld(this PosRot pr, Transform t)
            {
                pr.SetTo(t, true);
            }

            public static void SetTo(this PosRot pr, PosRot other)
            {
                pr.position = other.position;
                pr.rotation = other.rotation;
            }


        }

    }
}