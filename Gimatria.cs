using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FindAddIn
{
    /// <summary>
    /// ממיר מספר לגימטריה
    /// </summary>
    public class Gimatria
    {
        /// <summary>
        /// פעולת גימטריה הפוך מספר לאותיות עברית
        /// </summary>
        /// <param name="number">מספר שיש להמיר לגימטריה בין 1 ל-9999</param>
        /// <param name="thousandsInLetters">האם להציג אלפים כאותיות - א = 1000 במקום תתר</param>
        /// <returns></returns>
        public static string NumToHebrew(int number, bool thousandsInLetters)
        {
            if (number > 9999 || number < 1)
            {
                //throw new Exception("המספר חייב להיות בין 1 ל-9999");
                if (number > 9999)
                {
                    while (number > 9999)
                    {
                        number -= 9999;
                    }
                }
                else if (number < 1)
                {
                    number = 1;
                }
            }

            // מקום לשמור באם נעשו יחידות - דהיינו בשביל טו טז
            bool alreadyDoneUnits = false;

            // מפרק את המספר

            // יחידות
            int units = number % 10 - number % (10 / 10);

            // עשרות
            int tens = number % 100 - number % (100 / 10);

            // מאות
            int hundreds = number % 1000 - number % (1000 / 10);

            // אלפים
            int thousands = number % 10000 - number % (10000 / 10);

            StringBuilder sb = new StringBuilder();

            // מתחיל מהאלפים
            if (thousands != 0)
            {
                // האם להציג אלפים באותיות
                if (thousandsInLetters)
                {
                    // מחלק באלף ולוקח את האות מיחידות באותיות
                    sb.Append(unitsLetters[thousands / 1000] + "'");
                }
                else
                {
                    // כל זמן שהאלפים גדול מ-400 צריך להוסיף אות ת
                    while (thousands >= 400)
                    {
                        // מוסיף ת ומחסיר 400 מהאלפים
                        sb.Append(hundredsLetters[4]);
                        thousands -= 400;
                    }
                    // בסיום מה שנשאר מוסיף למאות ומטפל בהמשך
                    if (thousands != 0) { hundreds += thousands; }
                }
            }

            // מטפל במאות
            if (hundreds != 0)
            {
                // כל זמן שהמאות גדול מ-400 צריך להוסיף אות ת
                while (hundreds >= 400)
                {
                    // מוסיף ת ומחסיר 400 מהמאות
                    sb.Append(hundredsLetters[4]);
                    hundreds -= 400;
                }
                // מה שנשאר לוקח את אות המאות המתאימה
                sb.Append(hundredsLetters[hundreds / 100]);
            }

            // מטפל בעשרות
            if (tens != 0)
            {
                // האם העשרות הן 10 והיחידות הן 5 או 6 = מדובר בטו טז וצריך לטפל כאחת
                if (tens == 10 && (units == 5 || units == 6))
                {
                    // מציין שטיפל כבר ביחידות
                    alreadyDoneUnits = true;
                    if (units == 5)
                    {
                        sb.Append("טו");
                    }
                    else
                    {
                        sb.Append("טז");
                    }
                }
                else
                {
                    // לוקח את אות העשרות המתאימה
                    sb.Append(tensLetters[tens / 10]);
                }
            }

            // אם לא טופלו היחידות מטפל בהן כעת
            if (units != 0 && !alreadyDoneUnits)
            {
                sb.Append(unitsLetters[units]);
            }

            if (sb.Length != 0)
            {
                return sb.ToString();
            }
            else
            {
                return "false";
            }
        }

        /// <summary>
        /// פעולת גימטריה הפוך רצף אותיות עברית למספר
        /// </summary>
        /// <param name="text">סטרינג להמרה למספר</param>
        /// <returns>מספר שווה ערך לגימטריה של האותיות עברית בלבד</returns>
        public static int HebrewToNum(string text)
        {
            int number = 0;

            if (text.Length == 0 || text == null)
            {
                return number;
            }

            text = CleanText(text);

            if (text.Length == 0)
            {
                return number;
            }

            // מילון גימטריה עברית למספר
            Dictionary<string, int> hebToNum = new Dictionary<string, int>();

            hebToNum.Add("א", 1);
            hebToNum.Add("ב", 2);
            hebToNum.Add("ג", 3);
            hebToNum.Add("ד", 4);
            hebToNum.Add("ה", 5);
            hebToNum.Add("ו", 6);
            hebToNum.Add("ז", 7);
            hebToNum.Add("ח", 8);
            hebToNum.Add("ט", 9);
            hebToNum.Add("י", 10);
            hebToNum.Add("כ", 20);
            hebToNum.Add("ל", 30);
            hebToNum.Add("מ", 40);
            hebToNum.Add("נ", 50);
            hebToNum.Add("ס", 60);
            hebToNum.Add("ע", 70);
            hebToNum.Add("פ", 80);
            hebToNum.Add("צ", 90);
            hebToNum.Add("ק", 100);
            hebToNum.Add("ר", 200);
            hebToNum.Add("ש", 300);
            hebToNum.Add("ת", 400);

            for (int i = 0; i < text.Length; i++)
            {
                number += hebToNum[text.Substring(i, 1)];
            }

            return number;
        }

        #region Private Functions

        /// <summary>
        /// מנקה את הטקסט ומסדר שיהיה אותיות א-ב רגילות
        /// </summary>
        /// <param name="text">טקסט לניקוי</param>
        /// <returns>מחזיר סטרינג נקי של אותיות א-ב בלבד</returns>
        private static string CleanText(string text)
        {
            string result = "";

            if (text.Length == 0 || text == null)
            {
                return result;
            }

            // מוחק כל מה שלא שייך לאותיות עברית ביוניקוד
            // הן של עברית והן של אלפבטיק פרזנטיישין פורמס
            Regex regex = new Regex("[^\u05D0-\u05F2\uFB1D-\uFB4F]");
            result = regex.Replace(text, "");

            // מילון המרה לאותיות מיוחדות
            Dictionary<string, string> alphabtic = new Dictionary<string, string>();

            alphabtic.Add("ם", "מ");
            alphabtic.Add("ן", "נ");
            alphabtic.Add("ץ", "ס");
            alphabtic.Add("ף", "ע");
            alphabtic.Add("ך", "פ");
            alphabtic.Add("\u05F0", "וו");
            alphabtic.Add("\u05F1", "וי");
            alphabtic.Add("\u05F2", "יי");
            alphabtic.Add("\uFB1D", "י");
            alphabtic.Add("\uFB1E", "");
            alphabtic.Add("\uFB1F", "יי");
            alphabtic.Add("\uFB20", "ע");
            alphabtic.Add("\uFB21", "א");
            alphabtic.Add("\uFB22", "ד");
            alphabtic.Add("\uFB23", "ה");
            alphabtic.Add("\uFB24", "כ");
            alphabtic.Add("\uFB25", "ל");
            alphabtic.Add("\uFB26", "מ");
            alphabtic.Add("\uFB27", "ר");
            alphabtic.Add("\uFB28", "ת");
            alphabtic.Add("\uFB29", "");
            alphabtic.Add("\uFB2A", "ש");
            alphabtic.Add("\uFB2B", "ש");
            alphabtic.Add("\uFB2C", "ש");
            alphabtic.Add("\uFB2D", "ש");
            alphabtic.Add("\uFB2E", "א");
            alphabtic.Add("\uFB2F", "א");
            alphabtic.Add("\uFB30", "א");
            alphabtic.Add("\uFB31", "ב");
            alphabtic.Add("\uFB32", "ג");
            alphabtic.Add("\uFB33", "ד");
            alphabtic.Add("\uFB34", "ה");
            alphabtic.Add("\uFB35", "ו");
            alphabtic.Add("\uFB36", "ז");
            alphabtic.Add("\uFB37", "");
            alphabtic.Add("\uFB38", "ט");
            alphabtic.Add("\uFB39", "י");
            alphabtic.Add("\uFB3A", "כ");
            alphabtic.Add("\uFB3B", "כ");
            alphabtic.Add("\uFB3C", "ל");
            alphabtic.Add("\uFB3D", "");
            alphabtic.Add("\uFB3E", "מ");
            alphabtic.Add("\uFB3F", "");
            alphabtic.Add("\uFB40", "נ");
            alphabtic.Add("\uFB41", "ס");
            alphabtic.Add("\uFB42", "");
            alphabtic.Add("\uFB43", "פ");
            alphabtic.Add("\uFB44", "פ");
            alphabtic.Add("\uFB45", "");
            alphabtic.Add("\uFB46", "צ");
            alphabtic.Add("\uFB47", "ק");
            alphabtic.Add("\uFB48", "ר");
            alphabtic.Add("\uFB49", "ש");
            alphabtic.Add("\uFB4A", "ת");
            alphabtic.Add("\uFB4B", "ו");
            alphabtic.Add("\uFB4C", "ב");
            alphabtic.Add("\uFB4D", "כ");
            alphabtic.Add("\uFB4E", "פ");
            alphabtic.Add("\uFB4F", "אל");

            string resultNew = "";

            for (int i = 0; i < result.Length; i++)
            {
                string value = "";
                string s = result.Substring(i, 1);
                if (alphabtic.TryGetValue(s, out value))
                {
                    resultNew += value;
                }
                else
                {
                    resultNew += s;
                }
            }

            return resultNew;
        }

        /// <summary>
        /// יחידות באותיות
        /// תא 0 ריק, תא 1 מחזיר א תא 2 ב וכו
        /// </summary>
        private static string[] unitsLetters =
        {
            "",
            "א",
            "ב",
            "ג",
            "ד",
            "ה",
            "ו",
            "ז",
            "ח",
            "ט"
        };

        /// <summary>
        /// עשרות באותיות
        /// תא 0 ריק, תא 1 מחזיר י תא 2 כ וכו
        /// </summary>
        private static string[] tensLetters =
        {
            "",
            "י",
            "כ",
            "ל",
            "מ",
            "נ",
            "ס",
            "ע",
            "פ",
            "צ",
        };

        /// <summary>
        /// מאות באותיות
        /// תא 0 ריק, תא 1 מחזיר ק תא 2 ר וכו
        /// </summary>
        private static string[] hundredsLetters =
        {
            "",
            "ק",
            "ר",
            "ש",
            "ת"
        };

        #endregion Private Functions
    }
}