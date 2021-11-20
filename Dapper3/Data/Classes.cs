using System.Collections;
using System.Collections.Generic;

namespace APIRegioes.Data
{
    public class Regiao
    {
        public int IdRegiao { get; set; }
        public string CodRegiao { get; set; }
        public string NomeRegiao { get; set; }
        public List<Estado> Estados { get; set; }
    }

    public class Estado
    {
        public string SiglaEstado { get; set; }
        public string NomeEstado { get; set; }
        public string NomeCapital { get; set; }
    }

    public class RegiaoEstado //: IEnumerable
    {
        public int IdRegiao { get; set; }
        public string CodRegiao { get; set; }
        public string NomeRegiao { get; set; }
        public string SiglaEstado { get; set; }
        public string NomeEstado { get; set; }
        public string NomeCapital { get; set; }

        //public IEnumerator GetEnumerator()
        //{
        //    return (IEnumerator)GetEnumerator();
        //}
        //public RegiaoEstado GetEnumerator()
        //{
        //    return new RegiaoEstado(_people);
        //}
    
    }
}