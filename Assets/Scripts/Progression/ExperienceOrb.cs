using GalacticRogue.Bootstrap;
using GalacticRogue.Player;
using UnityEngine;

namespace GalacticRogue.Progression
{
    /// <summary>Small, visible XP pickup that homes toward the player at close range.</summary>
    public sealed class ExperienceOrb : MonoBehaviour
    {
        private int value;
        private Transform player;

        public static void Create(Vector3 position, int value)
        {
            GameObject orbObject = new GameObject("XP Core");
            orbObject.transform.position = ClampToVisibleScreen(position);
            orbObject.transform.localScale = Vector3.one * 0.18f;

            SpriteRenderer renderer = orbObject.AddComponent<SpriteRenderer>();
            renderer.sprite = PrototypeSpriteFactory.CreateCircleSprite();
            renderer.color = new Color(0.25f, 1f, 0.55f);

            ExperienceOrb orb = orbObject.AddComponent<ExperienceOrb>();
            orb.value = value;
        }

        private static Vector3 ClampToVisibleScreen(Vector3 position)
        {
            Camera camera = Camera.main;
            if (camera == null) return position;

            Vector3 viewport = camera.WorldToViewportPoint(position);
            viewport.x = Mathf.Clamp(viewport.x, 0.04f, 0.96f);
            viewport.y = Mathf.Clamp(viewport.y, 0.04f, 0.96f);
            viewport.z = -camera.transform.position.z;
            return camera.ViewportToWorldPoint(viewport);
        }

        private void Start()
        {
            PlayerShipController controller = Object.FindAnyObjectByType<PlayerShipController>();
            player = controller != null ? controller.transform : null;
        }

        private void Update()
        {
            if (player == null) return;

            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= 1.4f)
                transform.position = Vector2.MoveTowards(transform.position, player.position, 5f * Time.deltaTime);

            if (distance <= 0.16f)
            {
                player.GetComponent<ExperienceController>().AddExperience(value);
                Destroy(gameObject);
            }
        }
    }
}
