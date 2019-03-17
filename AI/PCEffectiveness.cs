using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCEffectiveness {

    public float[] GetValuesAt(float x)
    {
        float[] strategyWeights = { 0, 0, 0, 0 };


        if (x >= 0 && x <= 15)
        {
            strategyWeights[0] = 1;
            return strategyWeights;
        }

        if (x > 15 && x <= 20)
        {
            strategyWeights[0] = (-x / 15) + 2;
            return strategyWeights;
        }

        if (x > 20 && x < 30)
        {
            strategyWeights[0] = (-x / 15) + 2;
            strategyWeights[1] = (x / 10) - 2;
            return strategyWeights;
        }

        if (x >= 30 && x <= 40)
        {
            strategyWeights[1] = 1;
            return strategyWeights;
        }

        if (x > 40 && x < 50)
        {
            strategyWeights[1] = (-x / 10) + 5;
            strategyWeights[2] = (x / 10) - 4;
            return strategyWeights;
        }

        if (x >= 50 && x <= 60)
        {
            strategyWeights[2] = 1;
            return strategyWeights;
        }

        if (x > 60 && x < 70)
        {
            strategyWeights[2] = (-x / 10) + 7;
            strategyWeights[3] = (x / 20) - 3;
            return strategyWeights;
        }

        if (x >= 70 && x < 80)
        {
            strategyWeights[3] = (x / 20) - 3;
            return strategyWeights;
        }

        if (x >= 80)
        {
            strategyWeights[3] = 1;
            return strategyWeights;
        }

        strategyWeights[0] = -1;
        strategyWeights[1] = -1;
        strategyWeights[2] = -1;
        strategyWeights[3] = -1;
        return strategyWeights;
    }
}
