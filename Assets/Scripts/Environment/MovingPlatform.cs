using System.Collections.Generic;
using Shared.Collections;
using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovingPlatform : MonoBehaviour
    {
        public float pauseTime = 0.5f;
        public float speed = 4f;
        public List<Vector3> waypoints = new List<Vector3>();
        private bool _isReversed = true;
        private float _nextMoveTime;
        private Rigidbody _platform;
        private SelectList<Vector3> _waypoints;
        private Vector3 _currentWaypoint;

        private void Start()
        {
            _platform = GetComponent<Rigidbody>();
            _platform.isKinematic = true;
            _waypoints = new SelectList<Vector3>(waypoints);
            _currentWaypoint = _waypoints.GetSelected();
        }

        private void FixedUpdate()
        {
            UpdatePlatform();
        }

        private void MovePlatform(Vector3 target)
        {
            var step = speed * Time.deltaTime;
            var position = Vector3.MoveTowards(transform.position, target, step);
            _platform.MovePosition(position);
        }

        private void SelectNextWaypoint()
        {
            _waypoints.SelectNext(_isReversed, true);
            _currentWaypoint = _waypoints.GetSelected();
        }

        private void UpdatePlatform()
        {
            if (Time.time < _nextMoveTime)
                return;

            if (Vector3.Distance(transform.position, _currentWaypoint) > 0.001f)
            {
                MovePlatform(_currentWaypoint);
            }
            else
            {
                SelectNextWaypoint();

                if (_waypoints.IsFirstOrLastSelected)
                    _isReversed = !_isReversed;

                _nextMoveTime = Time.time + pauseTime;
            }
        }
    }
}