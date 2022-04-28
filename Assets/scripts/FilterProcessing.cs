using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterProcessing
{
    public Texture2D imageProcessing(Texture2D inputTexture )
    {
        int width = inputTexture.width;
        int height = inputTexture.height;

        Color[] inputColors = inputTexture.GetPixels();
        Color[] outputColors = new Color[width * height];

        float[,,] inMat = new float[width, height, 3];
        float[,,] outMat = new float[width, height, 3];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var color = inputColors[(width * y) + x];
                inMat[x, y, 0] = color.r;
                inMat[x, y, 1] = color.g;
                inMat[x, y, 2] = color.b;
            }
        }

        int kernel_size = 20;
        boxfilter(inMat, outMat, kernel_size, width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var color = inputColors[(width * y) + x];
                outputColors[(width * y) + x] = new Color(outMat[x, y, 0], outMat[x, y, 1], outMat[x, y, 2]);
            }
        }

        //for (int y = 0; y < height; y++)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        var color = inputColors[(width * y) + x];
        //        outputColors[(width * y) + x] = new Color(color.g, color.b, color.r);
        //    }
        //}

        Texture2D outputTexture = new Texture2D(width, height);
        outputTexture.SetPixels(outputColors);
        outputTexture.Apply();

        return outputTexture;
    }

    /* r: kernel size */
    private bool boxfilter(float[,,] inMat, float[,,] outMat, int r, int width, int height)
    {

        float[,,] II = new float[width, height, 3];
        float[,,] O = new float[width, height, 3];

        for( int j = 1; j < height; j++)
        {
            float[] sum = new float[3];
            for( int i = 0; i< width; i++)
            {
                sum[0] += inMat[i, j, 0];
                sum[1] += inMat[i, j, 1];
                sum[2] += inMat[i, j, 2];

                II[i, j, 0] = II[i, j - 1, 0] + sum[0];
                II[i, j, 1] = II[i, j - 1, 1] + sum[1];
                II[i, j, 2] = II[i, j - 1, 2] + sum[2];
            }
        }


        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int offset_x = 0;
                int offset_y = 0;
                int xidx0 = 0;
                int xidx1 = 0;
                int yidx0 = 0;
                int yidx1 = 0;

                if (i + r > width - 1)
                {
                    xidx0 = width - 1;
                    offset_x = i + r - (width - 1);

                }
                else
                {
                    xidx0 = i + r;
                }

                if (i - r - 1 < 0)
                {
                    xidx1 = 0;
                    offset_x = -(i - r - 1);
                }
                else
                {
                    xidx1 = i - r - 1;
                }


                if (j + r > height - 1)
                {
                    yidx0 = height - 1;
                    offset_y = j + r - (height - 1);
                }
                else
                {
                    yidx0 = j + r;
                }

                if (j - r - 1 < 0)
                {
                    yidx1 = 0;
                    offset_y = -(j - r - 1);
                }
                else
                {
                    yidx1 = j - r - 1;
                }


                float norm_val = (2 * r + 1 - offset_x) * (2 * r + 1 - offset_y);
                float norm_val_inv = 1.0f / norm_val;

                for( int cidx = 0; cidx < 3; cidx++)
                {
                    O[i, j, cidx] = II[xidx0, yidx0, cidx] - II[xidx1, yidx0, cidx] - II[xidx0, yidx1, cidx] + II[xidx1, yidx1, cidx];
                    outMat[i, j, cidx] = norm_val_inv * O[i, j, cidx];
                }
            }
        }         


        return true;
    }
}
