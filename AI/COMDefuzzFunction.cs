using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COMDefuzzFunction {



    public int GetMaximumAt(int x)
    {
        int[] maximums = { 10, 35, 60, 90 };
        return maximums[x];
    }
}
