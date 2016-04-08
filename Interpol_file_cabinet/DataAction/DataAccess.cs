using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interpol_file_cabinet.Model;

namespace Interpol_file_cabinet.DataAction
{
    static class DataAccess
    {
        /// <summary>
        /// Сохранение данных в файл
        /// </summary>
        /// <param name="fileName">Путь для сохранения файла</param>
        public static void Save(string fileName)
        {
            if (fileName.Trim() == string.Empty)
                return;

            FileStream fs = new FileStream(fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            foreach (Criminal cr in MyCollection.criminals)
            {
                string temp = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}",
                    cr.Surname, cr.Name, cr.Patronymic, cr.Nickname, cr.PlaceOfBirth, cr.DateOfBirth,
                    cr.Height, cr.Weight, cr.EyeColor, cr.SpecialSigns, cr.Profession, cr.Group);
                sw.WriteLine(temp);
            }
            sw.WriteLine("--|--");

            foreach (Criminal cr in MyCollection.criminalsArchive)
            {
                string temp = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|",
                    cr.Surname, cr.Name, cr.Patronymic, cr.Nickname, cr.PlaceOfBirth, cr.DateOfBirth,
                    cr.Height, cr.Weight, cr.EyeColor, cr.SpecialSigns, cr.Profession);
                sw.WriteLine(temp);
            }
            sw.WriteLine("--|--");

            foreach (Criminal cr in MyCollection.criminalsDead)
            {
                string temp = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|",
                    cr.Surname, cr.Name, cr.Patronymic, cr.Nickname, cr.PlaceOfBirth, cr.DateOfBirth,
                    cr.Height, cr.Weight, cr.EyeColor, cr.SpecialSigns, cr.Profession);
                sw.WriteLine(temp);
            }
            sw.WriteLine("--|--");

            foreach (Group gr in MyCollection.groups)
            {
                sw.WriteLine(gr.Name + " " + gr.criminalsInGroup.Count);
                foreach (Criminal cr in gr.criminalsInGroup)
                {
                    string temp = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}",
                        cr.Surname, cr.Name, cr.Patronymic, cr.Nickname, cr.PlaceOfBirth, cr.DateOfBirth,
                        cr.Height, cr.Weight, cr.EyeColor, cr.SpecialSigns, cr.Profession, cr.Group);
                    sw.WriteLine(temp);
                }
                sw.WriteLine("--*--");
            }

            ActionsWithFields.wasChangedData = false;
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// Загрузка данных из файла
        /// </summary>
        /// <param name="fileName">Путь к файлу</param>
        /// <returns>Логическое значение, возвращающее успешность загрузки</returns>
        public static bool Load(string fileName)
        {
            try
            {
                if (fileName.Trim() == string.Empty)
                    return false;
                FileStream fs = new FileStream(fileName, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                Criminal crim = new Criminal();
                Group group = new Group();
                int numOfPart = 0;

                MyCollection.criminals.Clear();
                MyCollection.criminalsArchive.Clear();
                MyCollection.criminalsDead.Clear();
                MyCollection.groups.Clear();

                while (!sr.EndOfStream)
                {
                    string[] tempStr = sr.ReadLine().Split('|');

                    while (tempStr[0] == "--")
                    {
                        string temp = sr.ReadLine();
                        if (temp == null)
                            break;
                        tempStr = temp.Split('|');
                        numOfPart++;
                    }

                    while (tempStr[0] == "--*--")
                    {
                        group.criminalsInGroup.Clear();
                        string temp = sr.ReadLine();
                        if (temp == null)
                            break;
                        tempStr = temp.Split('|');
                    }

                    if (sr.EndOfStream)
                        break;

                    if (tempStr.Length != 1)
                    {
                        crim = new Criminal(tempStr[0], tempStr[1], tempStr[2], tempStr[3], tempStr[4], tempStr[5],
                            Convert.ToDouble(tempStr[6]), Convert.ToDouble(tempStr[7]), tempStr[8], tempStr[9], tempStr[10]);
                        if (numOfPart == 0)
                            crim.Group = tempStr[11];
                    }

                    if (numOfPart == 3 && tempStr.Length == 1)
                    {
                        int numOfCrims = Convert.ToInt32(tempStr[0].Split('$')[1]);

                        group.Name = tempStr[0].Split('$')[0];

                        for (int i = 0; i < numOfCrims; i++)
                        {
                            tempStr = sr.ReadLine().Split('|');

                            crim = new Criminal(tempStr[0], tempStr[1], tempStr[2], tempStr[3], tempStr[4], tempStr[5],
                                Convert.ToDouble(tempStr[6]), Convert.ToDouble(tempStr[7]), tempStr[8], tempStr[9], tempStr[10]);
                            crim.Group = tempStr[11];

                            group.criminalsInGroup.Add(crim);
                        }

                    }

                    switch (numOfPart)
                    {
                        case 0:
                            MyCollection.criminals.Add(crim);
                            break;
                        case 1:
                            MyCollection.criminalsArchive.Add(crim);
                            break;
                        case 2:
                            MyCollection.criminalsDead.Add(crim);
                            break;
                        case 3:
                            MyCollection.groups.Add(new Group(group));
                            break;
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
