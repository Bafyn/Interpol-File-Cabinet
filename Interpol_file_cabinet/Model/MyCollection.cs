using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpol_file_cabinet.Model
{
    class MyCollection
    {
        public static List<Criminal> criminals;
        public static List<Criminal> criminalsArchive;
        public static List<Criminal> criminalsDead;
        public static List<Group> groups;
        public static List<string> professions;

        // Статический конструктор
        static MyCollection()
        {
            criminals = new List<Criminal>();
            criminalsArchive = new List<Criminal>();
            criminalsDead = new List<Criminal>();
            groups = new List<Group>();
            professions = new List<string>();
        }
    }
}
