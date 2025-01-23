using System.Collections;
using UnityEngine;
using Match;

namespace Match.Skills
{
    public class ResetSkill : MonoBehaviour
    {
        [SerializeField]
        private ItemSpawner itemSpawner; // T�m item'leri y�neten sistem

        [SerializeField]
        private GameObject resetEffectPrefab; // Reset efekti i�in prefab

        [SerializeField]
        private float resetDelay = 2f; // Eski objelerin yok edilmesinden sonra bekleme s�resi

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
            // 1. Sahnedeki t�m objeleri temizle
            ResetAllItems();

            // 2. Reset efekti oynat (iste�e ba�l�)
            PlayResetEffect();

            // 3. Bekleme s�resi (�rne�in, eski objelerin yok edilmesini beklemek i�in)
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

            // Reset efekti merkezde olu�tur
            Vector3 centerPosition = itemSpawner.transform.position;
            GameObject effect = Instantiate(resetEffectPrefab, centerPosition, Quaternion.identity);

            // Efekti belirli bir s�re sonra yok et
            Destroy(effect, 3f); // �rne�in 3 saniye sonra
        }

        private void RespawnItems()
        {
            // Yeni item'leri spawn et
            itemSpawner.SpawnObjects();
        }
    }
}
