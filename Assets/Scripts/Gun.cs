using System;
using System.Collections;
using UnityEngine;

namespace Scripts
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Camera _fpsCam;
        [SerializeField] private Player _player;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Animation _animation;

        [SerializeField] private AudioClip _fireSound;
        [SerializeField] private ParticleSystem _muzzleFlash;
        [SerializeField] private ParticleSystem _impactPrefab;

        [SerializeField] private int _damage = 5;
        [SerializeField] private float _range = 50f;
        [SerializeField] private float _fireRate = 300f;
        [SerializeField] private float _reloadDuration = 2f;
        [SerializeField] private int _maxAmmo = 30;
        [SerializeField] private float _recoilPower = 1f;

        private float _nextTimeToFire;
        private int _currentAmmo;
        private bool _isReloading;
        private bool _isWaitForFire;

        public event Action Changed;

        private void Start()
        {
            _currentAmmo = _maxAmmo;
            Changed?.Invoke();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Fire1"))
                _isWaitForFire = true;

            if (Input.GetButtonUp("Fire1"))
                _isWaitForFire = false;

            if (_isWaitForFire && Time.time >= _nextTimeToFire && _isReloading == false)
                Shoot();

            if (Input.GetKeyDown(KeyCode.R) && _isReloading == false)
                StartCoroutine(Reload());
        }

        public int GetCurrentAmmo() =>
            _currentAmmo;

        private void Shoot()
        {
            _nextTimeToFire = Time.time + 60f / _fireRate;
            _currentAmmo--;
            _player.SetWeaponRecoil(_recoilPower);

            RaycastHit hit;

            if (Physics.Raycast(_fpsCam.transform.position, _fpsCam.transform.forward, out hit, _range))
            {
                Health targetHealth = hit.transform.GetComponent<Health>();

                if (targetHealth != null)
                    targetHealth.TakeDamage(_damage);

                ParticleSystem vfx = Instantiate(_impactPrefab, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(vfx.gameObject, 0.5f);
            }

            if (_animation.isPlaying)
                _animation.Stop();

            _muzzleFlash.Play();
            _audioSource.PlayOneShot(_fireSound);
            _animation.Play();

            Changed?.Invoke();

            if (_currentAmmo == 0)
                StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            _isReloading = true;

            yield return new WaitForSeconds(_reloadDuration);

            _currentAmmo = _maxAmmo;
            Changed?.Invoke();
            _isReloading = false;
        }
    }
}