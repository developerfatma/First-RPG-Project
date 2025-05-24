using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        enum CursorType
        {
            None,
            Movement,
            Combat
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        private void Start()
        {
            health = GetComponent<Health>();
        }


        // Update is called once per frame
        void Update()
        {

            if(health.IsDead() == true)
            {
                return;
            }

            if(InteractWithCombat() == true)
            {
                return;
            }
            if(InteractWithMovement() == true)
            {
                
                return;
            }
            SetCursor(CursorType.None);

        }

        

        private bool InteractWithCombat()
        {
            
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                
                if(target == null)
                {
                    continue;
                }

                if(!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }

                if(target == null)
                {
                    continue;
                }

                

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                    

                }
                SetCursor(CursorType.Combat);
                return true;



            }
            return false;


        }


        private bool InteractWithMovement()
        {
            
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit == true)
            {
                if(Input.GetMouseButton(0))
                {
                    
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
                

            }
            return false;
        }

        private void SetCursor(CursorType cursorType)
        {
            CursorMapping mapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}


