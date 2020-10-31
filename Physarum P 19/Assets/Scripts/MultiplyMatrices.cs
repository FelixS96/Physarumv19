using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class MultiplyMatrices : MonoBehaviour
{
    [SerializeField]
    bool test;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            GaussianBlur(3,3);
            test = !test;
        }
    }
    public double[,] GaussianBlur(int lenght, double weight)
    {
        double[,] kernel = new double[lenght, lenght];
        double kernelSum = 0;
        int foff = (lenght - 1) / 2;
        double distance = 0;
        double constant = 1d / (2 * Math.PI * weight * weight);
        for (int y = -foff; y <= foff; y++)
        {
            for (int x = -foff; x <= foff; x++)
            {
                distance = ((y * y) + (x * x)) / (2 * weight * weight);
                kernel[y + foff, x + foff] = constant * Math.Exp(-distance);
                kernelSum += kernel[y + foff, x + foff];
            }
        }
        for (int y = 0; y < lenght; y++)
        {
            for (int x = 0; x < lenght; x++)
            {
                kernel[y, x] = kernel[y, x] * 1d / kernelSum;
            }
        }
        return kernel;
    }

    //#region Sequential_Loop
    //static void MultiplyMatricesSequential(double[,] matA, double[,] matB,
    //                                        double[,] result)
    //{
    //    int matACols = matA.GetLength(1);
    //    int matBCols = matB.GetLength(1);
    //    int matARows = matA.GetLength(0);

    //    for (int i = 0; i < matARows; i++)
    //    {
    //        for (int j = 0; j < matBCols; j++)
    //        {
    //            double temp = 0;
    //            for (int k = 0; k < matACols; k++)
    //            {
    //                temp += matA[i, k] * matB[k, j];
    //            }
    //            result[i, j] += temp;
    //        }
    //    }
    //}
    //#endregion

    //#region Parallel_Loop
    //static void MultiplyMatricesParallel(double[,] matA, double[,] matB, double[,] result)
    //{
    //    int matACols = matA.GetLength(1);
    //    int matBCols = matB.GetLength(1);
    //    int matARows = matA.GetLength(0);

    //    // A basic matrix multiplication.
    //    // Parallelize the outer loop to partition the source array by rows.
    //    Parallel.For(0, matARows, i =>
    //    {
    //        for (int j = 0; j < matBCols; j++)
    //        {
    //            double temp = 0;
    //            for (int k = 0; k < matACols; k++)
    //            {
    //                temp += matA[i, k] * matB[k, j];
    //            }
    //            result[i, j] = temp;
    //        }
    //    }); // Parallel.For
    //}
    //#endregion

    #region Main
    void MainFunc()
    {


        

        //// Set up matrices. Use small values to better view
        //// result matrix. Increase the counts to see greater
        //// speedup in the parallel loop vs. the sequential loop.
        //int colCount = 180;
        //int rowCount = 2000;
        //int colCount2 = 270;
        //double[,] m1 = InitializeMatrix(rowCount, colCount);
        //double[,] m2 = InitializeMatrix(colCount, colCount2);
        //double[,] result = new double[rowCount, colCount2];

        //// First do the sequential version.
        //UnityEngine.Debug.Log("Executing sequential loop...");
        //Stopwatch stopwatch = new Stopwatch();
        //stopwatch.Start();

        //MultiplyMatricesSequential(m1, m2, result);
        //stopwatch.Stop();
        //UnityEngine.Debug.Log("Sequential loop time in milliseconds: "+ stopwatch.ElapsedMilliseconds);

        //// For the skeptics.

        //// Reset timer and results matrix.
        //stopwatch.Reset();
        //result = new double[rowCount, colCount2];

        //// Do the parallel loop.
        //UnityEngine.Debug.Log("Executing parallel loop...");
        //stopwatch.Start();
        //MultiplyMatricesParallel(m1, m2, result);
        //stopwatch.Stop();
        //UnityEngine.Debug.Log("Parallel loop time in milliseconds: "+
        //                        stopwatch.ElapsedMilliseconds);

        ////// Keep the console window open in debug mode.
        ////Console.Error.WriteLine("Press any key to exit.");
        ////Console.ReadKey();
    }
    #endregion

    //#region Helper_Methods
    //static double[,] InitializeMatrix(int rows, int cols)
    //{
    //    double[,] matrix = new double[rows, cols];

    //    System.Random r = new System.Random();
    //    for (int i = 0; i < rows; i++)
    //    {
    //        for (int j = 0; j < cols; j++)
    //        {
    //            matrix[i, j] = r.Next(100);
    //        }
    //    }
    //    return matrix;
    //}
    //#endregion
}
