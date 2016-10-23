using Mammatus.Enums;
using System;

namespace Mammatus.Library.Mathematics
{
    public class Arithmetics
    {
        private double CalculateExpress(string stringExpression)
        {
            while (stringExpression.IndexOf("+") != -1 || stringExpression.IndexOf("-") != -1
            || stringExpression.IndexOf("*") != -1 || stringExpression.IndexOf("/") != -1)
            {
                var strOne = "";
                var strTwo = "";
                var strTempB = "";
                double replaceValue = 0;

                var strTemp = "";
                if (stringExpression.IndexOf("*") != -1)
                {
                    strTemp = stringExpression.Substring(stringExpression.IndexOf("*") + 1, stringExpression.Length - stringExpression.IndexOf("*") - 1);
                    strTempB = stringExpression.Substring(0, stringExpression.IndexOf("*"));
                    strOne = strTempB.Substring(GetPrivorPos(strTempB) + 1, strTempB.Length - GetPrivorPos(strTempB) - 1);

                    strTwo = strTemp.Substring(0, GetNextPos(strTemp));
                    replaceValue = Convert.ToDouble(GetExpType(strOne)) * Convert.ToDouble(GetExpType(strTwo));
                    stringExpression = stringExpression.Replace(strOne + "*" + strTwo, replaceValue.ToString());
                }
                else if (stringExpression.IndexOf("/") != -1)
                {
                    strTemp = stringExpression.Substring(stringExpression.IndexOf("/") + 1, stringExpression.Length - stringExpression.IndexOf("/") - 1);
                    strTempB = stringExpression.Substring(0, stringExpression.IndexOf("/"));
                    strOne = strTempB.Substring(GetPrivorPos(strTempB) + 1, strTempB.Length - GetPrivorPos(strTempB) - 1);


                    strTwo = strTemp.Substring(0, GetNextPos(strTemp));
                    replaceValue = Convert.ToDouble(GetExpType(strOne)) / Convert.ToDouble(GetExpType(strTwo));
                    stringExpression = stringExpression.Replace(strOne + "/" + strTwo, replaceValue.ToString());
                }
                else if (stringExpression.IndexOf("+") != -1)
                {
                    strTemp = stringExpression.Substring(stringExpression.IndexOf("+") + 1, stringExpression.Length - stringExpression.IndexOf("+") - 1);
                    strTempB = stringExpression.Substring(0, stringExpression.IndexOf("+"));
                    strOne = strTempB.Substring(GetPrivorPos(strTempB) + 1, strTempB.Length - GetPrivorPos(strTempB) - 1);

                    strTwo = strTemp.Substring(0, GetNextPos(strTemp));
                    replaceValue = Convert.ToDouble(GetExpType(strOne)) + Convert.ToDouble(GetExpType(strTwo));
                    stringExpression = stringExpression.Replace(strOne + "+" + strTwo, replaceValue.ToString());
                }
                else if (stringExpression.IndexOf("-") != -1)
                {
                    strTemp = stringExpression.Substring(stringExpression.IndexOf("-") + 1, stringExpression.Length - stringExpression.IndexOf("-") - 1);
                    strTempB = stringExpression.Substring(0, stringExpression.IndexOf("-"));
                    strOne = strTempB.Substring(GetPrivorPos(strTempB) + 1, strTempB.Length - GetPrivorPos(strTempB) - 1);


                    strTwo = strTemp.Substring(0, GetNextPos(strTemp));
                    replaceValue = Convert.ToDouble(GetExpType(strOne)) - Convert.ToDouble(GetExpType(strTwo));
                    stringExpression = stringExpression.Replace(strOne + "-" + strTwo, replaceValue.ToString());
                }
            }
            return Convert.ToDouble(stringExpression);
        }

        private double CalculateExExpress(string stringExpression, MathFormula expressType)
        {
            double retValue = 0;
            switch (expressType)
            {
                case MathFormula.Sin:
                    retValue = Math.Sin(Convert.ToDouble(stringExpression));
                    break;
                case MathFormula.Cos:
                    retValue = Math.Cos(Convert.ToDouble(stringExpression));
                    break;
                case MathFormula.Tan:
                    retValue = Math.Tan(Convert.ToDouble(stringExpression));
                    break;
                case MathFormula.ATan:
                    retValue = Math.Atan(Convert.ToDouble(stringExpression));
                    break;
                case MathFormula.Sqrt:
                    retValue = Math.Sqrt(Convert.ToDouble(stringExpression));
                    break;
                case MathFormula.Pow:
                    retValue = Math.Pow(Convert.ToDouble(stringExpression), 2);
                    break;
            }
            if (retValue == 0) return Convert.ToDouble(stringExpression);
            return retValue;
        }

        private int GetNextPos(string stringExpression)
        {
            int[] ExpPos = new int[4];
            ExpPos[0] = stringExpression.IndexOf("+");
            ExpPos[1] = stringExpression.IndexOf("-");
            ExpPos[2] = stringExpression.IndexOf("*");
            ExpPos[3] = stringExpression.IndexOf("/");
            int tmpMin = stringExpression.Length;
            for (int count = 1; count <= ExpPos.Length; count++)
            {
                if (tmpMin > ExpPos[count - 1] && ExpPos[count - 1] != -1)
                {
                    tmpMin = ExpPos[count - 1];
                }
            }
            return tmpMin;
        }

        private int GetPrivorPos(string stringExpression)
        {
            int[] ExpPos = new int[4];
            ExpPos[0] = stringExpression.LastIndexOf("+");
            ExpPos[1] = stringExpression.LastIndexOf("-");
            ExpPos[2] = stringExpression.LastIndexOf("*");
            ExpPos[3] = stringExpression.LastIndexOf("/");
            int tmpMax = -1;
            for (int count = 1; count <= ExpPos.Length; count++)
            {
                if (tmpMax < ExpPos[count - 1] && ExpPos[count - 1] != -1)
                {
                    tmpMax = ExpPos[count - 1];
                }
            }
            return tmpMax;

        }

        public string SpiltExpression(string stringExpression)
        {
            string strTemp = "";
            string strExp = "";
            while (stringExpression.IndexOf("(") != -1)
            {
                strTemp = stringExpression.Substring(stringExpression.LastIndexOf("(") + 1, stringExpression.Length - stringExpression.LastIndexOf("(") - 1);
                strExp = strTemp.Substring(0, strTemp.IndexOf(")"));
                stringExpression = stringExpression.Replace("(" + strExp + ")", CalculateExpress(strExp).ToString());
            }
            if (stringExpression.IndexOf("+") != -1 || stringExpression.IndexOf("-") != -1
            || stringExpression.IndexOf("*") != -1 || stringExpression.IndexOf("/") != -1)
            {
                stringExpression = CalculateExpress(stringExpression).ToString();
            }
            return stringExpression;
        }

        private string GetExpType(string stringExpression)
        {
            stringExpression = stringExpression.ToUpper();
            if (stringExpression.IndexOf("SIN") != -1)
            {
                return CalculateExExpress(stringExpression.Substring(stringExpression.IndexOf("N") + 1, stringExpression.Length - 1 - stringExpression.IndexOf("N")), MathFormula.Sin).ToString();
            }
            if (stringExpression.IndexOf("COS") != -1)
            {
                return CalculateExExpress(stringExpression.Substring(stringExpression.IndexOf("S") + 1, stringExpression.Length - 1 - stringExpression.IndexOf("S")), MathFormula.Cos).ToString();
            }
            if (stringExpression.IndexOf("TAN") != -1)
            {
                return CalculateExExpress(stringExpression.Substring(stringExpression.IndexOf("N") + 1, stringExpression.Length - 1 - stringExpression.IndexOf("N")), MathFormula.Tan).ToString();
            }
            if (stringExpression.IndexOf("ATAN") != -1)
            {
                return CalculateExExpress(stringExpression.Substring(stringExpression.IndexOf("N") + 1, stringExpression.Length - 1 - stringExpression.IndexOf("N")), MathFormula.ATan).ToString();
            }
            if (stringExpression.IndexOf("SQRT") != -1)
            {
                return CalculateExExpress(stringExpression.Substring(stringExpression.IndexOf("T") + 1, stringExpression.Length - 1 - stringExpression.IndexOf("T")), MathFormula.Sqrt).ToString();
            }
            if (stringExpression.IndexOf("POW") != -1)
            {
                return CalculateExExpress(stringExpression.Substring(stringExpression.IndexOf("W") + 1, stringExpression.Length - 1 - stringExpression.IndexOf("W")), MathFormula.Pow).ToString();
            }
            return stringExpression;
        }
    }
}
