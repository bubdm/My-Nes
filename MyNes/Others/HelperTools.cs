// This file is part of My Nes
//
// A Nintendo Entertainment System / Family Computer (Nes/Famicom)
// Emulator written in C#.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// 
// Author email: mailto:alaahadidfreeware@gmail.com
//
using System.IO;
using System.Security.Cryptography;

namespace MyNes.Core
{
    /// <summary>
    /// The helper tools
    /// </summary>
    public class HelperTools
    {
        /// <summary>
        /// Get a file size as label with unit
        /// </summary>
        /// <param name="FilePath">The file path</param>
        /// <returns>The file size label with unit</returns>
        public static string GetFileSize(string FilePath)
        {
            if (File.Exists(Path.GetFullPath(FilePath)) == true)
            {
                FileInfo Info = new FileInfo(FilePath);
                string Unit = " Byte";
                double Len = Info.Length;
                if (Info.Length >= 1024)
                {
                    Len = Info.Length / 1024.00;
                    Unit = " KB";
                }
                if (Len >= 1024)
                {
                    Len /= 1024.00;
                    Unit = " MB";
                }
                if (Len >= 1024)
                {
                    Len /= 1024.00;
                    Unit = " GB";
                }
                return Len.ToString("F2") + Unit;
            }
            return "";
        }
        /// <summary>
        /// Get size label with unit
        /// </summary>
        /// <param name="size">The size in bytes</param>
        /// <returns>The size label with unit</returns>
        public static string GetSize(long size)
        {
            string Unit = " Byte";
            double Len = size;
            if (size >= 1024)
            {
                Len = size / 1024.00;
                Unit = " KB";
            }
            if (Len >= 1024)
            {
                Len /= 1024.00;
                Unit = " MB";
            }
            if (Len >= 1024)
            {
                Len /= 1024.00;
                Unit = " GB";
            }
            if (Len < 0)
                return "???";
            return Len.ToString("F2") + Unit;
        }
        /// <summary>
        /// Get size label with unit
        /// </summary>
        /// <param name="size">The size in bytes</param>
        /// <returns>The size label with unit</returns>
        public static string GetSize(ulong size)
        {
            string Unit = " Byte";
            double Len = size;
            if (size >= 1024)
            {
                Len = size / 1024.00;
                Unit = " KB";
            }
            if (Len >= 1024)
            {
                Len /= 1024.00;
                Unit = " MB";
            }
            if (Len >= 1024)
            {
                Len /= 1024.00;
                Unit = " GB";
            }
            if (Len < 0)
                return "???";
            return Len.ToString("F2") + Unit;
        }
        /// <summary>
        /// Get file size in bytes
        /// </summary>
        /// <param name="FilePath">The file path</param>
        /// <returns>The file size in bytes.</returns>
        public static long GetSizeAsBytes(string FilePath)
        {
            if (File.Exists(FilePath) == true)
            {
                FileInfo Info = new FileInfo(FilePath);
                return Info.Length;
            }
            return 0;
        }
        /// <summary>
        /// Check if string contain numbers
        /// </summary>
        /// <param name="text">The string to check</param>
        /// <returns>True if given string contain numbers otherwise false</returns>
        public static bool IsStringContainsNumbers(string text)
        {
            foreach (char chr in text.ToCharArray())
            {
                int tt = 0;
                if (int.TryParse(chr.ToString(), out tt))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Calculate file CRC
        /// </summary>
        /// <param name="filePath">The complete file path</param>
        /// <returns>File CRC</returns>
        public static string CalculateCRC(string filePath)
        {
            if (File.Exists(filePath))
            {
                Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] fileBuffer = new byte[fileStream.Length];
                fileStream.Read(fileBuffer, 0, (int)fileStream.Length);
                fileStream.Close();
                string crc = "";
                Crc32 crc32 = new Crc32();
                byte[] crc32Buffer = crc32.ComputeHash(fileBuffer);

                foreach (byte b in crc32Buffer)
                    crc += b.ToString("x2").ToLower();

                return crc;
            }
            return "";
        }
        /// <summary>
        /// Calculate file CRC
        /// </summary>
        /// <param name="filePath">The complete file path</param>
        /// <param name="bytesToSkip">How many bytes should be skipped from the file (header)</param>
        /// <returns>File CRC</returns>
        public static string CalculateCRC(string filePath, int bytesToSkip)
        {
            if (File.Exists(filePath))
            {
                Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                fileStream.Read(new byte[bytesToSkip], 0, bytesToSkip);
                byte[] fileBuffer = new byte[fileStream.Length - bytesToSkip];
                fileStream.Read(fileBuffer, 0, (int)(fileStream.Length - bytesToSkip));
                fileStream.Close();

                string crc = "";
                Crc32 crc32 = new Crc32();
                byte[] crc32Buffer = crc32.ComputeHash(fileBuffer);

                foreach (byte b in crc32Buffer)
                    crc += b.ToString("x2").ToLower();

                return crc;
            }
            return "";
        }
        /// <summary>
        /// Calculate SHA1 for file
        /// </summary>
        /// <param name="filePath">The file path to calculate SHA1 for</param>
        /// <returns>The SHA1 for given file otherwise null</returns>
        public static string CalculateSHA1(string filePath)
        {
            if (File.Exists(filePath))
            {
                byte[] fileBuffer = GetBuffer(filePath);

                string Sha1 = "";
                SHA1Managed managedSHA1 = new SHA1Managed();
                byte[] shaBuffer = managedSHA1.ComputeHash(fileBuffer);

                foreach (byte b in shaBuffer)
                    Sha1 += b.ToString("x2").ToLower();

                return Sha1;
            }
            return "";
        }
        /// <summary>
        /// Get file bytes buffer
        /// </summary>
        /// <param name="filePath">The file path to get buffer for</param>
        /// <returns>The file buffer otherwise null</returns>
        public static byte[] GetBuffer(string filePath)
        {
            byte[] fileBuffer;
            Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            fileBuffer = new byte[fileStream.Length];
            fileStream.Read(fileBuffer, 0, (int)fileStream.Length);
            fileStream.Close();

            return fileBuffer;
        }
    }
}
