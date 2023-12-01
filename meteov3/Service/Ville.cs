using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meteov3.Service
{
    public class Ville
    {
        public List<String> LsVille;

        public Ville()
        {

            LsVille = new List<String>();
   
            LsVille.Add("Paris");   
            LsVille.Add("Marseille");
            LsVille.Add("Lyon");
            LsVille.Add("Toulouse");
            LsVille.Add("Nice");
            LsVille.Add("Nantes");
            LsVille.Add("Strasbourg");
            LsVille.Add("Montpellier");
          
        }

        //ajoute le code por ajouter une ville avec un btn pour ajouter une ville dnas la liste
          public void AddVille(string villeName)
        {
            if (!LsVille.Contains(villeName))
            {
                LsVille.Add(villeName);
            }
        }

        public void RemoveVille(string villeName)
        {
            if (LsVille.Contains(villeName))
            {
                LsVille.Remove(villeName);
            }
        }
       
    }
}
