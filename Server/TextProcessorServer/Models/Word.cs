using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TextProcessorServer.Models
{
    public sealed class Word
    {
        public const int MaxLength = 15;
        public const int MinLength = 3;


        public Word(string value, int frequency)
        {
            Value = value;
            Frequency = frequency;
        }

        public int Id { get; set; }

        /// <summary>
        /// Текстовое представление слова
        /// </summary>
        [MaxLength(MaxLength)]
        [MinLength(MinLength)]
        public string Value { get; set; }

        /// <summary>
        /// Частота, с которой встречается слово
        /// </summary>
        public int Frequency { get; set; }
    }
}
