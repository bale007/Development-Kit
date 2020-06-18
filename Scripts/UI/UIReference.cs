using System.Collections.Generic;
using UnityEngine;

namespace Bale007.UI
{
    public class UIReference : MonoBehaviour
    {
        [SerializeField]
        private List<ReferenceObj> references;

        public T TryGetComponent<T>(string name) where T: MonoBehaviour
        {
            foreach(ReferenceObj reference in references)
            {
                if(reference.name == name)
                {
                    return reference.obj.GetComponent<T>();
                }
            }

            Debug.LogError("Obj Not Found :"+ name);

            return null;
        }
    
        public GameObject TryGetGameObject(string name)
        {
            foreach(ReferenceObj reference in references)
            {
                if(reference.name == name)
                {
                    return reference.obj;
                }
            }

            Debug.LogError("Obj Not Found :"+ name);

            return null;
        }

        public void Dispose()
        {
            Destroy(this);
        }

        [System.Serializable]
        internal class ReferenceObj
        {
            public string name;
            public GameObject obj;
        }
    }
}
