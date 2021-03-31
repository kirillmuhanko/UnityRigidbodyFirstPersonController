using Shared.Collections;
using Shared.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Environment.Editor
{
    [CustomEditor(typeof(MovingPlatform))]
    public class MovingPlatformEditor : UnityEditor.Editor
    {
        private float _freeMoveHandleRadius = 0.5f;
        private float _snapIncrement = 0.5f;
        private MovingPlatform _platform;
        private SelectList<Vector3> _waypoints;

        private void OnEnable()
        {
            _platform = (MovingPlatform) target;
            _waypoints = new SelectList<Vector3>(_platform.waypoints);
        }

        private void OnSceneGUI()
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            for (var i = 0; i < _waypoints.Count; i++)
            {
                var current = _waypoints.Items[i];
                var nextIndex = i + 1;

                Handles.color = _waypoints.IsSelected(i) ? Color.yellow : Color.red;

                _waypoints.Items[i] = Handles.FreeMoveHandle(
                    current,
                    Quaternion.identity,
                    _freeMoveHandleRadius,
                    Vector3.zero,
                    Handles.SphereHandleCap);

                Handles.color = Color.green;

                var waypoint = _waypoints.GetSelected();
                var handle = Handles.DoPositionHandle(waypoint, Quaternion.identity);
                _waypoints.UpdateSelected(handle);

                if (nextIndex < _waypoints.Count)
                    Handles.DrawLine(current, _waypoints.Items[nextIndex]);

                if (Event.current.type == EventType.Used)
                {
                    var distance = HandleUtility.DistanceToCircle(current, _freeMoveHandleRadius);

                    if (distance <= _freeMoveHandleRadius)
                    {
                        _waypoints.Select(i);
                        Repaint();
                    }
                }
            }
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUI.BeginChangeCheck();

            _freeMoveHandleRadius = EditorGUILayout.Slider("Handle cap radius", _freeMoveHandleRadius, 0, 1);
            _snapIncrement = EditorGUILayout.Slider("Snap Value", _snapIncrement, 0.1f, 1f);
            var waypoint = _waypoints.IsEmpty ? Vector3.zero : _waypoints.GetSelected();
            _waypoints.UpdateSelected(EditorGUILayout.Vector3Field("Selected point", waypoint));

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Add"))
            {
                if (_waypoints.IsEmpty)
                {
                    _waypoints.Items.Add(_platform.transform.position);
                }
                else
                {
                    _waypoints.InsertAfterSelected(_waypoints.GetSelected() + Vector3.forward);
                    _waypoints.SelectNext();
                }

                Undo.RecordObject(target, "Added point");
            }

            if (GUILayout.Button("Delete"))
            {
                _waypoints.RemoveSelected();
                Undo.RecordObject(target, "Deleted point");
            }

            GUILayout.EndHorizontal();

            if (GUILayout.Button("Snap"))
            {
                SnapPoints(_snapIncrement);
                Undo.RecordObject(target, "Snapped points");
            }

            if (EditorGUI.EndChangeCheck())
                SceneView.RepaintAll();
        }

        private void SnapPoints(float snapIncrement)
        {
            for (var i = 0; i < _waypoints.Count; i++)
                _waypoints.Items[i] = Vector3Math.Snap(_waypoints.Items[i], snapIncrement);
        }
    }
}