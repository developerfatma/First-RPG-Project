using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;
        [SerializeField] float healthToRestore = 0;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                if(weapon != null)
                {
                    other.GetComponent<Fighter>().SpawnWeapon(weapon);
                }
               
                if(healthToRestore > 0)
                {
                    other.GetComponent<Health>().Heal(healthToRestore);
                }
                StartCoroutine(HideForSeconds(respawnTime));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            HideWeapon();
            yield return new WaitForSeconds(seconds);
            ShowWeapon();
        }

        private void ShowWeapon()
        {
            GetComponent<Collider>().enabled = true;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }

        private void HideWeapon()
        {
            GetComponent<Collider>().enabled = false;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

}

