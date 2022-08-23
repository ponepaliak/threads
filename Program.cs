using System.Collections.Generic;

    //Разработайте многопоточное приложение, выполняющее вычисление произведения матриц A (m×n) и B(n×k). Элементы cij матрицы 
    //произведения С = A×B вычисляются параллельно p однотипными потоками. Если некоторый поток уже вычисляет элемент cij 
    //матрицы C, то следующий приступающий к вычислению поток выбирает для расчета элемент cij+1, если j<k, и ci+1k, если 
    //j=k. Выполнив вычисление элемента матрицы-произведения, поток проверяет, нет ли элемента, который еще не рассчитывается. 
    //Если такой элемент есть, то приступает к его расчету. В противном случае отправляет (пользовательское) сообщение о 
    //завершении своей работы и приостанавливает своё выполнение. Главный поток, получив сообщения о завершении вычислений 
    //от всех потоков, выводит результат на экран и запускает поток, записывающий результат в конец файла-протокола. 
    //В каждом потоке должна быть задержка в выполнении вычислений (чтобы дать возможность поработать всем потокам). 
    //Синхронизацию потоков между собой организуйте через критическую секцию или мьютекс.

namespace Matrix
{
    class Program
    {
        private static List<List<double>> FirstMatrix = new List<List<double>>
        {
            new List<double> { 1, 2, 3, 4 },
            new List<double> { 4, 3, 2, 1 },
            new List<double> { 1, 2, 3, 4 },
            new List<double> { 4, 3, 2, 1 },
        };
        private static List<List<double>> SecondMatrix = new List<List<double>>
        {
            new List<double> { 1, 2, 3 },
            new List<double> { 3, 2, 1 },
            new List<double> { 1, 2, 3 },
            new List<double> { 3, 2, 1 },
        };

        static void Main(string[] args)
        {
            var matrixCalculator = new MatrixCalculator();
            matrixCalculator.Initialize(FirstMatrix, SecondMatrix, 3);
            matrixCalculator.Multiply();
            matrixCalculator.ShowMultiplicationResult();
        }    
    }
}
