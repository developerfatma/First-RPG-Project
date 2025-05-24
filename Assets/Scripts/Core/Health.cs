using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;
        [SerializeField] UnityEvent onDamage;
        [SerializeField] UnityEvent onDie;

        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        

        
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            onDamage.Invoke();
            if(health == 0)
            {
                onDie.Invoke();
                Die();
            }

        }

        public void Heal(float healtToRestore)
        {
            health = Mathf.Min(health + healtToRestore, 200);
        }

        private void Die()
        {
            if(isDead == true)
            {
                return;
            }

            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}

