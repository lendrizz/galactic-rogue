using System.Collections.Generic;
using GalacticRogue.Bootstrap;
using UnityEngine;

namespace GalacticRogue.Visuals
{
    /// <summary>Lightweight procedural background for the prototype; all stars share one cached sprite.</summary>
    public sealed class Starfield : MonoBehaviour
    {
        private const int StarCount = 56;
        private readonly List<Transform> stars = new List<Transform>(StarCount);
        private readonly List<float> speeds = new List<float>(StarCount);

        private void Start()
        {
            for (int i = 0; i < StarCount; i++)
            {
                GameObject star = new GameObject("Star");
                star.transform.SetParent(transform);
                star.transform.position = new Vector3(Random.Range(-3f, 3f), Random.Range(-5.5f, 5.5f), 2f);
                star.transform.localScale = Vector3.one * Random.Range(0.018f, 0.055f);

                SpriteRenderer renderer = star.AddComponent<SpriteRenderer>();
                renderer.sprite = PrototypeSpriteFactory.CreateCircleSprite();
                renderer.color = new Color(0.4f, Random.Range(0.65f, 0.95f), 1f, Random.Range(0.3f, 0.9f));
                renderer.sortingOrder = -10;

                stars.Add(star.transform);
                speeds.Add(Random.Range(0.18f, 0.8f));
            }
        }

        private void Update()
        {
            for (int i = 0; i < stars.Count; i++)
            {
                Transform star = stars[i];
                star.position += Vector3.down * (speeds[i] * Time.deltaTime);
                if (star.position.y < -5.6f)
                    star.position = new Vector3(Random.Range(-3f, 3f), 5.6f, 2f);
            }
        }
    }
}
