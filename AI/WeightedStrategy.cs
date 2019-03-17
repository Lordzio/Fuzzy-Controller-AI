using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    class WeightedStrategy
    {
        int strategy;
        float weight;

        public WeightedStrategy(float weight, int strategy)
        {
            this.strategy = strategy;
            this.weight = weight;
        }

        public float GetWeight()
        {
            return weight;
        }

        public int GetStrategy()
        {
            return strategy;
        }
    }

