using System.Collections.Generic;
using UnityEngine;

namespace GalacticRogue.Combat
{
    public sealed class Targetable : MonoBehaviour
    {
        private void OnEnable() => TargetRegistry.Register(this);
        private void OnDisable() => TargetRegistry.Unregister(this);
    }

    public static class TargetRegistry
    {
        private static readonly List<Targetable> Targets = new List<Targetable>();

        public static void Register(Targetable target)
        {
            if (!Targets.Contains(target))
            {
                Targets.Add(target);
            }
        }

        public static void Unregister(Targetable target) => Targets.Remove(target);

        public static Targetable FindNearest(Vector2 origin, float range, bool visibleOnly = true, Transform excludedTarget = null)
        {
            Targetable nearest = null;
            float nearestDistanceSquared = range * range;

            for (int i = Targets.Count - 1; i >= 0; i--)
            {
                Targetable candidate = Targets[i];
                if (candidate == null)
                {
                    Targets.RemoveAt(i);
                    continue;
                }

                if (candidate.transform == excludedTarget || (visibleOnly && !IsVisible(candidate.transform.position)))
                    continue;

                float distanceSquared = ((Vector2)candidate.transform.position - origin).sqrMagnitude;
                if (distanceSquared < nearestDistanceSquared)
                {
                    nearest = candidate;
                    nearestDistanceSquared = distanceSquared;
                }
            }

            return nearest;
        }

        public static bool IsVisible(Vector3 worldPosition)
        {
            Camera camera = Camera.main;
            if (camera == null) return true;

            Vector3 viewportPoint = camera.WorldToViewportPoint(worldPosition);
            return viewportPoint.z >= 0f && viewportPoint.x >= 0f && viewportPoint.x <= 1f && viewportPoint.y >= 0f && viewportPoint.y <= 1f;
        }
    }
}
