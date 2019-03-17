using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyController {

    private PCAggression aggression;
    private PCEffectiveness effectiveness;
    private List<WeightedStrategy> weightedStrategies;
    private COMDefuzzFunction defuzzFunction;

    private StrategyMatrix strategyMatrix;

    public FuzzyController()
    {
        weightedStrategies = new List<WeightedStrategy>();
        aggression = new PCAggression();
        effectiveness = new PCEffectiveness();
        defuzzFunction = new COMDefuzzFunction();
        strategyMatrix = new StrategyMatrix();
    }

    private void FilterStrategies(float aggr, float eff)
    {
        weightedStrategies = new List<WeightedStrategy>();
        float[] outputAggr, outputEff;
        outputAggr = aggression.GetValuesAt(aggr);
        outputEff = effectiveness.GetValuesAt(eff);


        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (outputAggr[i] > 0 && outputEff[j] > 0)
                {
                    if (outputAggr[i] <= outputEff[j])
                    {
                        weightedStrategies.Add(new WeightedStrategy(outputAggr[i], strategyMatrix.GetStrategy(i, j)));
                    }
                    else
                    if (outputEff[j] <= outputAggr[i])
                    {
                        weightedStrategies.Add(new WeightedStrategy(outputEff[j], strategyMatrix.GetStrategy(i, j)));
                    }
                }
            }
        }


        for (int i = 0; i < 4; i++)
        {
            float highestValue = 0;
            int highestValueIndex = -1;
            List<int> strategyIndexes = new List<int>();

            for (int j = 0; j < weightedStrategies.Count; j++)
            {
                if (weightedStrategies[j].GetStrategy() == i)
                {
                    if (weightedStrategies[j].GetWeight() > highestValue)
                    {
                        highestValue = weightedStrategies[j].GetWeight();
                        highestValueIndex = j;
                    }
                    strategyIndexes.Add(j);
                }

            }

            int offset = 0;
            for (int j = 0; j < strategyIndexes.Count; j++)
            {
                if (highestValueIndex != strategyIndexes[j])
                {
                    weightedStrategies.RemoveAt(strategyIndexes[j] - offset);
                    offset++;
                }
            }

        }
    }

    public int GetStrategy(int aggr, int eff)
    { 
        FilterStrategies(aggr, eff);

        float numerator = 0, denominator = 0, x = 0;

        for (int i = 0; i < weightedStrategies.Count; i++)
        {
            numerator += defuzzFunction.GetMaximumAt(weightedStrategies[i].GetStrategy()) * weightedStrategies[i].GetWeight();
            denominator += weightedStrategies[i].GetWeight();
        }

        x = numerator / denominator;

        if (x >= 0 && x < 25)
            return 0;

        if (x >= 25 && x < 50)
            return 1;

        if (x >= 50 && x < 75)
            return 2;

        if (x >= 75 && x <= 100)
            return 3;

        return -1;

    }
    


}
