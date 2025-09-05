using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Bundos.MovingPlatforms
{
    public enum WaypointPathType { Closed, Open }
    public enum WaypointBehaviorType { Loop, PingPong }

    public class PlatformController : MonoBehaviour
    {
        [HideInInspector]
        public List<Vector3> waypoints = new List<Vector3>();

        [Header("Editor Settings")]
        public float handleRadius = 0.5f;
        public Vector2 snappingSettings = new Vector2(0.1f, 0.1f); // NECESSÁRIO pro Editor
        public Color gizmoDeselectedColor = Color.blue;

        [Header("Platform Settings")]
        [SerializeField] private Rigidbody2D rb;
        public bool editing = false;

        public WaypointPathType pathType = WaypointPathType.Closed;
        public WaypointBehaviorType behaviorType = WaypointBehaviorType.Loop;

        public float moveSpeed = 5f;
        public float stopDistance = 0.1f;

        private int lastWaypointIndex = -1;
        private int currentWaypointIndex = 0;
        private int direction = 1; // 1 para frente, -1 para trás

        private void Update()
        {
            if (waypoints.Count == 0) return;

            if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex]) <= stopDistance)
            {
                lastWaypointIndex = currentWaypointIndex;

                if (pathType == WaypointPathType.Closed)
                {
                    if (behaviorType == WaypointBehaviorType.Loop)
                        currentWaypointIndex = Mod(currentWaypointIndex + direction, waypoints.Count);
                    else if (behaviorType == WaypointBehaviorType.PingPong)
                        UpdatePingPong();
                }
                else // Open path
                {
                    if (behaviorType == WaypointBehaviorType.Loop)
                        currentWaypointIndex = Mod(currentWaypointIndex + direction, waypoints.Count);
                    else if (behaviorType == WaypointBehaviorType.PingPong)
                        UpdatePingPong();
                }
            }
        }

        private void FixedUpdate()
        {
            if (waypoints.Count > 0)
                MoveToWaypoint(waypoints[currentWaypointIndex]);
        }

        private void MoveToWaypoint(Vector3 waypoint)
        {
            Vector2 dir = (waypoint - transform.position).normalized;
            rb.linearVelocity = dir * moveSpeed;
        }

        private void UpdatePingPong()
        {
            if ((currentWaypointIndex == 0 && direction < 0) || (currentWaypointIndex == waypoints.Count - 1 && direction > 0))
                direction *= -1;

            currentWaypointIndex = Mod(currentWaypointIndex + direction, waypoints.Count);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (IsSelected() && editing) return;

            Gizmos.color = gizmoDeselectedColor;

            for (int i = 0; i < waypoints.Count; i++)
            {
                Vector3 nextPoint = (i + 1 < waypoints.Count) ? waypoints[i + 1] : waypoints[0];

                if (pathType == WaypointPathType.Open && i == waypoints.Count - 1) break;

                Gizmos.DrawLine(waypoints[i], nextPoint);
                Gizmos.DrawSphere(waypoints[i], handleRadius / 2f);
            }
        }

        private bool IsSelected()
        {
            return Selection.activeGameObject == transform.gameObject;
        }
#endif

        private int Mod(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}
