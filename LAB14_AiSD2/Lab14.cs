using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
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
        public (int startIndex, int length)[] FindPalindromes(string text)
        {
            var R = Manacher(text);

            List<(int startIndex, int length)> maxPalindromes = new List<(int startIndex, int length)>();
            for (int i = 1; i <= text.Length; i++)
            {
                (int startIndex, int length) temp = (i - R[0, i] - 1, 2 * R[0, i]);
                if (temp.length > 1)
                    maxPalindromes.Add(temp);
                
                temp = (i - R[1, i] - 1, 2 * R[1, i] + 1);
                if (temp.length > 1)
                    maxPalindromes.Add(temp);
            }

            return maxPalindromes.ToArray();
        }

        public int[,] Manacher(string text)
        {
            int n = text.Length;
            text = "#" + text + "$";
            int[,] R = new int[2, n + 2];

            for (int j = 0; j <= 1; j++)
            {
                int rp = 0;

                for (int i = 1, k = 1; i <= n; i += k, k = 1)
                {
                    while (text[i - rp - 1] == text[i + rp + j])
                        rp++;
                    
                    R[j, i] = rp;
                    
                    while (R[j, i - k] != rp - k && k < rp)
                    {
                        R[j, i + k] = Math.Min(R[j, i - k], rp - k);
                        k++;
                    }

                    rp = Math.Max(rp - k, 0);
                }
            }

            text = text.Substring(1, n);

            return R;
        }
    }
}
