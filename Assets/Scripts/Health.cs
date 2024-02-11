using System;
using UnityEngine;

namespace Scripts
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;

        private int _currentHealth;

        public event Action Died;

        private void Start()
        {
            _currentHealth = _maxHealth;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Died?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}