using System;
using System.Collections.Generic;

namespace Mammatus.Helpers
{
    using System.Linq;

    public sealed class NumberGenerator
    {
        private int[] RandomSequence { get; set; }

        private int RandomSequenceIndex { get; set; }

        private int Number { get; set; }

        private List<int> History { get; set; }

        private bool _fieldIsRandom;
        public bool IsRandom
        {
            get { return this._fieldIsRandom; }
            set
            {
                if (this._fieldIsRandom != value)
                {
                    this._fieldIsRandom = value;
                    if (this._fieldIsRandom)
                    {
                        this.RefreshRandomSequence();
                    }
                }
            }
        }

        private static readonly Random Random = new Random();

        private int _fieldLength;
        public int Length
        {
            get { return this._fieldLength; }
            set
            {
                if (this._fieldLength != value)
                {
                    this._fieldLength = value;
                }
            }
        }

        public NumberGenerator(int length, int initialNumber)
        {
            this.Number = initialNumber;
            this.Length = length;
        }

        public NumberGenerator(int length) : this(length, 0)
        {
        }

        public int GetNumber(int index)
        {
            if (index < 0)
            {
                return 0;
            }

            if (this.History == null)
            {
                this.History = Enumerable.Range(this.Number, index).ToList();
            }

            while (index >= this.History.Count)
            {
                var number = this.GenerateNextNumber();
                this.History.Add(number);
            }
            return this.History[index];
        }

        private int GenerateNextNumber()
        {
            int number;
            if (this.IsRandom)
            {
                this.RandomSequenceIndex++;
                if (this.RandomSequenceIndex >= this.RandomSequence.Length)
                {
                    this.RefreshRandomSequence();
                    if (this.Length > 2 && this.Number == this.RandomSequence[0])
                    {
                        this.RandomSequenceIndex++;
                    }
                }
                number = this.RandomSequence[this.RandomSequenceIndex];
            }
            else
            {
                number = this.History.Last() + 1;
                if (number >= this.Length)
                {
                    number = 0;
                }
            }
            this.Number = number;
            return number;
        }

        public void RefreshRandomSequence()
        {
            this.RandomSequenceIndex = 0;

            switch (this.Length)
            {
                case 0:
                case 1:
                    this.RandomSequence = new int[] { 0 };
                    break;

                case 2:
                    this.RandomSequence = new int[] { 0, 1 };
                    break;

                default:
                    this.RandomSequence = Enumerable
                        .Range(0, this.Length)
                        .OrderBy(x => Random.Next(this.Length))
                        .ToArray();
                    break;
            }
        }
    }
}
