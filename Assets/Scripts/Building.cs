using System;

using Unity.Collections;

using UnityEngine;

namespace DefaultNamespace
{
    public class Building : MonoBehaviour
    {
        public virtual Vector2 size => Vector2.one;

        

        public FixedString32Bytes buildingID;

        public bool rotatable = false;

        
        

        private void Start()
        {
            
        }

        public virtual void OnRemove()
        {
            
        }
        public virtual void OnPlace(Map map, Vector2 pos)
        {
            
        }

        public virtual GameObject OnClick()
        {
            return null;
        }
    }
}