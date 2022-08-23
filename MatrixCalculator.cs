using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Matrix
{
    class MatrixCalculator
    {
        private Mutex MutexObj = new();
        private List<List<double>> FirstMatrix;
        private List<List<double>> SecondMatrix;
        private List<List<double>> MultiplicationResult;
        private List<List<bool?>> MultiplicationStatus;
        private List<Thread> Threads;
        private int ThreadsCount;

        private int RowsNumber => FirstMatrix.Count;
        private int ColumnsNumber => SecondMatrix[0].Count;

        public void Initialize(List<List<double>> firstMatrix, List<List<double>> secondMatrix, int threadsCount)
        {
            if (!IsCorrectMatrix(firstMatrix) || !IsCorrectMatrix(secondMatrix) || firstMatrix[0].Count != secondMatrix.Count)
            {
                throw new Exception("Row number of first matrix and line number of second matrix must be equal");
            }

            if (threadsCount <= 0)
            {
                throw new Exception("Threads count must be positive number");
            }

            FirstMatrix = firstMatrix;
            SecondMatrix = secondMatrix;
            ThreadsCount = threadsCount;
        }

        public void Multiply()
        {
            SetDefaultValueForResult();
            StartThreads();
        }

        public void ShowMultiplicationResult()
        {
            foreach (var line in MultiplicationResult)
            {
                Console.WriteLine(string.Join(" ", line));
            }
        }

        private bool IsCorrectMatrix(List<List<double>> matrix)
        {
            if (matrix == null || matrix.Count == 0 || matrix[0] == null || matrix[0].Count == 0)
            {
                return false;
            }

            var firstRowElementCount = matrix[0].Count;

            foreach (var row in matrix)
            {
                if (row.Count != firstRowElementCount)
                {
                    return false;
                }
            }

            return true;
        }

        private void SetDefaultValueForResult()
        {
            MultiplicationResult = new List<List<double>>();
            MultiplicationStatus = new List<List<bool?>>();
            for (int i = 0; i < RowsNumber; i++)
            {
                MultiplicationResult.Add(Enumerable.Repeat(0d, ColumnsNumber).ToList());
                MultiplicationStatus.Add(Enumerable.Repeat((bool?)true, ColumnsNumber).ToList());
            }
        }

        private void StartThreads()
        {
            Threads = new List<Thread>(ThreadsCount);

            for (int i = 0; i < ThreadsCount; i++)
            {
                var thread = new Thread(DoMultiply);
                Threads.Add(thread);
                thread.Start();
            }

            foreach (var thread in Threads)
            {
                thread.Join();
            }
        }

        private void DoMultiply()
        {
            for (int i = 0; i < RowsNumber; i++)
            {
                for (int j = 0; j < ColumnsNumber; j++)
                {
                    MutexObj.WaitOne();
                    if (IsElementReadyForCalculation(i, j))
                    {
                        MultiplyElement(i, j);
                    }
                    MutexObj.ReleaseMutex();
                    Thread.Sleep(200);
                }
            }
        }

        private bool IsElementReadyForCalculation(int line, int column)
        {
            return MultiplicationStatus[line][column] == true;
        }

        private void MultiplyElement(int line, int column)
        {
            SetElementAsInCalculating(line, column);
            double result = 0;
            for (int i = 0; i < RowsNumber; i++)
            {
                result += FirstMatrix[line][i] * SecondMatrix[i][column];
            }
            MultiplicationResult[line][column] = result;
            SetElementAsCalculated(line, column);
        }

        private void SetElementAsInCalculating(int line, int column)
        {
            MultiplicationStatus[line][column] = null;
        }

        private void SetElementAsCalculated(int line, int column)
        {
            MultiplicationStatus[line][column] = false;
        }
    }
}
