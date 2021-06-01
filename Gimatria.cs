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
        #region Functions

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

            for (int i = 0; i < text.Length; i++)
            {
                number += HebToNumDictionary[text.Substring(i, 1)];
            }

            return number;
        }

        #endregion Functions

        #region Convert dictionaries & strings

        /// <summary>
        /// מילון גימטריה עברית למספר
        /// </summary>
        private static readonly Dictionary<string, int> HebToNumDictionary = new Dictionary<string, int>
        {
            {"א", 1}, {"ב", 2}, {"ג", 3}, {"ד", 4}, {"ה", 5}, {"ו", 6}, {"ז", 7}, {"ח", 8}, {"ט", 9},
            {"י", 10}, {"כ", 20}, {"ל", 30}, {"מ", 40}, {"נ", 50}, {"ס", 60}, {"ע", 70}, {"פ", 80}, {"צ", 90},
            {"ק", 100}, {"ר", 200}, {"ש", 300}, {"ת", 400}
        };

        /// <summary>
        /// מילון המרה של אותיות עברית ביוניקוד לאותיות נכונות
        /// </summary>
        private static readonly Dictionary<string, string> AlphabeticDictionary = new Dictionary<string, string>
        {
            {"ם", "מ"}, {"ן", "נ"}, {"ץ", "ס"}, {"ף", "ע"}, {"ך", "פ"}, {"\u05F0", "וו"}, {"\u05F1", "וי"},
            {"\u05F2", "יי"}, {"\uFB1D", "י"}, {"\uFB1E", ""}, {"\uFB1F", "יי"},
            {"\uFB20", "ע"}, {"\uFB21", "א"}, {"\uFB22", "ד"}, {"\uFB23", "ה"}, {"\uFB24", "כ"}, {"\uFB25", "ל"}, {"\uFB26", "מ"},
            {"\uFB27", "ר"}, {"\uFB28", "ת"}, {"\uFB29", ""}, {"\uFB2A", "ש"}, {"\uFB2B", "ש"}, {"\uFB2C", "ש"}, {"\uFB2D", "ש"},
            {"\uFB2E", "א"}, {"\uFB2F", "א"}, {"\uFB30", "א"}, {"\uFB31", "ב"}, {"\uFB32", "ג"}, {"\uFB33", "ד"}, {"\uFB34", "ה"},
            {"\uFB35", "ו"}, {"\uFB36", "ז"}, {"\uFB37", ""}, {"\uFB38", "ט"}, {"\uFB39", "י"}, {"\uFB3A", "כ"}, {"\uFB3B", "כ"},
            {"\uFB3C", "ל"}, {"\uFB3D", ""}, {"\uFB3E", "מ"}, {"\uFB3F", ""}, {"\uFB40", "נ"}, {"\uFB41", "ס"}, {"\uFB42", ""},
            {"\uFB43", "פ"}, {"\uFB44", "פ"}, {"\uFB45", ""}, {"\uFB46", "צ"}, {"\uFB47", "ק"}, {"\uFB48", "ר"}, {"\uFB49", "ש"},
            {"\uFB4A", "ת"}, {"\uFB4B", "ו"}, {"\uFB4C", "ב"}, {"\uFB4D", "כ"}, {"\uFB4E", "פ"}, {"\uFB4F", "אל"}
        };

        /// <summary>
        /// יחידות באותיות
        /// תא 0 ריק, תא 1 מחזיר א תא 2 ב וכו
        /// </summary>
        private static readonly string[] unitsLetters =
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
        private static readonly string[] tensLetters =
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
        private static readonly string[] hundredsLetters =
        {
            "",
            "ק",
            "ר",
            "ש",
            "ת"
        };

        #endregion Convert dictionaries & strings

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

            string resultNew = "";

            for (int i = 0; i < result.Length; i++)
            {
                string value = "";
                string s = result.Substring(i, 1);
                if (AlphabeticDictionary.TryGetValue(s, out value))
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
    }
}
