using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


    class StrategyMatrix
    {
        private int[,] strategyArr;

        public StrategyMatrix()
        {
            strategyArr = new int[4, 4];

            strategyArr[0, 0] = 1;
            strategyArr[0, 1] = 1;
            strategyArr[0, 2] = 1;
            strategyArr[0, 3] = 0;

            strategyArr[1, 0] = 1;
            strategyArr[1, 1] = 0;
            strategyArr[1, 2] = 0;
            strategyArr[1, 3] = 0;

            strategyArr[2, 0] = 1;
            strategyArr[2, 1] = 0;
            strategyArr[2, 2] = 2;
            strategyArr[2, 3] = 2;

            strategyArr[3, 0] = 0;
            strategyArr[3, 1] = 0;
            strategyArr[3, 2] = 2;
            strategyArr[3, 3] = 3;

    }

       public int GetStrategy(int playerAggression, int playerEffectiveness)
        {
            return strategyArr[playerAggression, playerEffectiveness];
        }
    }

