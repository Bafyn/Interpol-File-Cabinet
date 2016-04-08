using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpol_file_cabinet.Model
{
    public class Group
    {
        // Свойства
        public string Name { get; set; }
        public List<Criminal> criminalsInGroup = new List<Criminal>();

        // Конструктор с параметрами
        public Group(string name, List<Criminal> crims)
        {
            Name = name;
            criminalsInGroup = crims;
        }

        // Конструктор копирования
        public Group(Group gr)
        {
            this.Name = gr.Name;
            this.criminalsInGroup = new List<Criminal>(gr.criminalsInGroup);
        }

        // Конструктор по умолчанию
        public Group() { }
    }
}
