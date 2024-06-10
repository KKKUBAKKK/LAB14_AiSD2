using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labratoria_ASD2_2024
{
    public class Lab14 : MarshalByRefObject
    {
        /// <summary>
        /// Znajduje wszystkie maksymalne palindromy długości przynajmniej 2 w zadanym słowie. Wykorzystuje Algorytm Manachera.
        /// 
        /// Palindromy powinny być zwracane jako lista par (indeks pierwszego znaku, długość palindromu), 
        /// tzn. para (i, d) oznacza, że pod indeksem i znajduje się pierwszy znak d-znakowego palindromu.
        /// 
        /// Kolejność wyników nie ma znaczenia.
        /// 
        /// Można założyć, że w tekście wejściowym nie występują znaki '#' i '$' - można je wykorzystać w roli wartowników
        /// </summary>
        /// <param name="text">Tekst wejściowy</param>
        /// <returns>Tablica znalezionych palindromów</returns>
        public (int startIndex, int length)[] FindPalindromes(string text) // moze zepsulaby sie zlozonosc ale najlatwiej by bylo wstawic znak specjalny miedzy kazda litere i traktowac kazde string jako nieparzysty
        {
            var R = Manacher(text);
            List<(int startIndex, int length)> res = new List<(int startIndex, int length)>();

            (int startIndex, int length) temp = (0, 0);
            for (int i = 1; i < text.Length; i++)
            {
                var t = (i - Math.Max(R[0, i], R[1, i]), Math.Max(2 * R[0, i], 2 * R[1, i] + 1));

                if ((t.Item1 <= temp.startIndex && t.Item1 + t.Item2 >= temp.startIndex) ||
                    (temp.startIndex == 0 && temp.length == 0))
                {
                    temp = t;
                }
                else
                {
                    res.Add(temp);
                    temp = t;
                }
            }
            
            return res.ToArray();
        }
        
        public int[,] Manacher(string text)
        {
            var R = new int[2, text.Length];

            for (int i = 1; i < text.Length;)
            {
                if (R[0, i] != 0 || R[1, i] != 0)
                {
                    i += Math.Max(R[0, i], R[1, i]);
                    continue;
                }
                R[0, i] = FindMaxREven(text, i);
                R[1, i] = FindMaxROdd(text, i);

                MarkPalindromesInsideEven(text, i, R);
                MarkPalindromesInsideOdd(text, i, R);

                i += Math.Max(R[0, i], R[1, i]) + 1;
            }
            
            return R;
        }

        public int FindMaxREven(string text, int i)
        {
            int rp = 0;
            while (i - 1 - rp >= 0 && i + rp < text.Length && text[i - 1 - rp] == text[i + rp])
                rp++;

            return rp;
        }

        public int FindMaxROdd(string text, int i)
        {
            int rp = 0;
            while (i + rp + 1 < text.Length && i - rp - 1 >= 0 && text[i - 1 - rp] == text[i + rp + 1])
                rp++;

            return rp;
        }

        public void MarkPalindromesInsideEven(string text, int i, int[,] R)
        {
            int rp = R[0, i];

            if (rp == 0)
                return;
            
            for (int k = 1; k <= rp; k++)
            {
                CopyPalindromeEven(text, i, rp, k, R[0, i - k], R);
                CopyPalindromeOdd(text, i , rp, k, R[1, i - k], R);
            }
        }
        
        public void MarkPalindromesInsideOdd(string text, int i, int[,] R)
        {
            int rp = R[1, i];

            if (rp == 0)
                return;
            
            for (int k = 1; k <= rp; k++)
            {
                CopyPalindromeEven(text, i, rp, k, R[0, i - k], R);
                CopyPalindromeOdd(text, i , rp, k, R[1, i - k], R);
            }
        }

        public void CopyPalindromeEven(string text, int i, int rp, int k, int r, int[,] R)
        {
            if (rp == 0)
                return;

            if (r < rp - k)
            {
                R[0, i + k - 1] = r;
                return;
            }

            if (r == rp - k)
            {
                R[0, i + k - 1] = FindMaxREven(text, i + k - 1);
                MarkPalindromesInsideEven(text, i + k - 1, R);
            }

            R[0, i + k - 1] = rp - k;
        }

        public void CopyPalindromeOdd(string text, int i, int rp, int k, int r, int[,] R)
        {
            if (rp == 0)
                return;

            if (r < rp - k)
            {
                R[1, i + k] = r;
                return;
            }

            if (r == rp - k)
            {
                R[1, i + k] = FindMaxROdd(text, i + k);
                MarkPalindromesInsideOdd(text, i + k, R);
            }

            R[1, i + k] = rp - k;
        }
    }

}
