using UnityEngine;
using GalacticRogue.Bootstrap;

namespace GalacticRogue.Combat
{
    /// <summary>
    /// Simple health bar above enemy ships. Procedural sprite-based for prototype.
    /// </summary>
    public sealed class HealthBar : MonoBehaviour
    {
        private Damageable damageable;
        private SpriteRenderer fillRenderer;

        public static void Create(GameObject owner, Damageable health)
        {
            GameObject barObject = new GameObject("HealthBar");
            barObject.transform.SetParent(owner.transform, false);
            barObject.transform.localPosition = new Vector3(0f, 0.6f, 0f);
            barObject.transform.localRotation = Quaternion.identity;

            SpriteRenderer renderer = barObject.AddComponent<SpriteRenderer>();
            renderer.sprite = PrototypeSpriteFactory.CreateCircleSprite();
            renderer.color = new Color(0.2f, 1f, 0.4f, 0.9f);
            renderer.transform.localScale = new Vector3(0.6f, 0.12f, 1f);
            renderer.sortingOrder = 10;

            HealthBar bar = barObject.AddComponent<HealthBar>();
            bar.fillRenderer = renderer;
            bar.damageable = health;
        }

        private void Update()
        {
            if (damageable == null || fillRenderer == null) return;
            float ratio = (float)damageable.CurrentHull / damageable.MaximumHull;
            fillRenderer.transform.localScale = new Vector3(Mathf.Max(0.05f, 0.6f * ratio), 0.12f, 1f);

            if (ratio <= 0.3f)
                fillRenderer.color = new Color(1f, 0.2f, 0.2f, 0.9f);
            else
                fillRenderer.color = new Color(0.2f, 1f, 0.4f, 0.9f);
        }
    }
}