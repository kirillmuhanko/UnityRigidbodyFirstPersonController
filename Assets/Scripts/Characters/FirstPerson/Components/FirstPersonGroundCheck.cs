using System;
using UnityEngine;

namespace Characters.FirstPerson.Components
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class FirstPersonGroundCheck : MonoBehaviour
    {
        public float groundCheckDistance = 0.1f;
        [Range(0f, 90f)] public float slopeLimit = 45f;
        private CapsuleCollider _capsule;
        public bool IsGrounded { get; private set; }
        public float GroundDistance => GroundHit.distance - (_capsule.height / 2f - _capsule.radius);
        public RaycastHit GroundHit { get; private set; }

        private void Awake()
        {
            _capsule = GetComponent<CapsuleCollider>();
        }

        public bool IsOnSteepSlope()
        {
            var angle = Vector3.Angle(Vector3.up, GroundHit.normal);
            var result = angle * 0.99f > slopeLimit;
            return result;
        }

        public void CheckGround()
        {
            if (_capsule.radius > _capsule.height / 2f)
                throw new Exception("Capsule radius is too big.");

            IsGrounded = false;

            if (!Physics.SphereCast(
                transform.position,
                _capsule.radius * 0.99f,
                Vector3.down,
                out var hit,
                _capsule.height,
                Physics.AllLayers,
                QueryTriggerInteraction.Ignore))
                return;

            if (GroundDistance <= groundCheckDistance)
                IsGrounded = true;

            GroundHit = hit;
        }
    }
}