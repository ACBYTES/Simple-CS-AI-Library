MIT License

Copyright (c) 2020 Alireza Shahbazi (ACBYTES) (alirezashahbazi641@yahoo.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearning
{
    class Game
    {
        //Total blocks = 16 SFFF
        //                  FHFH
        //                  FFFH
        //                  HFFG
        //S = 0, F = 0, H = 0 (Game Over), G = 1 (Game Over)

        int currentState = 0; //MAX = 15
        int[] holes = new int[] { 5, 7, 11, 12 };
        int goal = 15;
        int[] inGames = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

        public Game()
        {
            Console.WriteLine("SFFF\nFHFH\nFFFH\nHFFG");
        }

        public int Start()
        {
            currentState = 0;
            return 0;
        }

        public static string ReturnActionValueAsTableIndexString(int IntValue)
        {
            if (IntValue == 0)
                return "Up";
            else if (IntValue == 1)
                return "Down";
            else if (IntValue == 2)
                return "Right";
            else if (IntValue == 3)
                return "Left";
            else
                throw new Exception("Value can be between 0 and 3");
        }

        public static int ReturnActionValueAsTableIndexInt(string Value)
        {
            if (Value == "Up")
                return 0;
            else if (Value == "Down")
                return 1;
            else if (Value == "Right")
                return 2;
            else if (Value == "Left")
                return 3;
            else
                throw new Exception("Value can be between 0 and 3");
        }

        public int ReturnActionToStateValue(string Value)
        {
            if (Value == "Up")
                return -4;
            else if (Value == "Down")
                return 4;
            else if (Value == "Right")
                return 1;
            else if (Value == "Left")
                return -1;
            else
                throw new Exception("Value can be between 0 and 3");
        }

        public bool Step(string Action, out int Reward, out int NewState)
        {
            Console.ReadKey();
            Reward = 0;
            var s = ReturnActionToStateValue(Action);
            currentState += s;
            foreach (var item in inGames)
            {
                if (item != currentState)
                {
                    Reward = -1;
                }
                else
                {
                    Reward = 0;
                    break;
                }
            }
            currentState = Math.Min(Math.Max(currentState, 0), 15);
            NewState = currentState;

            int top = 19;
            int leftDec = 0;
            if (currentState < 4)
            {
                top = 19;
                leftDec = 0;
            }
            else if (currentState < 8)
            {
                top = 20;
                leftDec = 4;
            }
            else if (currentState < 12)
            {
                top = 21;
                leftDec = 8;
            }
            else
            {
                top = 22;
                leftDec = 12;
            }

            Console.SetCursorPosition(currentState - leftDec, top - 1);

            if (Reward == -1)
            {
                return true;
            }
            bool hasEnteredHole = false;
            foreach (var item in holes)
            {
                if (currentState == item)
                    hasEnteredHole = true;
            }
            if (hasEnteredHole)
            {
                //Reward = -1;
                return true;
            }
            else
            {
                if (currentState == 15)
                {
                    Reward = 1;
                    return true;
                }

                else
                    return false;
            }
        }

        Task Delay(int ms)
        {
            Task.Delay(ms);
            return Task.CompletedTask;
        }
    }
}
