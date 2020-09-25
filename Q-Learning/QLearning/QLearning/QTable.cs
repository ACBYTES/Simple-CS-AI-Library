using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLearning
{
    class QTable
    {
        struct Structure
        {
            public int states;
            public int actions;
            public string[] stateValues;
            public List<double[]> actionValues;
        }

        Structure tableStructure;

        public QTable(int States, int Actions, string[] StateValues, List<double[]> ActionValues)
        {
            List<Tuple<string, double[]>> qTable = new List<Tuple<string, double[]>>(); //string is going to be the state's name and the int array is going to be the rewards collected by the agent over each state taking an action.
            for (int i = 0; i < States; i++)
            {
                qTable.Add(new Tuple<string, double[]>(StateValues[i], ActionValues[i]));
            }

            tableStructure.states = States;
            tableStructure.actions = Actions;
            tableStructure.stateValues = StateValues;
            tableStructure.actionValues = ActionValues;
        }

        public static double[] Zeros(int Count)
        {
            double[] values = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                values[i] = 0;
            }
            return values;
        }

        public static string InitStructure(QTable Table)
        {
            string res = "[\n  ";
            for (int i = 0; i < Table.tableStructure.states; i++)
            {
                for (int j = 0; j < Table.tableStructure.actions; j++)
                {
                    res += $"{Table.tableStructure.actionValues[i][j]}, ";
                }
                res += "\n  ";
            }

            return res + "\b\b]";
        }

        public static string InitFileStructure(QTable Table)
        {
            string res = "[\n  ";
            for (int i = 0; i < Table.tableStructure.states; i++)
            {
                for (int j = 0; j < Table.tableStructure.actions; j++)
                {
                    res += $"{Table.tableStructure.actionValues[i][j]}, ";
                }
                res += "\n  ";
            }

            return res + "\b\b]";
        }

        public void SaveQTableToFile()
        {
            var st = InitFileStructure(this);
            string path = $@"C:\Users\Alireza\Desktop\QTableValues({DateTime.Now.DayOfYear}).ACCOQT";
            if (!File.Exists(path))
                File.Create(path).Close();

            File.WriteAllText(path, st);
        }

        public void ReadExistingStructure()
        {
            string path = $@"C:\Users\Alireza\Desktop\QTableValues({DateTime.Now.DayOfYear}).ACCOQT";
            if (File.Exists(path))
            {
                var res = File.ReadAllText(path);
                var resA = res.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                resA.RemoveAt(0);
                resA.RemoveAt(16);
                for (int j = 0; j < resA.Count; j++)
                {
                    var splitedVs = resA[j].Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < splitedVs.Count() - 1; i++)
                    {
                        this.UpdateValue(j.ToString(), 0, double.Parse(splitedVs[0]));
                        this.UpdateValue(j.ToString(), 1, double.Parse(splitedVs[1]));
                        this.UpdateValue(j.ToString(), 2, double.Parse(splitedVs[2]));
                        this.UpdateValue(j.ToString(), 3, double.Parse(splitedVs[3]));
                    }
                }
            }
        }

        public string Max(string State)
        {
            string maxValueAsString = string.Empty;
            for (int i = 0; i < tableStructure.stateValues.Count(); i++)
            {
                if (tableStructure.stateValues[i] == State)
                {
                    var res = tableStructure.actionValues[i];
                    var max = res.Max();
                    for (int j = 0; j < res.Count(); j++)
                    {
                        if (res[j] == max)
                        {
                            if (j == 0)
                            {
                                maxValueAsString = "Up";
                                break;
                            }
                            else if (j == 1)
                            {
                                maxValueAsString = "Down";
                                break;
                            }
                            else if (j == 2)
                            {
                                maxValueAsString = "Right";
                                break;
                            }
                            else if (j == 3)
                            {
                                maxValueAsString = "Left";
                                break;
                            }
                        }
                    }
                }
            }
            return maxValueAsString;
        }

        public double MaxInt(string State)
        {
            double max = 0;
            for (int i = 0; i < tableStructure.stateValues.Count(); i++)
            {
                if (tableStructure.stateValues[i] == State)
                {
                    var res = tableStructure.actionValues[i];
                    max = res.Max();
                }
            }
            return max;
        }

        public double All(int state)
        {
            double res = 0;
            foreach (var item in tableStructure.actionValues[state])
            {
                res += item;
            }
            return res;
        }

        public void UpdateValue(string State, int Action, double NewValue)
        {
            for (int i = 0; i < tableStructure.stateValues.Count(); i++)
            {
                if (tableStructure.stateValues[i] == State)
                {
                    var res = tableStructure.actionValues[i];
                    res[Action] = NewValue;
                }
            }
        }

        public double GetValue(string State, int Action)
        {
            double rRes = 0;
            for (int i = 0; i < tableStructure.stateValues.Count(); i++)
            {
                if (tableStructure.stateValues[i] == State)
                {
                    var res = tableStructure.actionValues[i];
                    rRes = res[Action];
                }
            }
            return rRes;
        }
    }
}
