using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearning
{
    class Program
    {
        static QTable table;
        static void Main(string[] args)
        {
            var actionList = new List<double[]>() { };
            int states = 16;
            int actions = 4;
            for (int i = 0; i < states; i++)
            {
                actionList.Add(QTable.Zeros(actions));
            }
            table = new QTable(states, actions, new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" }, actionList); //Actions: UP, DOWN, RIGHT, LEFT
            table.ReadExistingStructure();
            Console.WriteLine(QTable.InitStructure(table));
            //for (int repeater = 0; repeater < 10; repeater++)
            //{
                StartProcess();
            //}
            Console.WriteLine(QTable.InitStructure(table));
            table.SaveQTableToFile();
            Console.ReadLine();
        }

        static void StartProcess()
        {
            Game game = new Game(); //ENV
            int episodes = 10000;
            int maxStepsPerEpisode = 100;

            float learningRate = .1f;
            float discountRate = .99f;
            double explorationRate = 1;
            float maxExplorationRate = 1;
            float minExplorationRate = 0.01f;
            float explorationDecayRate = .01f;


            List<float> rewards = new List<float>();

            Random rand = new Random();


            for (int episode = 0; episode < episodes; episode++)
            {
                var state = game.Start(); //Specify the first state
                bool done = false;
                float currentRewards = 0;

                for (int step = 0; step < maxStepsPerEpisode; step++)
                {
                    int explorationRateThreshold = rand.Next(0, 2);
                    string action = string.Empty;
                    if (explorationRateThreshold > explorationRate /*&& table.All((int)state) != 0*/)
                    {
                        action = table.Max(state.ToString());
                        //We declare an action that will get the max value of the ones in the Q-Table at our particular state.
                    }

                    //else if (explorationRateThreshold > explorationRate && table.All((int)state) == 0)
                    //{
                    //    //Greedy Policy
                    //}

                    else
                    {
                        //var randN = rand.Next(0, 4);
                        //action = Game.ReturnActionValueAsTableIndexString(randN);
                        action = table.Max(state.ToString());
                        //We go for exploration instead of exploitation so we're going to do a random action for that.
                    }

                    int reward, newState;
                    done = game.Step(action, out reward, out newState);
                    //newState, reward, done, info = env.step(action) Get all the data after making the new move
                    table.UpdateValue(state.ToString(), Game.ReturnActionValueAsTableIndexInt(action), (table.GetValue(state.ToString(), Game.ReturnActionValueAsTableIndexInt(action)) * (1 - learningRate)
                        + learningRate * (reward + discountRate * table.MaxInt(newState.ToString()))));
                    //qTable[state, action] = qTable[state, action] * (1 - learningRate) + / learningRate * (reward + discountRate + np.max(qTable[newState, :])) new q through the formula in deeplizard.

                    state = newState;
                    //state = newState Update the current state because player has moved
                    currentRewards += reward;
                    //rewards += reward

                    explorationRate = minExplorationRate + (maxExplorationRate - minExplorationRate) * Math.Exp((-explorationDecayRate * episode));

                    if (done)
                    {
                        rewards.Add(currentRewards);
                        break;
                    }
                }
            }

            List<List<float>> perThousandRewards = new List<List<float>>();
            for (int i = 1; i < 11; i++)
            {
                List<float> thousand = new List<float>();
                for (int j = (1000 * i) - 1000; j < 1000 * i; j++)
                {
                    thousand.Add(rewards[j]);
                }
                perThousandRewards.Add(thousand);
            }
            int count = 1000;

            Console.WriteLine("##########Average reward per thousand episodes##########");
            foreach (var item in perThousandRewards)
            {
                float sum = 0;
                foreach (var t in item)
                {
                    sum += t;
                }
                Console.WriteLine($"{count}: {sum / 1000}");
                count += 1000;
            }

        }
    }
}
