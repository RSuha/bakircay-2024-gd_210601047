using System.Collections;
using UnityEngine;
using Match;

namespace Match.Skills
{
    public class ResetSkill : MonoBehaviour
    {
        [SerializeField]
        private ItemSpawner itemSpawner; // Tüm item'leri yöneten sistem

        [SerializeField]
        private GameObject resetEffectPrefab; // Reset efekti için prefab

        [SerializeField]
        private float resetDelay = 2f; // Eski objelerin yok edilmesinden sonra bekleme süresi

        public void UseResetSkill()
        {
            if (itemSpawner == null)
            {
                Debug.LogError("ItemSpawner is not assigned!");
                return;
            }

            StartCoroutine(ResetSequence());
        }

        private IEnumerator ResetSequence()
        {
            // 1. Sahnedeki tüm objeleri temizle
            ResetAllItems();

            // 2. Reset efekti oynat (isteðe baðlý)
            PlayResetEffect();

            // 3. Bekleme süresi (örneðin, eski objelerin yok edilmesini beklemek için)
            yield return new WaitForSeconds(resetDelay);

            // 4. Yeni objeleri spawn et
            RespawnItems();
        }

        private void ResetAllItems()
        {
            // ItemSpawner'daki objeleri temizle
            itemSpawner.ClearSpawnedObjects();
        }

        private void PlayResetEffect()
        {
            if (resetEffectPrefab == null)
                return;

            // Reset efekti merkezde oluþtur
            Vector3 centerPosition = itemSpawner.transform.position;
            GameObject effect = Instantiate(resetEffectPrefab, centerPosition, Quaternion.identity);

            // Efekti belirli bir süre sonra yok et
            Destroy(effect, 3f); // Örneðin 3 saniye sonra
        }

        private void RespawnItems()
        {
            // Yeni item'leri spawn et
            itemSpawner.SpawnObjects();
        }
    }
}
