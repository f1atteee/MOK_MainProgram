﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOK_MainInterface.Cypher
{
    class BruteForceAlgo
    {

        public delegate bool DelBruteForceAlgo(ref char[] inputs);


        public static int BruteForce(string inputs, int startLength, int endLength, DelBruteForceAlgo testCallback)
        {
            int attemps = 0;
            for (int len = startLength; len <= endLength; ++len)
            {
                char[] chars = new char[len];

                for (int i = 0; i < len; ++i)
                    chars[i] = inputs[0];

                if (testCallback(ref chars))
                    return attemps;

                for (int i1 = len - 1; i1 > -1; --i1)
                {
                    int i2 = 0;
                    attemps++;
                    for (i2 = inputs.IndexOf(chars[i1]) + 1; i2 < inputs.Length; ++i2)
                    {
                        chars[i1] = inputs[i2];

                        if (testCallback(ref chars))
                            return attemps;

                        for (int i3 = i1 + 1; i3 < len; ++i3)
                        {
                            if (chars[i3] != inputs[inputs.Length - 1])
                            {
                                i1 = len;
                                goto outerBreak;
                            }
                        }
                    }

                outerBreak:
                    if (i2 == inputs.Length)
                        chars[i1] = inputs[0];
                }
            }

            return attemps;
        }


        public static int MyBruteForce(string text, DelBruteForceAlgo testCallback)
        {
            int steps = 0;
            for (int len = 1; len <= text.Length; ++len)
            {
                char[] chars = new char[len];

                for (int i = 0; i < len; ++i)
                    chars[i] = text[0];

                if (testCallback(ref chars))
                    return steps;

                for (int i1 = len - 1; i1 > -1; --i1)
                {
                    int i2 = 0;
                    
                    for (i2 = text.IndexOf(chars[i1]) + 1; i2 < text.Length; ++i2)
                    {
                        chars[i1] = text[i2];

                        if (testCallback(ref chars))
                            return steps;

                        for (int i3 = i1 + 1; i3 < len; ++i3)
                        {
                            steps++;
                            if (chars[i3] != text[text.Length - 1])
                            {
                                i1 = len;
                                goto outerBreak;
                            }
                        }
                    }

                outerBreak:
                    if (i2 == text.Length)
                        chars[i1] = text[0];
                }
            }

            return steps;
        }

    }
}
