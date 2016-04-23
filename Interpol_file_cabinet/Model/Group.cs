using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpol_file_cabinet.Model
{
    public class Group
    {
        // Поля
        public int CountOfCriminals;
        public string Name;

        // Конструктор с параметрами
        public Group()
        {
            this.Name = "";
            CountOfCriminals = 0;
        }

        // Конструктор копирования
        public Group(Group gr)
        {
            CountOfCriminals = gr.CountOfCriminals;
            Name = gr.Name;
        }
    }
}
