using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match.Skills
{
    public class MagnetSkill : MonoBehaviour
    {
        public float pullForce = 15f; // Çekim kuvveti
        public float pullDuration = 3f; // Çekim süresi

        [SerializeField]
        private ItemSpawner itemSpawner;

        [SerializeField]
        private GameObject magnetEffectPrefab; // Çekim efekti için prefab

        public void UseMagnetSkill()
        {
            if (itemSpawner == null)
            {
                Debug.LogError("ItemSpawner is not assigned!");
                return;
            }

            if (magnetEffectPrefab == null)
            {
                Debug.LogError("Magnet effect prefab is not assigned!");
                return;
            }

            // Sahnedeki tüm aktif item'leri al
            List<Item> items = itemSpawner.GetItems();
            if (items == null || items.Count == 0)
            {
                Debug.LogWarning("No items found to pull!");
                return;
            }

            Vector3 centerPosition = itemSpawner.transform.position; // Çekim noktası

            // Çekim efekti oluştur
            SpawnMagnetEffect(centerPosition);

            // Her bir objeyi merkeze çek
            foreach (var item in items)
            {
                if (item == null || item.transform == null)
                    continue;

                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    Debug.LogWarning($"Item {item.name} has no Rigidbody!");
                    continue;
                }

                // Objeyi merkeze doğru çekmek için coroutine başlat
                StartCoroutine(PullToCenter(rb, centerPosition));
            }
        }

        private void SpawnMagnetEffect(Vector3 position)
        {
            // Çekim efekti prefab'ını belirtilen pozisyonda oluştur
            GameObject effect = Instantiate(magnetEffectPrefab, position, Quaternion.identity);

            // Efekti belirli bir süre sonra yok et
            Destroy(effect, pullDuration);
        }

        private System.Collections.IEnumerator PullToCenter(Rigidbody rb, Vector3 centerPosition)
        {
            float elapsedTime = 0f;

            while (elapsedTime < pullDuration)
            {
                // Merkeze doğru yön hesapla
                Vector3 direction = (centerPosition - rb.position).normalized;

                // Çekim kuvveti uygula
                rb.AddForce(direction * pullForce * Time.deltaTime, ForceMode.Acceleration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
