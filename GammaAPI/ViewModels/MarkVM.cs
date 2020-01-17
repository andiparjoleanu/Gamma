using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GammaAPI.ViewModels
{
    public class MarkVM
    {
        [DisplayName("Data")]
        public DateTime Date { get; set; }

        [DisplayName("Nota")]
        public int Value { get; set; }

        [DisplayName("Detalii")]
        public string Note { get; set; }

        public string StudentId { get; set; }
        public string CourseId { get; set; }
    }
}
