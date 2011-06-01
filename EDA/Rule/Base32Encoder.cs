using System;
using System.Collections;
using System.IO;
using System.Diagnostics;

namespace Base32Encoding
{

    public sealed class Base32Encoder
    {

        private static char[] sm_acMapping = {	'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 
												 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
												 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
												 'y', 'z', '2', '3', '4', '5', '6', '7'
											 };

        private static Hashtable sm_htReverseMapping;

        static Base32Encoder()
        {
            sm_htReverseMapping = new Hashtable();
            for (byte yValue = 0; yValue < sm_acMapping.Length; ++yValue)
            {
                sm_htReverseMapping.Add(sm_acMapping[yValue], yValue);
            }
        }

        public static string Encode(byte[] ayInput)
        {
            if (ayInput == null)
            {
                throw new ArgumentNullException("ayInput", "A null array cannot be encoded.");
            }
            else if (ayInput.Length == 0)
            {
                return "";
            }
            else
            {
                StringWriter swOutput = new StringWriter();
                //yCurrent stores any bits carried over from the previous character
                byte yCurrent = 0;
                //iBits is the number of bits written from the previous character
                int iBits = 0;
                //iIndex is the index into the mapping array
                int iIndex = 0;
                for (int iPosition = 0; iPosition < ayInput.Length; ++iPosition)
                {
                    if (iBits == 0)
                    {
                        yCurrent = (byte)((ayInput[iPosition] & 7) << 5);
                        iIndex = ayInput[iPosition] >> 3;
                        swOutput.Write(sm_acMapping[iIndex]);
                        iBits = 5;
                    }
                    else
                    {
                        Debug.Assert(iBits > 3, "encoding error!");
                        yCurrent |= (byte)(ayInput[iPosition] >> (8 - iBits));
                        iIndex = yCurrent >> 3;
                        swOutput.Write(sm_acMapping[iIndex]);
                        //remove the five bits that were just written
                        yCurrent = (byte)((yCurrent & 7) << 5);
                        iBits = (iBits + 5) % 8;
                        if (iBits <= 3)
                        {
                            //write the next five bits of this character
                            yCurrent = (byte)(ayInput[iPosition] << iBits);
                            iIndex = yCurrent >> 3;
                            swOutput.Write(sm_acMapping[iIndex]);
                            //remove the five bits that were just written
                            yCurrent = (byte)((yCurrent & 7) << 5);
                            iBits = (iBits + 5) % 8;
                        }
                        else if (iBits == 4)
                        {
                            //don't lose the last bit of the current character
                            yCurrent |= (byte)((ayInput[iPosition] & 1) << 4);
                        }
                    }
                }
                if (iBits > 0)
                {
                    iIndex = yCurrent >> 3;
                    swOutput.Write(sm_acMapping[iIndex]);
                }
                return swOutput.ToString();
            }
        }

        public static byte[] Decode(string sInput)
        {
            if (sInput == null)
            {
                throw new ArgumentNullException("sInput", "A null string cannot be decoded.");
            }
            else if (sInput.Length == 0)
            {
                return new byte[] { };
            }
            else
            {
                //make it all lowercase
                sInput = sInput.ToLower();
                //every character represents five bits, so eight characters = five bytes
                int iArrayLength = (sInput.Length / 8) * 5;
                switch (sInput.Length % 8)
                {
                    case 0:
                        break;
                    case 2:
                        iArrayLength += 1;
                        break;
                    case 4:
                        iArrayLength += 2;
                        break;
                    case 5:
                        iArrayLength += 3;
                        break;
                    case 7:
                        iArrayLength += 4;
                        break;
                    default:
                        throw new ArgumentException("Invalid length for base32 string.");
                }
                byte[] ayReturn = new byte[iArrayLength];
                //iIndex is the current index in the return array
                int iIndex = 0;
                //iPosition is the current position in the input string
                int iPosition = 0;
                //every eight characters in the input string is five bits
                //note: for our purposes, "characters" have only five bits.
                while (iPosition + 8 <= sInput.Length)
                {
                    string sChunk = sInput.Substring(iPosition, 8);
                    iPosition += 8;
                    //byte 0 is the first character and the first three bits of the second character
                    byte b1 = (byte)(((byte)sm_htReverseMapping[sChunk[0]]) << 3);
                    byte b2 = (byte)(((byte)sm_htReverseMapping[sChunk[1]]) >> 2);
                    ayReturn[iIndex] = (byte)(((byte)sm_htReverseMapping[sChunk[0]] << 3) | ((byte)sm_htReverseMapping[sChunk[1]] >> 2));
                    //byte 1 is the last two bits of the second character, all the bits in the third character, and the first bit in the fourth character
                    ayReturn[iIndex + 1] = (byte)(((byte)sm_htReverseMapping[sChunk[1]] << 6) | ((byte)sm_htReverseMapping[sChunk[2]] << 1) | ((byte)sm_htReverseMapping[sChunk[3]] >> 4));
                    //byte 2 is the last four bits of the fourth character and the first four bits of the fifth character
                    ayReturn[iIndex + 2] = (byte)(((byte)sm_htReverseMapping[sChunk[3]] << 4) | ((byte)sm_htReverseMapping[sChunk[4]] >> 1));
                    //byte 3 is the last bit of the fifth character, all the bits in the sixth character, and the first two bits in the seventh character
                    ayReturn[iIndex + 3] = (byte)(((byte)sm_htReverseMapping[sChunk[4]] << 7) | ((byte)sm_htReverseMapping[sChunk[5]] << 2) | ((byte)sm_htReverseMapping[sChunk[6]] >> 3));
                    //byte 4 is the last three bits in the seventh character and all the bits in the eighth character
                    ayReturn[iIndex + 4] = (byte)(((byte)sm_htReverseMapping[sChunk[6]] << 5) | ((byte)sm_htReverseMapping[sChunk[7]]));
                    iIndex += 5;
                }
                if (iPosition < sInput.Length)
                {
                    string sChunk = sInput.Substring(iPosition);
                    //byte 0 is the first character and the first three bits of the second character
                    ayReturn[iIndex] = (byte)(((byte)sm_htReverseMapping[sChunk[0]] << 3) | ((byte)sm_htReverseMapping[sChunk[1]] >> 2));
                    if (sChunk.Length > 2)
                    {
                        //byte 1 is the last two bits of the second character, all the bits in the third character, and the first bit in the fourth character
                        ayReturn[iIndex + 1] = (byte)(((byte)sm_htReverseMapping[sChunk[1]] << 6) | ((byte)sm_htReverseMapping[sChunk[2]] << 1) | ((byte)sm_htReverseMapping[sChunk[3]] >> 4));
                        if (sChunk.Length > 4)
                        {
                            //byte 2 is the last four bits of the fourth character and the first four bits of the fifth character
                            ayReturn[iIndex + 2] = (byte)(((byte)sm_htReverseMapping[sChunk[3]] << 4) | ((byte)sm_htReverseMapping[sChunk[4]] >> 1));
                            if (sChunk.Length > 5)
                            {
                                //byte 3 is the last bit of the fifth character, all the bits in the sixth character, and the first two bits in the seventh character
                                ayReturn[iIndex + 3] = (byte)(((byte)sm_htReverseMapping[sChunk[4]] << 7) | ((byte)sm_htReverseMapping[sChunk[5]] << 2) | ((byte)sm_htReverseMapping[sChunk[6]] >> 3));
                                if (sChunk.Length > 7)
                                {
                                    //byte 4 is the last three bits in the seventh character and all the bits in the eighth character
                                    ayReturn[iIndex + 4] = (byte)(((byte)sm_htReverseMapping[sChunk[6]] << 5) | ((byte)sm_htReverseMapping[sChunk[7]]));
                                }
                            }
                        }
                    }
                }
                return ayReturn;
            }
        }

    }

}
