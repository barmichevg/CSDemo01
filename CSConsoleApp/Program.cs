using System.Runtime.InteropServices;

namespace CSConsoleApp
{
    public static class Program
    {
        public static void Main()
        {
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            var filePath = System.IO.Directory.GetFiles(currentDirectory, "*.csv").First();

            IReadOnlyList<MovieCredit> movieCredits = null;
            try
            {
                var parser = new MovieCreditsParser(filePath);
                movieCredits = parser.Parse(); // Тип переменной теперь IReadOnlyList<MovieCredit>
            }
            catch (Exception exc)
            {
                Console.WriteLine("Не удалось распарсить csv");
                Environment.Exit(1);
            }
            var top10Actors = movieCredits
                                .SelectMany(movie => movie.Cast) // Объединяем всех актеров из всех фильмов в одну последовательность
                                .GroupBy(castMember => castMember.Name) // Группируем по имени актера
                                .Select(group => new
                                {
                                    ActorName = group.Key,
                                    MovieCount = group.Count() // Считаем количество фильмов для каждого
                                })
                                .OrderByDescending(actor => actor.MovieCount) // Сортируем по убыванию количества фильмов
                                .Take(10); // Берем первые 10

            Console.WriteLine(string.Join(Environment.NewLine, top10Actors.Select(a => $"{a.ActorName} - {a.MovieCount}")));
            

        }
    }
}