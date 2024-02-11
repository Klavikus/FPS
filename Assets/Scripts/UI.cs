using TMPro;
using UnityEngine;

namespace Scripts
{
    public class UI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _ammoText;
        [SerializeField] private Gun _gun;

        private void Awake()
        {
            OnGunAmmoChanged();
            _gun.Changed += OnGunAmmoChanged;
        }

        private void OnGunAmmoChanged() =>
            _ammoText.text = _gun.GetCurrentAmmo().ToString();
    }
}