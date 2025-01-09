using QuizLab3.Model;
using QuizLab3.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace QuizLab3.Json
{
    public class Json
    {
        private readonly string appDataPath;
        private readonly string filePath;
        private readonly JsonSerializerOptions options;

        public Json()
        {
            
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string appFolder = Path.Combine(appDataPath, "Quizlab3");
            Directory.CreateDirectory(appFolder);  

            filePath = Path.Combine(appFolder, "Quizlab3.json");

            options = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNameCaseInsensitive = true
            };
        }

        internal async Task SaveJson(List<QuestionPack> packs)
        {
            string json = JsonSerializer.Serialize(packs, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(filePath, json);
        }

        internal async Task<List<QuestionPack>> LoadJson()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = await File.ReadAllTextAsync(filePath);
                    return JsonSerializer.Deserialize<List<QuestionPack>>(json, options);
                }
                else
                {
                    Debug.WriteLine("File not found at path: " + filePath);
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine($"Error loading JSON: {ex.Message}");
            }
         
            return new List<QuestionPack>();
        }
    }

}



