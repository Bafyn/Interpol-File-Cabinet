using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Interpol_file_cabinet.Model
{
    static class Finder
    {
        /// <summary>
        /// Показывает отсортированных преступников
        /// </summary>
        /// <param name="form">Форма для отображения преступников</param>
        /// <param name="list">Список преступников для сортировки</param>
        /// <param name="cb">Выбранный параметр для поиска</param>
        /// <param name="numOfDiv">Номер панели для добавления преступника</param>
        /// <param name="searchPat">Шаблон для поиска</param>
        /// <param name="addGroupToRow">Логическое значение, указывающее, добавить ли название группировки в строку</param>
        public static void ShowSortedCriminals(MainForm form, List<Criminal> list, ComboBox cb, int numOfDiv, string searchPat,
            bool addGroupToRow)
        {
            foreach (Criminal crim in list)
            {
                switch (cb.SelectedIndex)
                {
                    case 0:
                        if (crim.Surname.ToLower().Contains(searchPat))
                            form.AddCriminal(crim, numOfDiv, false, addGroupToRow);
                        break;
                    case 1:
                        if (crim.Name.ToLower().Contains(searchPat))
                            form.AddCriminal(crim, numOfDiv, false, addGroupToRow);
                        break;
                    case 2:
                        if (crim.Patronymic.ToLower().Contains(searchPat))
                            form.AddCriminal(crim, numOfDiv, false, addGroupToRow);
                        break;
                    case 3:
                        if (crim.Nickname.ToLower().Contains(searchPat))
                            form.AddCriminal(crim, numOfDiv, false, addGroupToRow);
                        break;
                    case 4:
                        if (crim.PlaceOfBirth.ToLower().Contains(searchPat))
                            form.AddCriminal(crim, numOfDiv, false, addGroupToRow);
                        break;
                    case 5:
                        if (crim.DateOfBirth.ToLower().Contains(searchPat))
                            form.AddCriminal(crim, numOfDiv, false, addGroupToRow);
                        break;
                    case 6:
                        if (crim.Height.ToString().Contains(searchPat))
                            form.AddCriminal(crim, numOfDiv, false, addGroupToRow);
                        break;
                    case 7:
                        if (crim.Weight.ToString().Contains(searchPat))
                            form.AddCriminal(crim, numOfDiv, false, addGroupToRow);
                        break;
                    case 8:
                        if (crim.EyeColor.ToLower().Contains(searchPat))
                            form.AddCriminal(crim, numOfDiv, false, addGroupToRow);
                        break;
                    case 9:
                        if (crim.SpecialSigns.ToLower().Contains(searchPat))
                            form.AddCriminal(crim, numOfDiv, false, addGroupToRow);
                        break;
                    case 10:
                        if (crim.Profession.ToLower().Contains(searchPat))
                            form.AddCriminal(crim, numOfDiv, false, addGroupToRow);
                        break;
                    case 11:
                        if (crim.Group.ToLower().Contains(searchPat))
                            form.AddCriminal(crim, numOfDiv, false, addGroupToRow);
                        break;
                    case 12:
                        if (crim.Surname.ToLower().Contains(searchPat) || crim.Name.ToLower().Contains(searchPat) ||
                            crim.Patronymic.ToLower().Contains(searchPat) || crim.Nickname.ToLower().Contains(searchPat) ||
                            crim.PlaceOfBirth.ToLower().Contains(searchPat) || crim.DateOfBirth.ToLower().Contains(searchPat) ||
                            crim.Height.ToString() == searchPat || crim.Weight.ToString() == searchPat ||
                            crim.EyeColor.ToLower().Contains(searchPat) || crim.SpecialSigns.ToLower().Contains(searchPat) ||
                            crim.Profession.ToLower().Contains(searchPat) || crim.Group.ToLower().Contains(searchPat))
                        {
                            form.AddCriminal(crim, numOfDiv, false, addGroupToRow);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Показывает отсортированные группировки
        /// </summary>
        /// <param name="form">Форма, на которой отобразить группировки</param>
        /// <param name="searchPat">Шаблон для поиска</param>
        public static void ShowSortedGroups(MainForm form, string searchPat)
        {
            foreach (Group gr in MyCollection.groups)
            {
                if (gr.Name.ToLower().Contains(searchPat) || gr.CountOfCriminals.ToString().Contains(searchPat))
                {
                    DataGridViewRow row = ActionsWithFields.CreateRowOfGroup(gr);

                    form.AddGroupToDataGV(row);
                }
            }
        }
    }
}
