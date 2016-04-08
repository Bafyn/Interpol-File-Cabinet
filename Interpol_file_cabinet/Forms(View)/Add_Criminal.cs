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
    public partial class Add_Criminal : Form
    {
        public Add_Criminal()
        {
            InitializeComponent();
            this.AcceptButton = btnAddCriminal;
            this.dateTCriminalDateOfBirth.MaxDate = DateTime.Now;
        }

        MainForm mf = new MainForm();
        Criminal crim;

        private void buttonAddCriminal_Click(object sender, EventArgs e)
        {
            int flagErrorInFields = 0; // Флаг, указывающий на наличие ошибок в полях для ввода

            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                if (tb.Text.Trim() == string.Empty || ActionsWithFields.CheckWithRegex(tb.Text))
                {
                    if (tb.Name == "textBCriminalSigns" || tb.Name == "textBCriminalProfession")
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

            if (textBCriminalProfession.Text.Trim() == string.Empty || ActionsWithFields.CheckWithRegexWithSpaces(textBCriminalProfession.Text))
            {
                textBCriminalProfession.BackColor = Color.LightPink;
                flagErrorInFields = 1;
            }
            else
            {
                textBCriminalProfession.BackColor = Color.White;
            }

            if (flagErrorInFields == 1)
            {
                MessageBox.Show("Проверьте правильность заполения всех полей", "Ошибка");
                return;
            }

            // Проверка на наличие преступника в базе
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

            // Создание нового преступника
            crim = new Criminal(textBCriminalSurname.Text, textBCriminalName.Text, textBCriminalPatronymic.Text,
                textBCriminalNickname.Text, textBCriminalPlaceOfBirth.Text, dateTCriminalDateOfBirth.Value.ToShortDateString(),
                (double)numericCriminalHeight.Value, (double)numericCriminalWeight.Value, textBCriminalEyeColor.Text,
                textBCriminalSigns.Text, textBCriminalProfession.Text);
            mf = (MainForm)Owner;
            mf.AddCriminal(crim, 0, true, true);
            ActionsWithFields.wasChangedData = true;

            Close();
        }
    }
}
