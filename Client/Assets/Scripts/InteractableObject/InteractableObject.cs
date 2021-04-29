using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interactable
{
    public class InteractableObject : MonoBehaviour
    {
        public float radius = 3f;
        public Transform interactionTransform; //my GameObject position 

        private bool isFocus = false;
        private Transform player;

        private bool hasInteracted = false;


        //any interable object can override this method to implement your own behavior
        public virtual void Interact()
        {

            //send message to client log
            MessageWorld.instance.SendMessageWorld("Interacting with " + transform.name);

            //this method is meant to be overwritten
            Debug.Log("Interacting with " + transform.name);
        }


        private void Update()
        {
            if (isFocus && !hasInteracted)
            {
                float distance = Vector3.Distance(player.position, interactionTransform.position);
                if (distance <= radius)
                {
                    Interact();
                    hasInteracted = true;
                }
            }
        }

        //is called by PlayerMovement when the Righ-Click Mouse is pressed and the GameObject is set on InteractableObject
        public void OnFocused(Transform playerTransform)
        {
            isFocus = true;
            player = playerTransform;
            hasInteracted = false;
        }


        //is called by PlayerMovement when the Left-Click Mouse is pressed
        public void OnDefocused()
        {
            isFocus = false;
            player = null;
            hasInteracted = false;
        }


        private void OnDrawGizmosSelected()
        {
            if (interactionTransform == null)
            {
                interactionTransform = transform;
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(interactionTransform.position, radius);
        }
    }
}

