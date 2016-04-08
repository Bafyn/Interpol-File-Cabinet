using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interpol_file_cabinet.Forms;
using Interpol_file_cabinet.Model;
using Interpol_file_cabinet.DataAction;

namespace Interpol_file_cabinet
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            comboBSearchMain.SelectedIndex = 0;
            comboBSearchArchive.SelectedIndex = 0;
            comboBSearchDead.SelectedIndex = 0;
        }

        public Criminal tempCr;
        public Add_Group addGr;
        public Add_Criminal addCr;

        private void добавитьПреступникаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Открыть форму для добавления преступника
            addCr = new Add_Criminal();
            addCr.ShowDialog(this);
        }

        private void добавитьГруппировкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Открыть форму для добавления группировки
            addGr = new Add_Group();
            addGr.ShowDialog(this);
        }

        /// <summary>
        /// Добавить преступника в DataGridView
        /// </summary>
        /// <param name="crim">объект типа Criminal</param>
        /// <param name="numOfDivision">Число, указывающее, куда записать преступника(принимает занчения от 0 до 3)</param>
        /// <param name="addToList">Логическое значение, указывающее, добавить ли преступника в коллекцию MyCollection</param>
        /// <param name="addGroupToRow">Логическое значение, указывающее, добавить ли в строку название группировки</param>
        public void AddCriminal(Criminal crim, int numOfDivision, bool addToList, bool addGroupToRow)
        {
            // Создать строку с преступником
            DataGridViewRow row = ActionsWithFields.CreateRowOfCriminal(crim, false, addGroupToRow);

            if (addToList == true)
            {
                switch (numOfDivision)
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
                }
            }

            switch (numOfDivision)
            {
                case 0:
                    this.dataGVCriminals.Rows.Add(row);
                    break;
                case 1:
                    this.dataGVArchive.Rows.Add(row);
                    break;
                case 2:
                    this.dataGVDead.Rows.Add(row);
                    break;
                case 3:
                    this.dataGVMembers.Rows.Add(row);
                    break;
            }
        }

        /// <summary>
        /// Добавляет строку группировки в DataGridView
        /// </summary>
        /// <param name="row">Строка для добавления</param>
        public void AddGroupToDataGV(DataGridViewRow row)
        {
            this.dataGVGroups.Rows.Add(row);
        }

        /// <summary>
        ///  Увеличивает количество преступников в группировке на 1
        /// </summary>
        public void IncrNumOfCriminalsInGroupInDataGV()
        {
            dataGVGroups.CurrentRow.Cells[1].Value = Convert.ToInt32(dataGVGroups.CurrentRow.Cells[1].Value) + 1;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // При закрытии формы выдать предупреждение о сохранении данных
            if (ActionsWithFields.wasChangedData == true)
            {
                DialogResult f = MessageBox.Show("В случае закрытия все изменения будут отменены. Сохранить изменения?", "Картотека Интерпола", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (f == DialogResult.Yes)
                {
                    // Симулировать вызов события для сохранения файла
                    сохранитьToolStripMenuItem_Click(null, null);
                }
                if (f == DialogResult.Cancel)
                    e.Cancel = true;
            }
            else
            {
                DialogResult dr = MessageBox.Show("Вы уверены?", "Картотека Интерпола", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                    e.Cancel = true;
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = "D:\\Data";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.Title = "Сохранить файл";
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                // Сохранить данные в файл
                DataAccess.Save(saveFileDialog1.FileName);
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "D:\temp";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 0;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.Title = "Открыть файл";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                dataGVArchive.Rows.Clear();
                dataGVCriminals.Rows.Clear();
                dataGVDead.Rows.Clear();
                dataGVGroups.Rows.Clear();
                dataGVMembers.Rows.Clear();

                // Загрузить данные с файла
                if (!DataAccess.Load(openFileDialog1.FileName))
                {
                    MessageBox.Show("Неверный формат файла для загрузки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Записать всех преступников в DataGridView
                    foreach (Criminal cr in MyCollection.criminals)
                        AddCriminal(cr, 0, false, true);
                    foreach (Criminal cr in MyCollection.criminalsArchive)
                        AddCriminal(cr, 1, false, false);
                    foreach (Criminal cr in MyCollection.criminalsDead)
                        AddCriminal(cr, 2, false, false);
                    foreach (Group gr in MyCollection.groups)
                    {
                        DataGridViewRow row = ActionsWithFields.CreateRowOfGroup(gr);

                        this.dataGVGroups.Rows.Add(row);
                    }
                }
            }
        }

        private void btnEditCriminal_Click(object sender, EventArgs e)
        {
            if (this.dataGVCriminals.CurrentCell == null)
            {
                MessageBox.Show("Выберите преступника", "Ошибка");
                return;
            }
            // Открыть форму для редактирования преступника
            Edit_Criminal ec = new Edit_Criminal();
            ec.ChangeFields(dataGVCriminals.CurrentRow);
            ec.ShowDialog(this);
        }

        /// <summary>
        /// Изменяет строку в DataGridView после редактирования
        /// </summary>
        /// <param name="crim">Уже отредактированный преступник</param>
        public void ChangeRow(Criminal crim)
        {
            // Найти индекс преступника, который был отредактирован
            int ind = MyCollection.criminals.FindIndex(key =>
                key == ActionsWithFields.ConvertToCriminal(dataGVCriminals.CurrentRow));
            MyCollection.criminals.RemoveAt(ind);

            dataGVCriminals.CurrentRow.Cells[0].Value = crim.Surname;
            dataGVCriminals.CurrentRow.Cells[1].Value = crim.Name;
            dataGVCriminals.CurrentRow.Cells[2].Value = crim.Patronymic;
            dataGVCriminals.CurrentRow.Cells[3].Value = crim.Nickname;
            dataGVCriminals.CurrentRow.Cells[4].Value = crim.PlaceOfBirth;
            dataGVCriminals.CurrentRow.Cells[5].Value = crim.DateOfBirth;
            dataGVCriminals.CurrentRow.Cells[6].Value = crim.Height;
            dataGVCriminals.CurrentRow.Cells[7].Value = crim.Weight;
            dataGVCriminals.CurrentRow.Cells[8].Value = crim.EyeColor;
            dataGVCriminals.CurrentRow.Cells[9].Value = crim.SpecialSigns;
            dataGVCriminals.CurrentRow.Cells[10].Value = crim.Profession;

            // Добавить уже отредактированного преступника
            MyCollection.criminals.Insert(ind, crim);
        }

        /// <summary>
        /// Изменяет группу преступника в DataGridView
        /// </summary>
        /// <param name="crim">Преступник, у которого поменялась группировка</param>
        /// <param name="nameOfGroup">Имя новой группировки</param>
        public void ChangeGroupOfCriminalInRow(Criminal crim, string nameOfGroup)
        {
            foreach (DataGridViewRow row in dataGVCriminals.Rows)
            {
                if (ActionsWithFields.ConvertToCriminal(row) == crim)
                    row.Cells[11].Value = nameOfGroup;
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void btnAddToArchive_Click(object sender, EventArgs e)
        {
            // Добавить преступника в архив
            MoveCriminalFromMainTable(MyCollection.criminalsArchive, 1);
        }

        private void btnAddToDead_Click(object sender, EventArgs e)
        {
            // Добавить преступника к умершим
            MoveCriminalFromMainTable(MyCollection.criminalsDead, 2);
        }

        /// <summary>
        /// Переместить преступника с главного DataGridView
        /// </summary>
        /// <param name="list">Новое место расположения преступника</param>
        /// <param name="numOfDiv">Номер пнаели, куда поместить преступника</param>
        private void MoveCriminalFromMainTable(List<Criminal> list, int numOfDiv)
        {
            if (this.dataGVCriminals.CurrentCell == null)
            {
                MessageBox.Show("Выберите преступника", "Ошибка");
                return;
            }

            Criminal newCrim = MyCollection.criminals.Find(key => key == ActionsWithFields.ConvertToCriminal(dataGVCriminals.CurrentRow));

            // Удалить перступника с главной панели
            MyCollection.criminals.Remove(newCrim);

            // Если преступник состоит в группировке, удалить его и оттуда
            if (newCrim.Group != "")
            {
                foreach (Group group in MyCollection.groups)
                {
                    if (group.Name == newCrim.Group)
                    {
                        group.criminalsInGroup.Remove(newCrim);
                        break;
                    }
                }

                //Уменьшить кол-во преступников в группировке на 1
                foreach (DataGridViewRow row in dataGVGroups.Rows)
                {
                    if (row.Cells[0].Value.ToString() == newCrim.Group)
                        row.Cells[1].Value = (Convert.ToInt32(row.Cells[1].Value) - 1).ToString();
                }
            }

            newCrim.Group = "";
            list.Add(newCrim);
            AddCriminal(newCrim, numOfDiv, false, false);
            dataGVCriminals.Rows.Remove(dataGVCriminals.CurrentRow);

            ActionsWithFields.wasChangedData = true;
        }

        // Добавть преступника на главную панель
        private void btnAddToActive_Click(object sender, EventArgs e)
        {
            if (this.dataGVArchive.CurrentCell == null)
            {
                MessageBox.Show("Выберите преступника", "Ошибка");
                return;
            }

            Criminal newCrim = MyCollection.criminalsArchive.Find(key => key == ActionsWithFields.ConvertToCriminal(dataGVArchive.CurrentRow));

            MyCollection.criminalsArchive.Remove(newCrim);
            MyCollection.criminals.Add(newCrim);
            AddCriminal(newCrim, 0, false, true);
            dataGVArchive.Rows.Remove(dataGVArchive.CurrentRow);

            ActionsWithFields.wasChangedData = true;
        }

        // Добавить преступника к умершим из архива
        private void btnAddToDeadFromArchive_Click(object sender, EventArgs e)
        {
            if (this.dataGVArchive.CurrentCell == null)
            {
                MessageBox.Show("Выберите преступника", "Ошибка");
                return;
            }

            Criminal newCrim = MyCollection.criminalsArchive.Find(key => key == ActionsWithFields.ConvertToCriminal(dataGVArchive.CurrentRow));
            MyCollection.criminalsArchive.Remove(newCrim);
            MyCollection.criminalsDead.Add(newCrim);
            AddCriminal(newCrim, 2, false, false);
            dataGVArchive.Rows.Remove(dataGVArchive.CurrentRow);

            ActionsWithFields.wasChangedData = true;
        }

        // Удалить преступника
        private void btnRemoveCriminal_Click(object sender, EventArgs e)
        {
            if (this.dataGVDead.CurrentCell == null)
            {
                MessageBox.Show("Выберите преступника", "Ошибка");
                return;
            }

            DialogResult DR = MessageBox.Show("Удалить преступника?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (DR == DialogResult.Yes)
            {
                MyCollection.criminalsDead.RemoveAll(key =>
                    key == ActionsWithFields.ConvertToCriminal(dataGVDead.CurrentRow));
                dataGVDead.Rows.Remove(dataGVDead.CurrentRow);

                ActionsWithFields.wasChangedData = true;
            }
        }


        // Найти преступников на главной панели
        private void btnFindMain_Click(object sender, EventArgs e)
        {
            string searchPattern = textBSearchPatternMain.Text.ToLower();

            if (searchPattern.Trim() == string.Empty)
                return;

            dataGVCriminals.Rows.Clear();

            Finder.ShowSortedCriminals(this, MyCollection.criminals, comboBSearchMain, 0, searchPattern, true);
        }

        // Найти преступников на среди тех, кто в архиве
        private void btnFindArchive_Click(object sender, EventArgs e)
        {
            string searchPattern = textBSearchPatternArchive.Text.ToLower();

            if (searchPattern.Trim() == string.Empty)
                return;

            dataGVArchive.Rows.Clear();

            Finder.ShowSortedCriminals(this, MyCollection.criminalsArchive, comboBSearchArchive, 1, searchPattern, false);
        }

        // Найти преступников на среди умерших
        private void btnFindDead_Click(object sender, EventArgs e)
        {
            string searchPattern = textBSearchPatternDead.Text.ToLower();

            if (searchPattern.Trim() == string.Empty)
                return;

            dataGVDead.Rows.Clear();

            Finder.ShowSortedCriminals(this, MyCollection.criminalsDead, comboBSearchDead, 2, searchPattern, false);
        }

        // Найти группировки
        private void btnFindGroups_Click(object sender, EventArgs e)
        {
            string searchPattern = textBSearchPatternGroups.Text.ToLower();

            if (searchPattern.Trim() == string.Empty)
                return;

            dataGVGroups.Rows.Clear();

            Finder.ShowSortedGroups(this, searchPattern);
        }

        // Сбрасывает поле для поиска на главной панели
        private void btnResetSearchMain_Click(object sender, EventArgs e)
        {
            dataGVCriminals.Rows.Clear();

            foreach (Criminal crim in MyCollection.criminals)
            {
                AddCriminal(crim, 0, false, true);
            }

            textBSearchPatternMain.Text = "";
        }

        // Сбрасывает поле для поиска на панели с архивированными преступниками
        private void btnResetSearchArchive_Click(object sender, EventArgs e)
        {
            dataGVArchive.Rows.Clear();

            foreach (Criminal crim in MyCollection.criminalsArchive)
            {
                AddCriminal(crim, 1, false, false);
            }

            textBSearchPatternArchive.Text = "";
        }

        // Сбрасывает поле для поиска на панели с умершими преступниками
        private void btnResetSearchDead_Click(object sender, EventArgs e)
        {
            dataGVDead.Rows.Clear();

            foreach (Criminal crim in MyCollection.criminalsDead)
            {
                AddCriminal(crim, 2, false, false);
            }

            textBSearchPatternDead.Text = "";
        }

        // Сбрасывает поле для поиска на панели с группировками
        private void btnResetSearchGroups_Click(object sender, EventArgs e)
        {
            dataGVGroups.Rows.Clear();

            foreach (Group gr in MyCollection.groups)
            {
                AddGroupToDataGV(ActionsWithFields.CreateRowOfGroup(gr));
            }

            textBSearchPatternGroups.Text = "";
        }


        // Показать преступников, состоящих в группировке, при выборе группировки
        private void dataGVGroups_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            Group group = new Group();

            dataGVMembers.Rows.Clear();

            foreach (Group gr in MyCollection.groups)
            {
                if (gr.Name == dataGVGroups.CurrentRow.Cells[0].Value.ToString())
                {
                    group = gr;
                    break;
                }
            }

            foreach (Criminal crim in group.criminalsInGroup)
            {
                dataGVMembers.Rows.Add(ActionsWithFields.CreateRowOfCriminal(crim, false, false));
            }
        }

        // Удалить группировку
        private void btnRemoveGroup_Click(object sender, EventArgs e)
        {
            if (dataGVGroups.CurrentCell == null)
            {
                MessageBox.Show("Выберите группировку", "Ошибка");
                return;
            }

            DialogResult DR = MessageBox.Show("Вы действительно хотите удалить группировку?", "Подтверждение",
                MessageBoxButtons.YesNo);

            if (DR == DialogResult.Yes)
            {
                for (int i = 0; i < MyCollection.groups.Count; i++)
                {
                    if (MyCollection.groups[i].Name == dataGVGroups.CurrentRow.Cells[0].Value.ToString())
                    {
                        foreach (Criminal crim in MyCollection.groups[i].criminalsInGroup)
                        {
                            MyCollection.criminals.Find(key => key == crim).Group = "";

                            ChangeGroupOfCriminalInRow(crim, "");
                        }

                        MyCollection.groups.Remove(MyCollection.groups[i]);
                        dataGVGroups.Rows.Remove(dataGVGroups.CurrentRow);

                        break;
                    }
                }
            }
        }

        // Удалить преступника с группировки
        private void btnRemoveCriminalFromGroup_Click(object sender, EventArgs e)
        {
            if (dataGVMembers.CurrentCell == null)
            {
                MessageBox.Show("Выберите преступника для удаления", "Ошибка");
                return;
            }

            for (int i = 0; i < MyCollection.groups.Count; i++)
            {
                if (MyCollection.groups[i].Name == dataGVGroups.CurrentRow.Cells[0].Value.ToString())
                {
                    Criminal crim = ActionsWithFields.ConvertToCriminal(dataGVMembers.CurrentRow);
                    crim.Group = dataGVGroups.CurrentRow.Cells[0].Value.ToString();

                    MyCollection.criminals.Find(key => key == crim).Group = "";
                    ChangeGroupOfCriminalInRow(crim, "");
                    MyCollection.groups[i].criminalsInGroup.Remove(crim);
                    dataGVMembers.Rows.Remove(dataGVMembers.CurrentRow);
                    dataGVGroups.CurrentRow.Cells[1].Value = Convert.ToInt32(dataGVGroups.CurrentRow.Cells[1].Value) - 1;

                    break;
                }
            }
        }

        // Добавить преступника в группировку
        private void btnAddCriminalToGroup_Click(object sender, EventArgs e)
        {
            if (dataGVGroups.CurrentCell == null)
            {
                MessageBox.Show("Выберите группировку", "Ошибка");
                return;
            }

            Add_Group AG = new Add_Group();

            TextBox tb = (TextBox)AG.Controls.Find("textBGroupName", false)[0];
            tb.ReadOnly = true;
            tb.Text = dataGVGroups.CurrentRow.Cells[0].Value.ToString();

            // Открыть форму для выбора преступников
            AG.ShowDialog(this);
        }
    }
}
