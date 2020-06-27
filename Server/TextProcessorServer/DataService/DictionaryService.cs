using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TextProcessorServer.Models;

namespace TextProcessorServer.DataService
{
    sealed class DictionaryService
    {
        private readonly Context Context;

        public DictionaryService(Context context)
        {
            this.Context = context;
        }

        public void Create(string filePath)
        {
            CheckFileExist(filePath);
            Delete();
            AddOrUpdateWords(filePath);
        }

        public IEnumerable<string> Read(string prefix)
        {
            return Context.Words
                .AsNoTracking()
                .Where(w => w.Value.StartsWith(prefix))
                .OrderByDescending(w => w.Frequency)
                .ThenBy(w => w.Value)
                .Select(w => w.Value)
                .Take(5)
                .AsEnumerable();
        }

        public void Update(string filePath)
        {
            CheckFileExist(filePath);
            AddOrUpdateWords(filePath);
        }


        public void Delete()
        {
            Context.Database.ExecuteSqlCommand($"delete from Words");
        }

        private static void CheckFileExist(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);
        }

        private void AddOrUpdateWords(string filePath)
        {
            var newWordsList = GetWordsList(filePath);
            foreach (var newWord in newWordsList)
            {
                var word = Context.Words.FirstOrDefault(w => w.Value == newWord.Value);

                if (word != null)
                    word.Frequency += newWord.Frequency;

                else if(newWord.Frequency >= 3)
                    Context.Words.Add(newWord);
            }
            
            Context.SaveChanges();
        }

        private IEnumerable<Word> GetWordsList(string filePath)
        {
            var wordsList = new List<Word>();
            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var words = line.Split(' ')
                        .Where(w => w.Length <= Word.MaxLength && w.Length >= Word.MinLength);

                    foreach (var wordValue in words)
                    {
                        var word = wordsList.Find(w => w.Value == wordValue);
                        if (word == null)
                        {
                            word = new Word(wordValue, 0);
                            wordsList.Add(word);
                        }

                        word.Frequency++;
                    }
                }
            }

            return wordsList;
        }
    }
}
