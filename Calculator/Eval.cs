using Calculator.Calculator.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Calculator
{
    internal class Eval
    {
        private static List<int[]> GetIndexes(string equation)
        {
            List<int[]> result = new List<int[]>();
            int count1 = 0; // count of (
            int count2 = 0; // count of )
            for (int i = 0; i <= equation.Length - 1; i++)
            {
                if (equation[i] == '(')
                {
                    if (count1 - count2 == 0)
                    {
                        result.Add(new int[2] { i, 0 });
                    }
                    count1 += 1;
                }
                else if (equation[i] == ')')
                {
                    if (count1 - count2 == 1)
                    {
                        result[result.Count - 1][1] = i;
                    }
                    count2 += 1;
                }
            }
            return result;
        }
        private static List<IndexOfAction> GetValues(string equation)
        {
            List<IndexOfAction> values = new();
            foreach (var curAct in action)
            {
                string temp = equation;
                foreach (var chr in curAct)
                {
                    while (temp.IndexOf(chr) != -1)
                    {
                        int ind = temp.IndexOf(chr);
                        values.Add(new IndexOfAction { action = chr, index = ind });
                        temp = temp.Remove(ind, 1);
                    }
                }
            }
            var actGroup1 = values.Where(m => action[0].ToList().IndexOf(m.action) != -1).OrderBy(m => m.index);
            var actGroup2 = values.Where(m => action[1].ToList().IndexOf(m.action) != -1).OrderBy(m => m.index);

            values = new();
            values.AddRange(actGroup1);
            values.AddRange(actGroup2);
            return values;
        }
        private static char[][] action = new char[][] { new char[] { '*', '/' }, new char[] { '+', '-' } };

        /// <summary>
        /// Solving for equation resursively
        /// </summary>
        /// <param name="equation"></param>
        /// <returns></returns>
        public static string Parse(string equation)
        {
            equation = equation.Replace(" ", "");

            List<int[]> indexes = GetIndexes(equation);
            if (indexes.Count >= 1)
            {
                foreach (var index in indexes)
                {
                    int start = index[0];
                    int end = index[1] - index[0] + 1;
                    string temp = equation.Substring(start, end);
                    string ans = Parse(string.Join("", temp.Skip(1).SkipLast(1).ToArray()));
                    equation = equation.Remove(start, end).Insert(start, ans);
                }
            }
            indexes = GetIndexes(equation);
            if (indexes.Count == 0)
            {

                int i = 0;
                while(true)
                {
                    var values = GetValues(equation);

                    if (values.Count == 0)
                        break;

                    var val = values[i];
                    char act = val.action;

                    int strIndex = 0;
                    int index = equation.IndexOf(act);
                    if (index == 0)
                        break;

                    string lNumString = "", rNumString = "";
                    double lNum, rNum;
                    int[] borderIndex = new int[2];
                    bool wasDigit = false;
                    strIndex = index - 1;
                    while (strIndex >= 0)
                    {
                        if (char.IsDigit(equation[strIndex]) || equation[strIndex] == ',')
                        {
                            wasDigit = true;
                            if (wasDigit)
                                lNumString = lNumString.Insert(0, $"{equation[strIndex]}");
                            if (strIndex == 0)
                            {
                                borderIndex[0] = strIndex;
                                break;
                            }
                        }
                        else
                        {
                            if (wasDigit)
                            {
                                borderIndex[0] = strIndex + 1;
                                break;
                            }
                        }
                        strIndex -= 1;
                    }
                    lNum = double.Parse(lNumString);

                    strIndex = index + 1;
                    wasDigit = false;
                    while (strIndex >= 0)
                    {
                        if (char.IsDigit(equation[strIndex]) || equation[strIndex] == ',')
                        {
                            wasDigit = true;
                            if (wasDigit)
                                rNumString += equation[strIndex];
                            if (strIndex == equation.Length - 1)
                            {
                                borderIndex[1] = strIndex;
                                break;
                            }
                        }
                        else
                        {
                            if (wasDigit)
                            {
                                borderIndex[1] = strIndex - 1;
                                break;
                            }
                        }
                        strIndex += 1;
                    }
                    rNum = double.Parse(rNumString);

                    double answerOfCurrentEquation = act switch
                    {
                        '*' => lNum * rNum,
                        '/' => lNum / rNum,
                        '+' => lNum + rNum,
                        '-' => lNum - rNum,
                    };
                    equation = equation.Remove(borderIndex[0], borderIndex[1] - borderIndex[0] + 1).Insert(borderIndex[0], $"{answerOfCurrentEquation}");

                    //убираем двойные знаки
                    for (int i1 = 1; i1 <= equation.Length - 1; i1++)
                    {
                        char chr = equation[i1];
                        char previouschr = equation[i1 - 1];
                        if (action[1].Contains(chr))
                        {
                            if (action[1].Contains(previouschr))
                            {
                                equation = equation.Remove(i1 - 1, 1);
                                i1--;
                            }
                            else if (action[0].Contains(previouschr))
                            {
                                equation = equation.Remove(i1, 1);
                                equation.Insert(0, $"{previouschr}");
                            }

                        }
                    }
                }
                return equation;
            }
            return equation;
            //Console.WriteLine(equation);
            //equation = SoulveEquation_Recursive(equation);
        }

    }
}
/*
 * 2+(2+(2+(2+3)))
 * 
 */