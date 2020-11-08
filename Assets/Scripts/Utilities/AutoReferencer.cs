using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Helper class to automatize the UI assignments in the inspector
/// </summary>
/// <typeparam name="T"></typeparam>
public class AutoReferencer<T> : MonoBehaviour where T : AutoReferencer<T>
{
    #if UNITY_EDITOR
    //This method is called once when I add component to game object
    protected virtual void Reset()
    {
        //Magic of reflection
        //For each field in my class/component I'm looking only for those that are empty/null
        foreach(var field in typeof(T).GetFields().Where(field => field.GetValue(this) == null))
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

            //if I find ibject that have same name as field, I'm trying to get component that will be in type of a field and assign it
            if (obj != null)
            {
                field.SetValue(this, obj.GetComponent(field.FieldType));
            }
        }
    }

    #endif
}
