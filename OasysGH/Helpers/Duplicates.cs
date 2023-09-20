using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OasysGH.Helpers {
  public class Duplicates {

    public static bool AreEqual(object objA, object objB, bool excludeGuid = false) {
      if (objA == null && objB == null) {
        return true;
      }

      if(objA == null || objB == null) {
        return false;
      }

      if (!(excludeGuid && objA.Equals(typeof(Guid)))) {
        if (!objA.ToString().Equals(objB.ToString())) {
          return false;
        }
      }

      Type typeA = objA.GetType();
      Type typeB = objB.GetType();

      PropertyInfo[] propertyInfoA
        = typeA.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
      PropertyInfo[] propertyInfoB
        = typeB.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

      for (int i = 0; i < propertyInfoA.Length; i++) {
        PropertyInfo propertyA = propertyInfoA[i];
        PropertyInfo propertyB = propertyInfoB[i];

        if (!propertyA.CanWrite && !propertyB.CanWrite) {
          continue;
        } else if (!propertyA.CanWrite || !propertyB.CanWrite) {
          if (!objA.Equals(objB)) {
            return false;
          }
        }

        object objPropertyValueA;
        object objPropertyValueB;
        Type propertyTypeA = propertyA.PropertyType;
        Type propertyTypeB = propertyB.PropertyType;

        try {
          objPropertyValueA = propertyA.GetValue(objA, null);
          objPropertyValueB = propertyB.GetValue(objB, null);

          if (propertyTypeA.IsInterface) {
            if (objPropertyValueA != null) {
              propertyTypeA = objPropertyValueA.GetType();
            }
          }

          if (propertyTypeB.IsInterface) {
            if (objPropertyValueB != null) {
              propertyTypeB = objPropertyValueB.GetType();
            }
          }

          if (typeof(IEnumerable).IsAssignableFrom(propertyTypeA)
            && !typeof(string).IsAssignableFrom(propertyTypeA)) {
            if (typeof(IEnumerable).IsAssignableFrom(propertyTypeB)
              && !typeof(string).IsAssignableFrom(propertyTypeB)) {
              if (objPropertyValueA == null || objPropertyValueB == null) {
                if (!objPropertyValueA.Equals(objPropertyValueB)) {
                  return false;
                }
              } else {
                IEnumerable<object> enumerableA = ((IEnumerable)objPropertyValueA).Cast<object>();
                IEnumerable<object> enumerableB = ((IEnumerable)objPropertyValueB).Cast<object>();

                Type enumrableTypeA = null;
                Type enumrableTypeB = null;
                if (enumerableA.GetType().GetGenericArguments().Length > 0) {
                  enumrableTypeA = enumerableA.GetType().GetGenericArguments()[0];
                }

                if (enumerableB.GetType().GetGenericArguments().Length > 0) {
                  enumrableTypeB = enumerableB.GetType().GetGenericArguments()[0];
                }

                if (!enumrableTypeA.Equals(enumrableTypeB)) {
                  return false;
                }

                if (enumrableTypeA.ToString() is "System.Object") {
                  if (enumerableA.Any()) {
                    enumrableTypeA = enumerableA.First().GetType();
                  } else {
                    continue;
                  }
                }

                if (enumrableTypeB.ToString() is "System.Object") {
                  if (enumerableB.Any()) {
                    enumrableTypeB = enumerableB.First().GetType();
                  } else {
                    continue;
                  }
                }

                Type genericListTypeA = typeof(List<>).MakeGenericType(enumrableTypeA);
                Type genericListTypeB = typeof(List<>).MakeGenericType(enumrableTypeB);
                if (!genericListTypeA.Equals(genericListTypeB)) {
                  return false;
                }

                IEnumerator<object> enumeratorB = enumerableB.GetEnumerator();

                using (IEnumerator<object> enumeratorA = enumerableA.GetEnumerator()) {
                  while (enumeratorA.MoveNext()) {
                    if (!enumeratorB.MoveNext()) {
                      return false;
                    }

                    AreEqual(enumeratorA.Current, enumeratorB.Current);
                  }
                }
              }
            } else {
              if (!objPropertyValueA.Equals(objPropertyValueB)) {
                return false;
              }
            }
          } else if (propertyTypeA.IsValueType || propertyTypeA.IsEnum
            || propertyTypeA.Equals(typeof(string))) {
            if (excludeGuid && propertyTypeA.Equals(typeof(Guid))) {
              continue;
            }

            if (!objPropertyValueA.Equals(objPropertyValueB)) {
              return false;
            }
          } else if (objPropertyValueA == null || objPropertyValueB == null) {
            if (!objPropertyValueA.Equals(objPropertyValueB)) {
              return false;
            }
          } else {
            AreEqual(objPropertyValueA, objPropertyValueB, excludeGuid);
          }
        } catch (TargetParameterCountException) { }
      }

      return true;
    }
  }
}
