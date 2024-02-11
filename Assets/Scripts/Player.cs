using UnityEngine;

namespace Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _mouseSensitivity = 100f;
        [SerializeField] private float _recoilResistance = 5f;

        private Vector3 _rotation;
        private float _recoil;

        private void Start()
        {
            _rotation = transform.localRotation.eulerAngles;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            _rotation.x -= Input.GetAxisRaw("Mouse Y") * _mouseSensitivity * Time.deltaTime;
            _rotation.y += Input.GetAxisRaw("Mouse X") * _mouseSensitivity * Time.deltaTime;

            _recoil = Mathf.MoveTowards(_recoil, 0, _recoilResistance * Time.deltaTime);

            _rotation.x = Mathf.Clamp(_rotation.x - _recoil, -90, 90);

            transform.localRotation = Quaternion.Euler(_rotation);
        }

        public void SetWeaponRecoil(float recoil) =>
            _recoil = recoil;
    }
}