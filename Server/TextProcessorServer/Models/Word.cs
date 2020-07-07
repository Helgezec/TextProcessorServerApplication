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
        /// Text representation of word
        /// </summary>
        [MaxLength(MaxLength)]
        [MinLength(MinLength)]
        public string Value { get; set; }

        /// <summary>
        /// Frequency with which the word occurs
        /// </summary>
        public int Frequency { get; set; }
    }
}
