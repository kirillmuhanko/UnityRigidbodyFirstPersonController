using UnityEngine;

namespace Shared.Mathematics
{
    public static class Vector3Math
    {
        public static Vector3 Clamp(Vector3 vector, float min, float max)
        {
            vector.x = Mathf.Clamp(vector.x, min, max);
            vector.y = Mathf.Clamp(vector.y, min, max);
            vector.z = Mathf.Clamp(vector.z, min, max);
            return vector;
        }

        public static Vector3 Snap(Vector3 vector, float snapValue)
        {
            vector.x = Snap(vector.x, snapValue);
            vector.y = Snap(vector.y, snapValue);
            vector.z = Snap(vector.z, snapValue);
            return vector;
        }

        private static float Snap(float value, float snapValue)
        {
            value = snapValue * Mathf.Round(value / snapValue);
            return value;
        }
    }
}