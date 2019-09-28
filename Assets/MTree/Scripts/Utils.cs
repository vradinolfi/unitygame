using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mtree
{
    public class Utils
    {
        public static Shader GetBarkShader()
        {
            return Shader.Find("Mtree/Bark");
        }

        public static Shader GetLeafShader()
        {
            return Shader.Find("Mtree/Leafs");
        }

        public static Shader GetBillboardShader()
        {
            return Shader.Find("Mtree/Billboard");
        }

        public static T[] SampleArray<T>(T[] array, int number)
        {
            number = Mathf.Max(0, Mathf.Min(array.Length, number));
            T[] result = new T[number];
            for (int i = 0; i < number; i++)
            {
                int index = Random.Range(i, array.Length - 1);
                result[i] = array[index];
                array[index] = array[i];
            }

            return result;
        }

        public static void DilateTexture(Texture2D texture, int iterations)
        {
            Color[] cols = texture.GetPixels();
            Color[] copyCols = texture.GetPixels();
            HashSet<int> borderIndices = new HashSet<int>();
            HashSet<int> indexBuffer = new HashSet<int>();
            int w = texture.width;
            int h = texture.height;
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].a < 0.5f)
                {
                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            int index = i + y * w + x;
                            if (index >= 0 && index < cols.Length && cols[index].a > 0.5f) // if a non transparent pixel is near the transparent one, add the transparent pixel index to border indices
                            {
                                borderIndices.Add(i);
                                goto End;
                            }
                        }
                    }
                    End:;
                }
            }

            for (int iteration = 0; iteration < iterations; iteration++)
            {
                foreach (int i in borderIndices)
                {
                    Color meanCol = Color.black;
                    int opaqueNeighbours = 0;
                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            int index = i + y * w + x;
                            if (index >= 0 && index < cols.Length && index != i)
                            {
                                if (cols[index].a > 0.5f)
                                {
                                    cols[index].a = 1;
                                    meanCol += cols[index];
                                    opaqueNeighbours++;
                                }
                                else
                                {
                                    indexBuffer.Add(index);
                                }
                            }
                        }
                    }
                    cols[i] = meanCol / opaqueNeighbours;
                }

                indexBuffer.ExceptWith(borderIndices);

                borderIndices = indexBuffer;
                indexBuffer = new HashSet<int>();
            }
            for (int i = 0; i < cols.Length; i++)
                cols[i].a = copyCols[i].a;
            texture.SetPixels(cols);
        }


    }

}