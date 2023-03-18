using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility
{
    public static Vector3 RoundVector3(Vector3 value, int digit)
    {
        value.x = (float)Math.Round(value.x, digit);
        value.y = (float)Math.Round(value.y, digit);
        value.z = (float)Math.Round(value.z, digit);
        return value;
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.OrderBy(_ => Guid.NewGuid());
    }
}