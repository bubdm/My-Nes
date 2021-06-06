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
namespace MyNes.Core
{
    /*Emulates the 4016 and 4017 ports*/
    public partial class NesEmu
    {
        private static int PORT0;
        private static int PORT1;
        private static int inputStrobe;

        private static IJoypadConnecter joypad1;
        private static IJoypadConnecter joypad2;
        private static IJoypadConnecter joypad3;
        private static IJoypadConnecter joypad4;
        private static IShortcutsHandler shortucts;
        public static bool IsFourPlayers;

        private static void PORTSInitialize()
        {
            if (joypad1 == null)
                joypad1 = new BlankJoypad();
            if (joypad2 == null)
                joypad2 = new BlankJoypad();
            if (joypad3 == null)
                joypad3 = new BlankJoypad();
            if (joypad4 == null)
                joypad4 = new BlankJoypad();
            if (shortucts == null)
                shortucts = new BlankShortuctsHandler();
        }
        public static void SetupShortcutsHandler(IShortcutsHandler hh)
        {
            shortucts = hh;
        }
        public static void SetupControllers(IJoypadConnecter joy1, IJoypadConnecter joy2, IJoypadConnecter joy3, IJoypadConnecter joy4)
        {
            joypad1 = joy1;
            joypad2 = joy2;
            joypad3 = joy3;
            joypad4 = joy4;
        }
        public static void SetupVSUnisystemDIP(IVSUnisystemDIPConnecter uni)
        {
            // TODO: setup IVSUnisystemDIPConnecter
        }
        public static void SetupControllersP1(IJoypadConnecter joy)
        {
            joypad1 = joy;
        }
        public static void SetupControllersP2(IJoypadConnecter joy)
        {
            joypad2 = joy;
        }
        public static void SetupControllersP3(IJoypadConnecter joy)
        {
            joypad3 = joy;
        }
        public static void SetupControllersP4(IJoypadConnecter joy)
        {
            joypad4 = joy;
        }
        public static void DestroyJoypads()
        {
            if (joypad1 == null)
                joypad1 = new BlankJoypad();
            else
                joypad1.Destroy();
            if (joypad2 == null)
                joypad2 = new BlankJoypad();
            else
                joypad1.Destroy();
            if (joypad3 == null)
                joypad3 = new BlankJoypad();
            else
                joypad1.Destroy();
            if (joypad4 == null)
                joypad4 = new BlankJoypad();
            else
                joypad1.Destroy();
        }
        private static void PORTWriteState(ref System.IO.BinaryWriter bin)
        {
            bin.Write(PORT0);
            bin.Write(PORT1);
            bin.Write(inputStrobe);
        }
        private static void PORTReadState(ref System.IO.BinaryReader bin)
        {
            PORT0 = bin.ReadInt32();
            PORT1 = bin.ReadInt32();
            inputStrobe = bin.ReadInt32();
        }
    }
}
