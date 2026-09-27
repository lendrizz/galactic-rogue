using UnityEngine;

namespace GalacticRogue.Combat
{
    /// <summary>
    /// Floating damage number that rises and fades out after a hit.
    /// </summary>
    public sealed class DamageNumber : MonoBehaviour
{
        private float lifetime;
        private float fadeSpeed;
        private TextMesh textMesh;

        public static void Create(Vector3 position, int damage)
        {
            GameObject numberObject = new GameObject("DamageNumber");
            numberObject.transform.position = position + Vector3.up * 0.5f;

            TextMesh tm = numberObject.AddComponent<TextMesh>();
            tm.text = damage.ToString();
            tm.fontSize = 5;
            tm.color = Color.red;
            tm.alignment = TextAlignment.Center;
            tm.anchor = TextAnchor.MiddleCenter;

            DamageNumber number = numberObject.AddComponent<DamageNumber>();
            number.lifetime = 0.6f;
            number.fadeSpeed = 1.5f;
        }

        private void Update()
        {
            lifetime -= Time.deltaTime;
            transform.position += Vector3.up * Time.deltaTime;

            if (textMesh == null) return;

            float alpha = Mathf.Clamp01(lifetime / 0.6f);
            textMesh.color = new Color(1f, 0.2f, 0.2f, alpha);

            if (lifetime <= 0f)
                Destroy(gameObject);
        }
    }
}