using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interpol_file_cabinet.Model;

namespace Interpol_file_cabinet.Forms
{
    public partial class Add_Group : Form
    {
        public Add_Group()
        {
            InitializeComponent();
            this.AcceptButton = btnAddGroup;
        }

        MainForm Mform;
        Group group = new Group();

        // Запись в DataGridView всех преступников, не состоящих ни в одной из группировок
        private void Add_Group_Load(object sender, EventArgs e)
        {
            foreach (Criminal crim in MyCollection.criminals)
            {
                if (crim.Group == "")
                {
                    DataGridViewRow row = ActionsWithFields.CreateRowOfCriminal(crim, true, false);

                    dataGVCriminalsWithoutGroups.Rows.Add(row);
                }
            }
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            bool isCBabs = true;

            if (textBGroupName.Text.Trim() == string.Empty || ActionsWithFields.CheckWithRegexGroupName(textBGroupName.Text))
            {
                MessageBox.Show("Проверте правильность заполнения поля", "Ошибка");
                return;
            }

            // Проверка на наличие группировки в базе
            foreach (Group gr in MyCollection.groups)
            {
                if (gr.Name.ToLower() == textBGroupName.Text.ToLower())
                {
                    if (textBGroupName.ReadOnly == true)
                    {
                        group = gr;
                        break;
                    }
                    else
                    {
                        MessageBox.Show("Группировка уже существует", "Ошибка");
                        return;
                    }
                }
            }

            Mform = (MainForm)Owner;
            group.Name = textBGroupName.Text;

            // Добавление всех отмеченных преступников в группировку
            foreach (DataGridViewRow row in dataGVCriminalsWithoutGroups.Rows)
            {
                if (row.Cells["сolumnCriminalWithoutGroupCheckB"].Value != null &&
                    row.Cells["сolumnCriminalWithoutGroupCheckB"].Value.ToString() == "True")
                {
                    Criminal crim = ActionsWithFields.ConvertToCriminalWithCheckB(row);

                    MyCollection.criminals.Find(key => key == crim).Group = textBGroupName.Text;

                    // Добавить группировку преступнику в DataGridView
                    Mform.ChangeGroupOfCriminalInRow(crim, textBGroupName.Text);

                    // Если форма открыта для добавления преступников в существующую группировку(поле с названием группировки
                    //доступно только для чтения)
                    if (textBGroupName.ReadOnly == true)
                    {
                        Mform.IncrNumOfCriminalsInGroupInDataGV();
                        Mform.AddCriminal(crim, 3, false, false);
                    }
                    group.CountOfCriminals++;
                    isCBabs = false;
                }
            }

            if (isCBabs == true)
            {
                MessageBox.Show("Вы не выбрали ни одного преступника", "Ошибка");
                return;
            }

            // Добавить строчку с группировкой в DataGridView
            if (textBGroupName.ReadOnly == false)
            {
                MyCollection.groups.Add(group);
                Mform.AddGroupToDataGV(ActionsWithFields.CreateRowOfGroup(group));
            }

            ActionsWithFields.wasChangedData = true;

            Close();
        }
    }
}
