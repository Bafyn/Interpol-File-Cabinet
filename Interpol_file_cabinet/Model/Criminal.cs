using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpol_file_cabinet.Model
{
    public class Criminal
    {
        // Конструктор с параметрами.
        public Criminal(string surname, string name, string patronymic, string nickname, string placeOfBirth,
            string dateOfBirth, double height, double weight, string eyeColor, string specialSigns, string profession)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Nickname = nickname;
            PlaceOfBirth = placeOfBirth;
            DateOfBirth = dateOfBirth;
            Height = height;
            Weight = weight;
            EyeColor = eyeColor;
            SpecialSigns = specialSigns;
            Profession = profession;
            Group = "";
            DateOfDeath = "";
        }

        // Конструктор по умолчанию.
        public Criminal() { }

        // Свойства.
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Nickname { get; set; }
        public string PlaceOfBirth { get; set; }
        public string DateOfBirth { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string EyeColor { get; set; }
        public string SpecialSigns { get; set; }
        public string Profession { get; set; }
        public string Group { get; set; }
        public string DateOfDeath { get; set; }

        // Переопределение оператора "=="
        public static bool operator ==(Criminal cr1, Criminal cr2)
        {
            if (cr1.Surname == cr2.Surname && cr1.Name == cr2.Name && cr1.Patronymic == cr2.Patronymic &&
                cr1.Nickname == cr2.Nickname && cr1.PlaceOfBirth == cr2.PlaceOfBirth && cr1.DateOfBirth == cr2.DateOfBirth &&
                cr1.Height == cr2.Height && cr1.Weight == cr2.Weight && cr1.SpecialSigns == cr2.SpecialSigns &&
                cr1.EyeColor == cr2.EyeColor && cr1.Profession == cr2.Profession && cr1.Group == cr2.Group)
                return true;
            return false;
        }

        public static bool operator !=(Criminal cr1, Criminal cr2)
        {
            if (cr1.Surname != cr2.Surname || cr1.Name != cr2.Name || cr1.Patronymic != cr2.Patronymic ||
                cr1.Nickname != cr2.Nickname || cr1.PlaceOfBirth != cr2.PlaceOfBirth || cr1.DateOfBirth != cr2.DateOfBirth ||
                cr1.Height != cr2.Height || cr1.Weight != cr2.Weight || cr1.SpecialSigns != cr2.SpecialSigns ||
                cr1.EyeColor != cr2.EyeColor || cr1.Profession != cr2.Profession || cr1.Group != cr2.Group)
                return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;

            Criminal crim = (Criminal)obj;
            return
                (Surname == crim.Surname && Name == crim.Name && Patronymic == crim.Patronymic && Nickname == crim.Nickname &&
                PlaceOfBirth == crim.PlaceOfBirth && DateOfBirth == crim.DateOfBirth && Height == crim.Height &&
                Weight == crim.Weight && EyeColor == crim.EyeColor && SpecialSigns == crim.SpecialSigns &&
                Profession == crim.Profession && Group == crim.Group);
        }

        public override int GetHashCode()
        {
            int hashC = Surname.GetHashCode();
            hashC = hashC * 42 + Name.GetHashCode();
            hashC = hashC * 42 + Patronymic.GetHashCode();
            hashC = hashC * 42 + Nickname.GetHashCode();
            hashC = hashC * 42 + PlaceOfBirth.GetHashCode();
            hashC = hashC * 42 + DateOfBirth.GetHashCode();
            hashC = hashC * 42 + Height.GetHashCode();
            hashC = hashC * 42 + Weight.GetHashCode();
            hashC = hashC * 42 + EyeColor.GetHashCode();
            hashC = hashC * 42 + SpecialSigns.GetHashCode();
            hashC = hashC * 42 + Profession.GetHashCode();
            hashC = hashC * 42 + Group.GetHashCode();

            return hashC;
        }
    }
}
