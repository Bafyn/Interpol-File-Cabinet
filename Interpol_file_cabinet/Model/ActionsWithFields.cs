using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interpol_file_cabinet.Model
{
    static class ActionsWithFields
    {
        // Флажок для проверки, были ли изменены данные.
        public static bool wasChangedData = false;

        /// <summary>
        /// Проверяет соответствие текста регулярному выражению (все, кроме букв)
        /// </summary>
        /// <param name="str">Строка, которую нужно проверить</param>
        /// <returns>Логическое значение</returns>
        public static bool CheckWithRegex(string str)
        {
            // ^\w - кроме любого текстового символа, не являющегося пробелом, символом табуляции и т.п.; [0-9] - любая цифра.
            Regex exp = new Regex(@"[0-9]|[^\w]");
            return exp.IsMatch(str);
        }

        /// <summary>
        /// Проверяет соответствие текста регулярному выражению (кроме букв и пробелов)
        /// </summary>
        /// <param name="str">Строка, которую нужно проверить</param>
        /// <returns>Логическое значение</returns>
        public static bool CheckWithRegexWithSpaces(string str)
        {
            // ^\s - кроме пробела; [0-9] - любая цифра.
            Regex exp = new Regex(@"[0-9]|[^\s\w]");
            return exp.IsMatch(str);
        }

        /// <summary>
        /// Проверяет соответствие названия группы регулярному выражению (кроме букв, пробелов и цифр)
        /// </summary>
        /// <param name="str">Название группы</param>
        /// <returns>Логическое значение</returns>
        public static bool CheckWithRegexGroupName(string str)
        {
            Regex exp = new Regex(@"[^\s\w]");
            return exp.IsMatch(str);
        }

        /// <summary>
        /// Конвертирует строку DataGridViewRow в Criminal
        /// </summary>
        /// <param name="row">Строка DataGridView</param>
        /// <returns>Объект Criminal</returns>
        public static Criminal ConvertToCriminal(DataGridViewRow row)
        {
            Criminal crim = new Criminal(row.Cells[0].Value.ToString(), row.Cells[1].Value.ToString(),
                row.Cells[2].Value.ToString(), row.Cells[3].Value.ToString(), row.Cells[4].Value.ToString(),
                row.Cells[5].Value.ToString(), Convert.ToDouble(row.Cells[6].Value), Convert.ToDouble(row.Cells[7].Value),
                row.Cells[8].Value.ToString(), row.Cells[9].Value.ToString(), row.Cells[10].Value.ToString());
            if (row.Cells.Count == 11)
            {
                crim.Group = "";
            }
            else
            {
                crim.Group = row.Cells[11].Value == null ? "" : row.Cells[11].Value.ToString();
            }

            return crim;
        }

        /// <summary>
        /// Переводит строку DataGridView, содержащую CheckBox, в Criminal
        /// </summary>
        /// <param name="row">Строка DataGridView с CheckBox</param>
        /// <returns>Объект Criminal</returns>
        public static Criminal ConvertToCriminalWithCheckB(DataGridViewRow row)
        {
            Criminal crim = new Criminal(row.Cells[1].Value.ToString(), row.Cells[2].Value.ToString(),
               row.Cells[3].Value.ToString(), row.Cells[4].Value.ToString(), row.Cells[5].Value.ToString(),
               row.Cells[6].Value.ToString(), Convert.ToDouble(row.Cells[7].Value), Convert.ToDouble(row.Cells[8].Value),
               row.Cells[9].Value.ToString(), row.Cells[10].Value.ToString(), row.Cells[11].Value.ToString());

            return crim;
        }

        /// <summary>
        /// Конвертирует объект типа Criminal в DataGridViewRow
        /// </summary>
        /// <param name="cr">Объект типа Criminal</param>
        /// <param name="addCheckBox">Логическое значение, указывающее, стоит ли добавить CheckBox в строку</param>
        /// <param name="addGroup">Логическое значение, указывающее, стоит ли добавить ячейку с названием группы в строку</param>
        /// <returns>DataGridViewRow</returns>
        public static DataGridViewRow CreateRowOfCriminal(Criminal cr, bool addCheckBox, bool addGroup)
        {
            DataGridViewRow row = new DataGridViewRow();

            DataGridViewCell surname = new DataGridViewTextBoxCell();
            surname.Value = cr.Surname;
            DataGridViewCell name = new DataGridViewTextBoxCell();
            name.Value = cr.Name;
            DataGridViewCell patron = new DataGridViewTextBoxCell();
            patron.Value = cr.Patronymic;
            DataGridViewCell nick = new DataGridViewTextBoxCell();
            nick.Value = cr.Nickname;
            DataGridViewCell placeOfB = new DataGridViewTextBoxCell();
            placeOfB.Value = cr.PlaceOfBirth;
            DataGridViewCell dateOfB = new DataGridViewTextBoxCell();
            dateOfB.Value = cr.DateOfBirth;
            DataGridViewCell height = new DataGridViewTextBoxCell();
            height.Value = cr.Height;
            DataGridViewCell weight = new DataGridViewTextBoxCell();
            weight.Value = cr.Weight;
            DataGridViewCell eye = new DataGridViewTextBoxCell();
            eye.Value = cr.EyeColor;
            DataGridViewCell signs = new DataGridViewTextBoxCell();
            signs.Value = cr.SpecialSigns;
            DataGridViewCell prof = new DataGridViewTextBoxCell();
            prof.Value = cr.Profession;

            if (addCheckBox == true)
            {
                DataGridViewCell chB = new DataGridViewCheckBoxCell();
                row.Cells.Add(new DataGridViewCheckBoxCell());
            }

            row.Cells.Add(surname);
            row.Cells.Add(name);
            row.Cells.Add(patron);
            row.Cells.Add(nick);
            row.Cells.Add(placeOfB);
            row.Cells.Add(dateOfB);
            row.Cells.Add(height);
            row.Cells.Add(weight);
            row.Cells.Add(eye);
            row.Cells.Add(signs);
            row.Cells.Add(prof);

            if (addGroup == true)
            {
                DataGridViewCell group = new DataGridViewTextBoxCell();
                group.Value = cr.Group;
                row.Cells.Add(group);
            }

            row.Height = 30;

            return row;
        }

        /// <summary>
        /// Конвертирует объект типа Group в DataGridViewRow
        /// </summary>
        /// <param name="gr">Объект типа Group</param>
        /// <returns>DataGridViewRow</returns>
        public static DataGridViewRow CreateRowOfGroup(Group gr)
        {
            DataGridViewRow row = new DataGridViewRow();

            DataGridViewCell name = new DataGridViewTextBoxCell();
            name.Value = gr.Name;
            DataGridViewCell num = new DataGridViewTextBoxCell();
            num.Value = gr.CountOfCriminals;

            row.Cells.Add(name);
            row.Cells.Add(num);
            row.Height = 40;

            return row;
        }
    }
}
