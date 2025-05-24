using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Controller;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {

        GameObject player;
        
        void Start()
        {
            player = GameObject.FindWithTag("Player");
            GetComponent<PlayableDirector>().played += DisabledControl;
            GetComponent<PlayableDirector>().stopped += EnabledControl;
        }

        void EnabledControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }

        void DisabledControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = false;
        }

        
    }
}

