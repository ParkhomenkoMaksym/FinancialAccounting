using FinancialAccounting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinancialAccounting.Services
{
    public static class FileServices
    {
        private static readonly string filePath = 
            Path.Combine(FileSystem.AppDataDirectory, "finance.json");

        public static async Task SaveDataAsync(FinanceData data)
        {
            //data.SavedIndex = savedIndex;

            //string filePath = Path.Combine(FileSystem.AppDataDirectory, "finance.json");

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true,
            });

            await File.WriteAllTextAsync(filePath, json);
        }

        public static async Task<FinanceData> LoadDataAsync()
        {
            //string filePath = Path.Combine(FileSystem.AppDataDirectory, "finance.json");

            if (!File.Exists(filePath))
                return new FinanceData();

            var json = await File.ReadAllTextAsync(filePath);

            return JsonSerializer.Deserialize<FinanceData>(json) ?? new FinanceData();
        }
    }
}
