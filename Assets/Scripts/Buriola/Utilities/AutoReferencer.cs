using UnityEngine;
using System.Linq;

namespace Buriola.Utilities
{
    public class AutoReferencer<T> : MonoBehaviour where T : AutoReferencer<T>
    {
#if UNITY_EDITOR
        private void Reset()
        {
            //Magic of reflection
            //For each field in my class/component I'm looking only for those that are empty/null
            foreach (var field in typeof(T).GetFields().Where(field => field.GetValue(this) == null))
            {
                //Now I'm look for object (self or child) that have same name as field
                Transform obj;
                if (transform.name == field.Name)
                {
                    obj = transform;
                }
                else
                {
                    obj = transform.Find(field.Name); // Or I need to implement recursion to looking into deeper childs
                }

                //if I find object that have same name as field, I'm trying to get component that will be in type of a field and assign it
                if (obj != null)
                {
                    field.SetValue(this, obj.GetComponent(field.FieldType));
                }
            }
        }

#endif
    }
}
