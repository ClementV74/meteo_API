using System;
using System.Collections.Generic;
using System.IO;

namespace meteov3.Service
{
    public class Ville
    {
        public List<string> LsVille;

        public Ville()
        {
            LsVille = new List<string>();
            LoadCitiesFromFile("cities.txt");
        }

        public void AddVille(string villeName)
        {
            if (!LsVille.Contains(villeName))
            {
                LsVille.Add(villeName);
                SaveCitiesToFile("cities.txt");
            }
        }

        public void RemoveVille(string villeName)
        {
            if (LsVille.Contains(villeName))
            {
                LsVille.Remove(villeName);
                SaveCitiesToFile("cities.txt");
            }
        }

        private void LoadCitiesFromFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    LsVille = new List<string>(File.ReadAllLines(fileName));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading cities from file: {ex.Message}");
            }
        }

        private void SaveCitiesToFile(string fileName)
        {
            try
            {
                File.WriteAllLines(fileName, LsVille);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving cities to file: {ex.Message}");
            }
        }
    }
}
