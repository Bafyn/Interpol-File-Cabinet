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
using Interpol_file_cabinet.Forms_View_;

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
            foreach (string str in new Add_Criminal().Controls.OfType<ComboBox>().First().Items)
            {
                MyCollection.professions.Add(str);
            }
        }

        public Criminal tempCr;
        public Add_Group addGr;
        public Add_Criminal addCr;
        public About about;

        /// <summary>
        /// Добавляет преступника в DataGridView
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
                    DataGridViewTextBoxCell dateOfDeath = new DataGridViewTextBoxCell();
                    dateOfDeath.Value = crim.DateOfDeath;
                    row.Cells.Add(dateOfDeath);
                    this.dataGVDead.Rows.Add(row);
                    break;
                case 3:
                    this.dataGVMembers.Rows.Add(row);
                    break;
            }
        }

        /// <summary>
        /// Изменяет строку в DataGridView после редактирования
        /// </summary>
        /// <param name="crim">Уже отредактированный преступник</param>
        public void ChangeRow(Criminal crim)
        {
            MethodsForMainForm.ChangeRowInForm(dataGVCriminals, crim);
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


        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = System.Environment.CurrentDirectory;
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
            openFileDialog1.InitialDirectory = System.Environment.CurrentDirectory;
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

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about = new About();
            about.ShowDialog();
        }


        private void btnEditCriminal_Click(object sender, EventArgs e)
        {
            if (this.dataGVCriminals.CurrentCell == null)
            {
                MessageBox.Show("Выберите преступника", "Ошибка");
                return;
            }
            // Открыть форму для редактирования преступника
            Add_Criminal ec = new Add_Criminal();

            ec.ChangeFields(dataGVCriminals.CurrentRow);
            ec.ShowDialog(this);
        }

        private void btnAddToArchive_Click(object sender, EventArgs e)
        {
            // Добавить преступника в архив
            MethodsForMainForm.MoveCriminalFromMainTable(dataGVCriminals, dataGVGroups, MyCollection.criminalsArchive, 1, this);
        }

        private void btnAddToDead_Click(object sender, EventArgs e)
        {
            // Добавить преступника к умершим
            MethodsForMainForm.MoveCriminalFromMainTable(dataGVCriminals, dataGVGroups, MyCollection.criminalsDead, 2, this);
        }

        // Добавить преступника на главную панель
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
            newCrim.DateOfDeath = DateTime.Now.ToShortDateString();
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
                        MyCollection.groups.Remove(MyCollection.groups[i]);
                        break;
                    }
                }

                for (int j = 0; j < MyCollection.criminals.Count; j++)
                {
                    if (MyCollection.criminals[j].Group == dataGVGroups.CurrentRow.Cells[0].Value.ToString())
                    {
                        ChangeGroupOfCriminalInRow(MyCollection.criminals[j], "");
                        MyCollection.criminals[j].Group = "";
                    }
                }
                dataGVGroups.Rows.Remove(dataGVGroups.CurrentRow);
                ActionsWithFields.wasChangedData = true;
            }
        }

        // Удалить преступника из группировки
        private void btnRemoveCriminalFromGroup_Click(object sender, EventArgs e)
        {
            if (dataGVMembers.CurrentCell == null)
            {
                MessageBox.Show("Выберите преступника", "Ошибка");
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
                    MyCollection.groups[i].CountOfCriminals--;
                    dataGVMembers.Rows.Remove(dataGVMembers.CurrentRow);
                    dataGVGroups.CurrentRow.Cells[1].Value = MyCollection.groups[i].CountOfCriminals;

                    break;
                }
            }

            ActionsWithFields.wasChangedData = true;
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
            dataGVMembers.Rows.Clear();

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
            dataGVMembers.Rows.Clear();

            foreach (Criminal crim in MyCollection.criminals)
            {
                if (crim.Group == dataGVGroups.CurrentRow.Cells[0].Value.ToString())
                {
                    dataGVMembers.Rows.Add(ActionsWithFields.CreateRowOfCriminal(crim, false, false));
                }
            }
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

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
