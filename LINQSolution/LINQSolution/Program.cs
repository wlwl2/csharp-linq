using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqFaroShuffle
{
    class Program
    {
        public Program()
        {
        }

        public static void Main(string[] args)
        {
            var startingDeck = (from s in Suits().LogQuery("Suit Generation")
                                from r in Ranks().LogQuery("Value Generation")
                                select new PlayingCard(s, r))
                    .LogQuery("Starting Deck")
                    .ToArray();

            foreach (var c in startingDeck)
            {
                Console.WriteLine(c);
            }

            Console.WriteLine();

            var times = 0;
            var shuffle = startingDeck;

            do
            {
                /*
                shuffle = shuffle.Take(26)
                    .LogQuery("Top Half")
                    .InterleaveSequenceWith(shuffle.Skip(26).LogQuery("Bottom Half"))
                    .LogQuery("Shuffle")
                    .ToArray();
                */

                shuffle = shuffle.Skip(26)
                    .LogQuery("Bottom Half")
                    .InterleaveSequenceWith(shuffle.Take(26).LogQuery("Top Half"))
                    .LogQuery("Shuffle")
                    .ToArray();

                foreach (var c in shuffle)
                {
                    Console.WriteLine(c);
                }

                times++;
                Console.WriteLine(times);
            } while (!startingDeck.SequenceEquals(shuffle));

            Console.WriteLine(times);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public enum Suit
        {
            Clubs,
            Diamonds,
            Hearts,
            Spades
        }

        static IEnumerable<Suit> Suits() => Enum.GetValues(typeof(Suit)) as IEnumerable<Suit>;

        public enum Rank
        {
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
            Ace
        }

        static IEnumerable<Rank> Ranks() => Enum.GetValues(typeof(Rank)) as IEnumerable<Rank>;

        public class PlayingCard
        {
            public Suit CardSuit { get; }
            public Rank CardRank { get; }

            public PlayingCard(Suit s, Rank r)
            {
                CardSuit = s;
                CardRank = r;
            }

            public override string ToString()
            {
                return $"{CardRank} of {CardSuit}";
            }
        }
    }
}
