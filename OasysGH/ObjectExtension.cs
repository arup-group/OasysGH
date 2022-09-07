using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OasysGH
{
  public static class ObjectExtension
  {
    public static object Duplicate(this object objSource)
    {
      // get the type of source object and create a new instance of that type
      Type typeSource = objSource.GetType();

      object objTarget = Activator.CreateInstance(typeSource);

      // return here if source is struct
      if (typeSource.IsValueType)
      {
        objTarget = objSource;
        return objTarget;
      }
      if (typeSource == typeof(System.Guid))
        return Guid.NewGuid();

      // get all the properties of source object type
      PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
      // assign all source property to taget object's properties
      foreach (PropertyInfo property in propertyInfo)
      {
        // check whether property can be written to
        if (property.CanWrite)
        {
          object objPropertyValue;
          Type propertyType = property.PropertyType;
          try
          {
            // check wether property is an guid - this we do not want to duplicate
            if (propertyType.Equals(typeof(System.Guid)))
              objPropertyValue = Guid.NewGuid();
            else
              objPropertyValue = property.GetValue(objSource, null);

            // check wether property is an interface
            if (propertyType.IsInterface)
            {
              if (objPropertyValue != null)
                propertyType = objPropertyValue.GetType();
            }

            // check wether property is an enumerable
            if (typeof(IEnumerable).IsAssignableFrom(propertyType) && !typeof(string).IsAssignableFrom(propertyType))
            {
              if (objPropertyValue == null)
              {
                property.SetValue(objTarget, null, null);
              }
              else
              {
                IEnumerable<object> enumerable = ((IEnumerable)objPropertyValue).Cast<object>();
                Type enumrableType = enumerable.GetType().GetGenericArguments()[0];

                // if type is a struct, we have to check the actual list items
                // this will fail if list is actually of type "System.Object"..
                if (enumrableType.ToString() is "System.Object")
                {
                  if (enumerable.Any())
                    enumrableType = enumerable.First().GetType();
                  else
                    continue; // can´t get type of struct in empty list? 
                }

                Type genericListType = typeof(List<>).MakeGenericType(enumrableType);

                IList list = (IList)Activator.CreateInstance(genericListType);
                using (var enumerator = enumerable.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    list.Add(enumerator.Current.Duplicate());
                  }
                }
                property.SetValue(objTarget, list, null);
              }
            }

            // check whether property type is value type, enum or string type
            else if (propertyType.IsValueType || propertyType.IsEnum || propertyType.Equals(typeof(System.String)))
            {
              property.SetValue(objTarget, objPropertyValue, null);
            }
            else
            // property type is object/complex type, so need to recursively call this method until the end of the tree is reached
            {
              if (objPropertyValue == null)
              {
                property.SetValue(objTarget, null, null);
              }
              else
              {
                property.SetValue(objTarget, objPropertyValue.Duplicate(), null);
              }
            }
          }
          catch (TargetParameterCountException)
          {

          }
        }
      }
      return objTarget;
    }
  }
}
