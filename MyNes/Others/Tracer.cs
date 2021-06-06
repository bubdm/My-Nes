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
using System;
using System.Diagnostics;

namespace MyNes.Core
{
    public sealed class Tracer
    {
        public static event EventHandler<TracerEventArgs> EventRaised;

        public static void WriteLine(string message)
        {
            EventRaised?.Invoke(null, new TracerEventArgs(message, TracerStatus.Normal));
            Trace.WriteLine(message);
        }
        public static void WriteLine(string message, string category)
        {
            EventRaised?.Invoke(null, new TracerEventArgs(string.Format("{0}: {1}", category, message), TracerStatus.Normal));
            Trace.WriteLine(string.Format("{0}: {1}", category, message));
        }
        public static void WriteLine(string message, TracerStatus status)
        {
            EventRaised?.Invoke(null, new TracerEventArgs(message, status));
            Trace.WriteLine(message);
        }
        public static void WriteLine(string message, string category, TracerStatus status)
        {
            EventRaised?.Invoke(null, new TracerEventArgs(string.Format("{0}: {1}", category, message), status));
            Trace.WriteLine(string.Format("{0}: {1}", category, message));
        }
        public static void WriteError(string message)
        {
            WriteLine(message, TracerStatus.Error);
        }
        public static void WriteError(string message, string category)
        {
            WriteLine(message, category, TracerStatus.Error);
        }
        public static void WriteWarning(string message)
        {
            WriteLine(message, TracerStatus.Warning);
        }
        public static void WriteWarning(string message, string category)
        {
            WriteLine(message, category, TracerStatus.Warning);
        }
        public static void WriteInformation(string message)
        {
            WriteLine(message, TracerStatus.Infromation);
        }
        public static void WriteInformation(string message, string category)
        {
            WriteLine(message, category, TracerStatus.Infromation);
        }
    }
}
