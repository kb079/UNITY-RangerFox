using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExtRandom : MonoBehaviour
{
    public static T Choose<T>(IList<T> array)
    {
        return array[Random.Range(0, array.Count)];
    }

    //desordena la lista
    public static void RandomizeArray(IList array)
    {
        int n = array.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            var value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }

    //los diccionarios son como los Maps de Java (de hecho el HashMap es una versión de diccionario)
    public static T ChooseWeighted<T>(Dictionary<T, int> weightedItems)
    {
        //devuelve la suma de los elementos. Cada elemento tiene un peso
        //I----1-----------------4--------2----1----1I etc
        int total = weightedItems.Sum(v => v.Value);
        int r = Random.Range(0, total);
        int cuenta = 0;
        //va comprobando paso a paso el rango en el que se encuentra el valor. Ej:
        //I----1----X------------4--------2----1----1I, la X no es menor a 1, per sí menor a 1+5
        foreach (KeyValuePair<T, int> item in weightedItems)
        {
            cuenta += item.Value;
            if (r < cuenta)
            {
                return item.Key;
            }
        }
        return default(T);
    }
}
