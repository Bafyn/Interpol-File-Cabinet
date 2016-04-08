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
    public partial class Edit_Criminal : Form
    {
        public Edit_Criminal()
        {
            InitializeComponent();
            this.AcceptButton = btnEditCriminal;
            this.dateTCriminalDateOfBirth.MaxDate = DateTime.Now;
        }

        /// <summary>
        /// Заполняет поля формы значениями строки, выбранной для редактирования
        /// </summary>
        /// <param name="row">Строка DataGridView, с которой берутся данные</param>
        public void ChangeFields(DataGridViewRow row)
        {
            this.textBCriminalSurname.Text = row.Cells[0].Value.ToString();
            this.textBCriminalName.Text = row.Cells[1].Value.ToString();
            this.textBCriminalPatronymic.Text = row.Cells[2].Value.ToString();
            this.textBCriminalNickname.Text = row.Cells[3].Value.ToString();
            this.textBCriminalPlaceOfBirth.Text = row.Cells[4].Value.ToString();
            this.dateTCriminalDateOfBirth.Value = Convert.ToDateTime(row.Cells[5].Value);
            this.numericCriminalHeight.Text = row.Cells[6].Value.ToString();
            this.numericCriminalWeight.Text = row.Cells[7].Value.ToString();
            this.textBCriminalEyeColor.Text = row.Cells[8].Value.ToString();
            this.textBCriminalSigns.Text = row.Cells[9].Value.ToString();
            this.textBCriminalProfession.Text = row.Cells[10].Value.ToString();
        }

        private void btnEditCriminal_Click(object sender, EventArgs e)
        {
            int flagErrorInFields = 0; // Флаг, указывающий на наличие ошибок в полях для ввода

            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                if (tb.Text.Trim() == string.Empty || ActionsWithFields.CheckWithRegex(tb.Text))
                {
                    if (tb.Name == "textBCriminalSigns")
                        continue;
                    tb.BackColor = Color.LightPink; // Изменение цвета фона в случае ошибки
                    flagErrorInFields = 1;
                }
                else
                {
                    tb.BackColor = Color.White;
                }
            }

            if (textBCriminalSigns.Text.Trim() == string.Empty || ActionsWithFields.CheckWithRegexWithSpaces(textBCriminalSigns.Text))
            {
                textBCriminalSigns.BackColor = Color.LightPink;
                flagErrorInFields = 1;
            }
            else
            {
                textBCriminalSigns.BackColor = Color.White;
            }

            if (flagErrorInFields == 1)
            {
                MessageBox.Show("Проверьте правильность заполения всех полей", "Ошибка");
                return;
            }

            // Проверка отредактированного преступника на наличие в базе
            foreach (Criminal cr in MyCollection.criminals)
            {
                if (cr.Surname.ToLower() == textBCriminalSurname.Text.ToLower() &&
                    cr.Name.ToLower() == textBCriminalName.Text.ToLower() &&
                    cr.Patronymic.ToLower() == textBCriminalPatronymic.Text.ToLower() &&
                    cr.PlaceOfBirth.ToLower() == textBCriminalPlaceOfBirth.Text.ToLower() &&
                    cr.DateOfBirth == dateTCriminalDateOfBirth.Value.ToShortDateString() &&
                    cr.Height == Convert.ToDouble(numericCriminalHeight.Value) &&
                    cr.Weight == Convert.ToDouble(numericCriminalWeight.Value) &&
                    cr.EyeColor.ToLower() == textBCriminalEyeColor.Text.ToLower() &&
                    cr.SpecialSigns.ToLower() == textBCriminalSigns.Text.ToLower() &&
                    cr.Profession.ToLower() == textBCriminalProfession.Text.ToLower())
                {
                    MessageBox.Show("Такой преступник уже есть в базе.", "Преступник уже записан");
                    return;
                }
            }

            MainForm f = new MainForm();
            f = (MainForm)Owner;
            Criminal crim = new Criminal(textBCriminalSurname.Text, textBCriminalName.Text, textBCriminalPatronymic.Text,
                textBCriminalNickname.Text, textBCriminalPlaceOfBirth.Text, dateTCriminalDateOfBirth.Value.ToShortDateString(),
                Convert.ToDouble(numericCriminalHeight.Value), Convert.ToDouble(numericCriminalWeight.Value),
                textBCriminalEyeColor.Text, textBCriminalSigns.Text, textBCriminalProfession.Text);

            // Изменение преступника
            f.ChangeRow(crim);
            ActionsWithFields.wasChangedData = true;
            Close();
        }
    }
}
