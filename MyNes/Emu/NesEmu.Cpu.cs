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
using System.Runtime.InteropServices;

namespace MyNes.Core
{
    /*CPU 6502 Section*/
    // TODO: 5-branch_delays_irq test of cpu_interrupts_v2 tests fails.
    public partial class NesEmu
    {
        [StructLayout(LayoutKind.Explicit)]
        struct CPURegister
        {
            [FieldOffset(0)]
            internal byte l;
            [FieldOffset(1)]
            internal byte h;
            [FieldOffset(0)]
            internal ushort v;
        }

        private static CPURegister cpu_reg_pc;
        private static CPURegister cpu_reg_sp;
        private static CPURegister cpu_reg_ea;

        private static byte cpu_reg_a;
        private static byte cpu_reg_x;
        private static byte cpu_reg_y;
        // flags
        private static bool cpu_flag_n;
        private static bool cpu_flag_v;
        private static bool cpu_flag_d;
        private static bool cpu_flag_i;
        private static bool cpu_flag_z;
        private static bool cpu_flag_c;
        private static byte register_p
        {
            get
            {
                return (byte)(
                    (cpu_flag_n ? 0x80 : 0) |
                    (cpu_flag_v ? 0x40 : 0) |
                    (cpu_flag_d ? 0x08 : 0) |
                    (cpu_flag_i ? 0x04 : 0) |
                    (cpu_flag_z ? 0x02 : 0) |
                    (cpu_flag_c ? 0x01 : 0) | 0x20);
            }
            set
            {
                cpu_flag_n = (value & 0x80) != 0;
                cpu_flag_v = (value & 0x40) != 0;
                cpu_flag_d = (value & 0x08) != 0;
                cpu_flag_i = (value & 0x04) != 0;
                cpu_flag_z = (value & 0x02) != 0;
                cpu_flag_c = (value & 0x01) != 0;
            }
        }
        private static byte register_pb()
        {
            return (byte)(
                (cpu_flag_n ? 0x80 : 0) |
                (cpu_flag_v ? 0x40 : 0) |
                (cpu_flag_d ? 0x08 : 0) |
                (cpu_flag_i ? 0x04 : 0) |
                (cpu_flag_z ? 0x02 : 0) |
                (cpu_flag_c ? 0x01 : 0) | 0x30);
        }
        private static byte cpu_m;
        private static byte cpu_opcode;
        // Using temp values increase performance by avoiding memory allocation.
        private static byte cpu_byte_temp;
        private static int cpu_int_temp;
        private static int cpu_int_temp1;
        private static byte cpu_dummy;
        private static bool cpu_bool_tmp;
        private static CPURegister temp_add;

        private static bool CPU_IRQ_PIN;
        private static bool CPU_NMI_PIN;
        private static bool cpu_suspend_nmi;
        private static bool cpu_suspend_irq;

        // Addressing modes map
        private static Action[] cpu_addressings;
        // Instructions map
        private static Action[] cpu_instructions;

        private static void CPUInitialize()
        {
            cpu_addressings = new Action[]
            {
              //0x0,     0x1,     0x2,     0x3,     0x4,     0x5,     0x6,     0x7,     0x8,     0x9,    0xA,      0xB,     0xC,     0xD,     0xE,     0xF     //////
         /*0x0*/Imp____, IndX_R_, ImA____, IndX_W_, Zpg_R__, Zpg_R__, Zpg_RW_, Zpg_W__, ImA____, Imm____, ImA____, Imm____, Abs_R__, Abs_R__, Abs_RW_, Abs_W__,// 0x0
         /*0x1*/Imp____, IndY_R_, Imp____, IndY_W_, ZpgX_R_, ZpgX_R_, ZpgX_RW, ZpgX_W_, ImA____, AbsY_R_, ImA____, AbsY_W_, AbsX_R_, AbsX_R_, AbsX_RW, AbsX_W_,// 0x1
         /*0x2*/Imp____, IndX_R_, ImA____, IndX_W_, Zpg_R__, Zpg_R__, Zpg_RW_, Zpg_W__, ImA____, Imm____, ImA____, Imm____, Abs_R__, Abs_R__, Abs_RW_, Abs_W__,// 0x2
         /*0x3*/Imp____, IndY_R_, Imp____, IndY_W_, ZpgX_R_, ZpgX_R_, ZpgX_RW, ZpgX_W_, ImA____, AbsY_R_, ImA____, AbsY_W_, AbsX_R_, AbsX_R_, AbsX_RW, AbsX_W_,// 0x3
         /*0x4*/ImA____, IndX_R_, ImA____, IndX_W_, Zpg_R__, Zpg_R__, Zpg_RW_, Zpg_W__, ImA____, Imm____, ImA____, Imm____, Abs_W__, Abs_R__, Abs_RW_, Abs_W__,// 0x4
         /*0x5*/Imp____, IndY_R_, Imp____, IndY_W_, ZpgX_R_, ZpgX_R_, ZpgX_RW, ZpgX_W_, ImA____, AbsY_R_, ImA____, AbsY_W_, AbsX_R_, AbsX_R_, AbsX_RW, AbsX_W_,// 0x5
         /*0x6*/ImA____, IndX_R_, ImA____, IndX_W_, Zpg_R__, Zpg_R__, Zpg_RW_, Zpg_W__, ImA____, Imm____, ImA____, Imm____, Imp____, Abs_R__, Abs_RW_, Abs_W__,// 0x6
         /*0x7*/Imp____, IndY_R_, Imp____, IndY_W_, ZpgX_R_, ZpgX_R_, ZpgX_RW, ZpgX_W_, ImA____, AbsY_R_, ImA____, AbsY_W_, AbsX_R_, AbsX_R_, AbsX_RW, AbsX_W_,// 0x7
         /*0x8*/Imm____, IndX_W_, Imm____, IndX_W_, Zpg_W__, Zpg_W__, Zpg_W__, Zpg_W__, ImA____, Imm____, ImA____, Imm____, Abs_W__, Abs_W__, Abs_W__, Abs_W__,// 0x8
         /*0x9*/Imp____, IndY_W_, Imp____, IndY_W_, ZpgX_W_, ZpgX_W_, ZpgY_W_, ZpgY_W_, ImA____, AbsY_W_, ImA____, AbsY_W_, Abs_W__, AbsX_W_, Abs_W__, AbsY_W_,// 0x9
         /*0xA*/Imm____, IndX_R_, Imm____, IndX_R_, Zpg_R__, Zpg_R__, Zpg_R__, Zpg_R__, ImA____, Imm____, ImA____, Imm____, Abs_R__, Abs_R__, Abs_R__, Abs_R__,// 0xA
         /*0xB*/Imp____, IndY_R_, Imp____, IndY_R_, ZpgX_R_, ZpgX_R_, ZpgY_R_, ZpgY_R_, ImA____, AbsY_R_, ImA____, AbsY_R_, AbsX_R_, AbsX_R_, AbsY_R_, AbsY_R_,// 0xB
         /*0xC*/Imm____, IndX_R_, Imm____, IndX_R_, Zpg_R__, Zpg_R__, Zpg_RW_, Zpg_R__, ImA____, Imm____, ImA____, Imm____, Abs_R__, Abs_R__, Abs_RW_, Abs_R__,// 0xC
         /*0xD*/Imp____, IndY_R_, Imp____, IndY_RW, ZpgX_R_, ZpgX_R_, ZpgX_RW, ZpgX_RW, ImA____, AbsY_R_, ImA____, AbsY_RW, AbsX_R_, AbsX_R_, AbsX_RW, AbsX_RW,// 0xD
         /*0xE*/Imm____, IndX_R_, Imm____, IndX_W_, Zpg_R__, Zpg_R__, Zpg_RW_, Zpg_W__, ImA____, Imm____, ImA____, Imm____, Abs_R__, Abs_R__, Abs_RW_, Abs_W__,// 0xE
         /*0xF*/Imp____, IndY_R_, Imp____, IndY_W_, ZpgX_R_, ZpgX_R_, ZpgX_RW, ZpgX_W_, ImA____, AbsY_R_, ImA____, AbsY_W_, AbsX_R_, AbsX_R_, AbsX_RW, AbsX_W_,// 0xF
            };
            cpu_instructions = new Action[]
            {
              //0x0,   0x1,   0x2,   0x3,   0x4,   0x5,   0x6,   0x7,   0x8,   0x9,   0xA,   0xB,   0xC,   0xD,   0xE,   0xF   //////
         /*0x0*/BRK__, ORA__, NOP__, SLO__, NOP__, ORA__, ASL_M, SLO__, PHP__, ORA__, ASL_A, ANC__, NOP__, ORA__, ASL_M, SLO__,// 0x0
         /*0x1*/BPL__, ORA__, NOP__, SLO__, NOP__, ORA__, ASL_M, SLO__, CLC__, ORA__, NOP__, SLO__, NOP__, ORA__, ASL_M, SLO__,// 0x1
         /*0x2*/JSR__, AND__, NOP__, RLA__, BIT__, AND__, ROL_M, RLA__, PLP__, AND__, ROL_A, ANC__, BIT__, AND__, ROL_M, RLA__,// 0x2
         /*0x3*/BMI__, AND__, NOP__, RLA__, NOP__, AND__, ROL_M, RLA__, SEC__, AND__, NOP__, RLA__, NOP__, AND__, ROL_M, RLA__,// 0x3
         /*0x4*/RTI__, EOR__, NOP__, SRE__, NOP__, EOR__, LSR_M, SRE__, PHA__, EOR__, LSR_A, ALR__, JMP__, EOR__, LSR_M, SRE__,// 0x4
         /*0x5*/BVC__, EOR__, NOP__, SRE__, NOP__, EOR__, LSR_M, SRE__, CLI__, EOR__, NOP__, SRE__, NOP__, EOR__, LSR_M, SRE__,// 0x5
         /*0x6*/RTS__, ADC__, NOP__, RRA__, NOP__, ADC__, ROR_M, RRA__, PLA__, ADC__, ROR_A, ARR__, JMP_I, ADC__, ROR_M, RRA__,// 0x6
         /*0x7*/BVS__, ADC__, NOP__, RRA__, NOP__, ADC__, ROR_M, RRA__, SEI__, ADC__, NOP__, RRA__, NOP__, ADC__, ROR_M, RRA__,// 0x7
         /*0x8*/NOP__, STA__, NOP__, SAX__, STY__, STA__, STX__, SAX__, DEY__, NOP__, TXA__, XAA__, STY__, STA__, STX__, SAX__,// 0x8
         /*0x9*/BCC__, STA__, NOP__, AHX__, STY__, STA__, STX__, SAX__, TYA__, STA__, TXS__, XAS__, SHY__, STA__, SHX__, AHX__,// 0x9
         /*0xA*/LDY__, LDA__, LDX__, LAX__, LDY__, LDA__, LDX__, LAX__, TAY__, LDA__, TAX__, LAX__, LDY__, LDA__, LDX__, LAX__,// 0xA
         /*0xB*/BCS__, LDA__, NOP__, LAX__, LDY__, LDA__, LDX__, LAX__, CLV__, LDA__, TSX__, LAR__, LDY__, LDA__, LDX__, LAX__,// 0xB
         /*0xC*/CPY__, CMP__, NOP__, DCP__, CPY__, CMP__, DEC__, DCP__, INY__, CMP__, DEX__, AXS__, CPY__, CMP__, DEC__, DCP__,// 0xC
         /*0xD*/BNE__, CMP__, NOP__, DCP__, NOP__, CMP__, DEC__, DCP__, CLD__, CMP__, NOP__, DCP__, NOP__, CMP__, DEC__, DCP__,// 0xD
         /*0xE*/CPX__, SBC__, NOP__, ISC__, CPX__, SBC__, INC__, ISC__, INX__, SBC__, NOP__, SBC__, CPX__, SBC__, INC__, ISC__,// 0xE
         /*0xF*/BEQ__, SBC__, NOP__, ISC__, NOP__, SBC__, INC__, ISC__, SED__, SBC__, NOP__, ISC__, NOP__, SBC__, INC__, ISC__,// 0xF
            };
        }
        private static void CPUClock()
        {
            // First clock is to fetch opcode
            Read(ref cpu_reg_pc.v, out cpu_opcode);
            cpu_reg_pc.v++;

            cpu_addressings[cpu_opcode]();
            cpu_instructions[cpu_opcode]();
            // Handle interrupts...
            if (CPU_IRQ_PIN || CPU_NMI_PIN)
            {
                Read(ref cpu_reg_pc.v, out cpu_dummy);
                Read(ref cpu_reg_pc.v, out cpu_dummy);
                Interrupt();
            }
        }

        private static void CPUHardReset()
        {
            // registers
            cpu_reg_a = 0x00;
            cpu_reg_x = 0x00;
            cpu_reg_y = 0x00;

            cpu_reg_sp.l = 0xFD;
            cpu_reg_sp.h = 0x01;

            ushort rst = 0xFFFC;
            mem_board.ReadPRG(ref rst, out cpu_reg_pc.l);
            rst++;
            mem_board.ReadPRG(ref rst, out cpu_reg_pc.h);
            register_p = 0;
            cpu_flag_i = true;
            cpu_reg_ea.v = 0;
            cpu_opcode = 0;
            //interrupts
            CPU_IRQ_PIN = false;
            CPU_NMI_PIN = false;
            cpu_suspend_nmi = false;
            cpu_suspend_irq = false;
            IRQFlags = 0;
        }
        private static void CPUSoftReset()
        {
            cpu_flag_i = true;
            cpu_reg_sp.v -= 3;

            ushort add = 0xFFFC;
            Read(ref add, out cpu_reg_pc.l);
            add++;
            Read(ref add, out cpu_reg_pc.h);
        }

        #region Addressing modes
        /*
         * _R: read access instructions, set the M value. Some addressing modes will execute 1 extra cycle when page crossed.
         * _W: write access instructions, doesn't set the M value. The instruction should handle write at effective address (EF).
         * _RW: Read-Modify-Write instructions, set the M value and the instruction should handle write at effective address (EF).
         */
        private static void Imp____()
        {
            // No addressing mode ...
        }
        private static void IndX_R_()
        {
            temp_add.h = 0;// the zero page boundary crossing is not handled.
            Read(ref cpu_reg_pc.v, out temp_add.l);
            cpu_reg_pc.v++;// CLock 1
            Read(ref temp_add.v, out cpu_dummy);// Clock 2
            temp_add.l += cpu_reg_x;

            Read(ref temp_add.v, out cpu_reg_ea.l);// Clock 3
            temp_add.l++;

            Read(ref temp_add.v, out cpu_reg_ea.h);// Clock 4

            Read(ref cpu_reg_ea.v, out cpu_m);
        }
        private static void IndX_W_()
        {
            temp_add.h = 0;// the zero page boundary crossing is not handled.
            Read(ref cpu_reg_pc.v, out temp_add.l);
            cpu_reg_pc.v++;// CLock 1
            Read(ref temp_add.v, out cpu_dummy);// Clock 2
            temp_add.l += cpu_reg_x;

            Read(ref temp_add.v, out cpu_reg_ea.l);// Clock 3
            temp_add.l++;

            Read(ref temp_add.v, out cpu_reg_ea.h);// Clock 4
        }
        private static void IndX_RW()
        {
            temp_add.h = 0;// the zero page boundary crossing is not handled.
            Read(ref cpu_reg_pc.v, out temp_add.l);
            cpu_reg_pc.v++;// CLock 1
            Read(ref temp_add.v, out cpu_dummy);// Clock 2
            temp_add.l += cpu_reg_x;

            Read(ref temp_add.v, out cpu_reg_ea.l);// Clock 3
            temp_add.l++;

            Read(ref temp_add.v, out cpu_reg_ea.h);// Clock 4

            Read(ref cpu_reg_ea.v, out cpu_m);
        }
        private static void IndY_R_()
        {
            temp_add.h = 0;// the zero page boundary crossing is not handled.
            Read(ref cpu_reg_pc.v, out temp_add.l);
            cpu_reg_pc.v++;// CLock 1
            Read(ref temp_add.v, out cpu_reg_ea.l);// Clock 3
            temp_add.l++;// Clock 2
            Read(ref temp_add.v, out cpu_reg_ea.h);// Clock 4

            cpu_reg_ea.l += cpu_reg_y;

            Read(ref cpu_reg_ea.v, out cpu_m);
            if (cpu_reg_ea.l < cpu_reg_y)
            {
                cpu_reg_ea.h++;
                Read(ref cpu_reg_ea.v, out cpu_m);
            }
        }
        private static void IndY_W_()
        {
            temp_add.h = 0;// the zero page boundary crossing is not handled.
            Read(ref cpu_reg_pc.v, out temp_add.l);
            cpu_reg_pc.v++;// CLock 1

            Read(ref temp_add.v, out cpu_reg_ea.l);
            temp_add.l++;// Clock 2

            Read(ref temp_add.v, out cpu_reg_ea.h);// Clock 2
            cpu_reg_ea.l += cpu_reg_y;

            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 3
            if (cpu_reg_ea.l < cpu_reg_y)
                cpu_reg_ea.h++;
        }
        private static void IndY_RW()
        {
            temp_add.h = 0;// the zero page boundary crossing is not handled.
            Read(ref cpu_reg_pc.v, out temp_add.l);
            cpu_reg_pc.v++;// CLock 1
            Read(ref temp_add.v, out cpu_reg_ea.l);
            temp_add.l++;// Clock 2
            Read(ref temp_add.v, out cpu_reg_ea.h);// Clock 2

            cpu_reg_ea.l += cpu_reg_y;

            Read(ref cpu_reg_ea.v, out cpu_dummy);// Clock 3
            if (cpu_reg_ea.l < cpu_reg_y)
                cpu_reg_ea.h++;

            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 3
        }
        private static void Zpg_R__()
        {
            cpu_reg_ea.h = 0;
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 3
        }
        private static void Zpg_W__()
        {
            cpu_reg_ea.h = 0;
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
        }
        private static void Zpg_RW_()
        {
            cpu_reg_ea.h = 0;
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 3
        }
        private static void ZpgX_R_()
        {
            cpu_reg_ea.h = 0;
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_ea.v, out cpu_dummy);// Clock 2
            cpu_reg_ea.l += cpu_reg_x;
            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 3
        }
        private static void ZpgX_W_()
        {
            cpu_reg_ea.h = 0;
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_ea.v, out cpu_dummy);// Clock 2
            cpu_reg_ea.l += cpu_reg_x;
        }
        private static void ZpgX_RW()
        {
            cpu_reg_ea.h = 0;
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_ea.v, out cpu_dummy);// Clock 2
            cpu_reg_ea.l += cpu_reg_x;
            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 3
        }
        private static void ZpgY_R_()
        {
            cpu_reg_ea.h = 0;
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_ea.v, out cpu_dummy);// Clock 2
            cpu_reg_ea.l += cpu_reg_y;
            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 3
        }
        private static void ZpgY_W_()
        {
            cpu_reg_ea.h = 0;
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_ea.v, out cpu_dummy);// Clock 2
            cpu_reg_ea.l += cpu_reg_y;
        }
        private static void ZpgY_RW()
        {
            cpu_reg_ea.h = 0;
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_ea.v, out cpu_dummy);// Clock 2
            cpu_reg_ea.l += cpu_reg_y;
            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 3
        }
        private static void Imm____()
        {
            Read(ref cpu_reg_pc.v, out cpu_m);
            cpu_reg_pc.v++;// Clock 1
        }
        private static void ImA____()
        {
            Read(ref cpu_reg_pc.v, out cpu_dummy);
        }
        private static void Abs_R__()
        {
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.h);
            cpu_reg_pc.v++;// Clock 2
            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 3
        }
        private static void Abs_W__()
        {
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.h);
            cpu_reg_pc.v++;// Clock 2
        }
        private static void Abs_RW_()
        {
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.h);
            cpu_reg_pc.v++;// Clock 2
            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 3
        }
        private static void AbsX_R_()
        {
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.h);
            cpu_reg_pc.v++;// Clock 2

            cpu_reg_ea.l += cpu_reg_x;

            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 3
            if (cpu_reg_ea.l < cpu_reg_x)
            {
                cpu_reg_ea.h++;
                Read(ref cpu_reg_ea.v, out cpu_m);// Clock 4
            }
        }
        private static void AbsX_W_()
        {
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.h);
            cpu_reg_pc.v++;// Clock 2

            cpu_reg_ea.l += cpu_reg_x;

            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 4
            if (cpu_reg_ea.l < cpu_reg_x)
                cpu_reg_ea.h++;
        }
        private static void AbsX_RW()
        {
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.h);
            cpu_reg_pc.v++;// Clock 2

            cpu_reg_ea.l += cpu_reg_x;

            Read(ref cpu_reg_ea.v, out cpu_dummy);// Clock 3
            if (cpu_reg_ea.l < cpu_reg_x)
                cpu_reg_ea.h++;

            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 4
        }
        private static void AbsY_R_()
        {
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.h);
            cpu_reg_pc.v++;// Clock 2

            cpu_reg_ea.l += cpu_reg_y;

            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 4
            if (cpu_reg_ea.l < cpu_reg_y)
            {
                cpu_reg_ea.h++;
                Read(ref cpu_reg_ea.v, out cpu_m);// Clock 4
            }
        }
        private static void AbsY_W_()
        {
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.h);
            cpu_reg_pc.v++;// Clock 2

            cpu_reg_ea.l += cpu_reg_y;

            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 4
            if (cpu_reg_ea.l < cpu_reg_y)
                cpu_reg_ea.h++;
        }
        private static void AbsY_RW()
        {
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;// Clock 1
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.h);
            cpu_reg_pc.v++;// Clock 2

            cpu_reg_ea.l += cpu_reg_y;

            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 4
            if (cpu_reg_ea.l < cpu_reg_y)
                cpu_reg_ea.h++;

            Read(ref cpu_reg_ea.v, out cpu_m);// Clock 4
        }
        #endregion

        #region Instructions
        private static void Interrupt()
        {
            Push(ref cpu_reg_pc.h);
            Push(ref cpu_reg_pc.l);
            cpu_dummy = cpu_opcode == 0 ? register_pb() : register_p;
            Push(ref cpu_dummy);

            // pins are detected during φ2 of previous cycle (before push about 2 ppu cycles)
            temp_add.v = InterruptVector;

            // THEORY: 
            // Once the vector requested, the interrupts are suspended and cleared
            // by setting the I flag and clearing the nmi detect flag. Also, the nmi
            // detection get suspended for 2 cycles while pulling PC, irq still can
            // be detected but will not be taken since I is set.
            cpu_suspend_nmi = true;
            cpu_flag_i = true;
            CPU_NMI_PIN = false;

            Read(ref temp_add.v, out cpu_reg_pc.l);
            temp_add.v++;
            Read(ref temp_add.v, out cpu_reg_pc.h);

            cpu_suspend_nmi = false;
        }
        private static void Branch(ref bool condition)
        {
            Read(ref cpu_reg_pc.v, out cpu_byte_temp);
            cpu_reg_pc.v++;

            if (condition)
            {
                cpu_suspend_irq = true;
                Read(ref cpu_reg_pc.v, out cpu_dummy);
                cpu_reg_pc.l += cpu_byte_temp;
                cpu_suspend_irq = false;
                if (cpu_byte_temp >= 0x80)
                {
                    if (cpu_reg_pc.l >= cpu_byte_temp)
                    {
                        Read(ref cpu_reg_pc.v, out cpu_dummy);
                        cpu_reg_pc.h--;
                    }
                }
                else
                {
                    if (cpu_reg_pc.l < cpu_byte_temp)
                    {
                        Read(ref cpu_reg_pc.v, out cpu_dummy);
                        cpu_reg_pc.h++;
                    }
                }
            }
        }
        private static void Push(ref byte val)
        {
            Write(ref cpu_reg_sp.v, ref val);
            cpu_reg_sp.l--;
        }
        private static void Pull(out byte val)
        {
            cpu_reg_sp.l++;
            Read(ref cpu_reg_sp.v, out val);
        }
        private static void ADC__()
        {
            cpu_int_temp = (cpu_reg_a + cpu_m + (cpu_flag_c ? 1 : 0));

            cpu_flag_v = ((cpu_int_temp ^ cpu_reg_a) & (cpu_int_temp ^ cpu_m) & 0x80) != 0;
            cpu_flag_n = (cpu_int_temp & 0x80) != 0;
            cpu_flag_z = (cpu_int_temp & 0xFF) == 0;
            cpu_flag_c = (cpu_int_temp >> 0x8) != 0;

            cpu_reg_a = (byte)(cpu_int_temp & 0xFF);
        }
        private static void AHX__()
        {
            cpu_byte_temp = (byte)((cpu_reg_a & cpu_reg_x) & 7);
            Write(ref cpu_reg_ea.v, ref cpu_byte_temp);
        }
        private static void ALR__()
        {
            cpu_reg_a &= cpu_m;

            cpu_flag_c = (cpu_reg_a & 0x01) != 0;

            cpu_reg_a >>= 1;

            cpu_flag_n = (cpu_reg_a & 0x80) != 0;
            cpu_flag_z = cpu_reg_a == 0;
        }
        private static void ANC__()
        {
            cpu_reg_a &= cpu_m;
            cpu_flag_n = (cpu_reg_a & 0x80) != 0;
            cpu_flag_z = cpu_reg_a == 0;
            cpu_flag_c = (cpu_reg_a & 0x80) != 0;
        }
        private static void AND__()
        {
            cpu_reg_a &= cpu_m;
            cpu_flag_n = (cpu_reg_a & 0x80) == 0x80;
            cpu_flag_z = (cpu_reg_a == 0);
        }
        private static void ARR__()
        {
            cpu_reg_a = (byte)(((cpu_m & cpu_reg_a) >> 1) | (cpu_flag_c ? 0x80 : 0x00));

            cpu_flag_z = (cpu_reg_a & 0xFF) == 0;
            cpu_flag_n = (cpu_reg_a & 0x80) != 0;
            cpu_flag_c = (cpu_reg_a & 0x40) != 0;
            cpu_flag_v = ((cpu_reg_a << 1 ^ cpu_reg_a) & 0x40) != 0;
        }
        private static void AXS__()
        {
            cpu_int_temp = (cpu_reg_a & cpu_reg_x) - cpu_m;

            cpu_flag_n = (cpu_int_temp & 0x80) != 0;
            cpu_flag_z = (cpu_int_temp & 0xFF) == 0;
            cpu_flag_c = (~cpu_int_temp >> 8) != 0;

            cpu_reg_x = (byte)(cpu_int_temp & 0xFF);
        }
        private static void ASL_M()
        {
            cpu_flag_c = (cpu_m & 0x80) == 0x80;
            Write(ref cpu_reg_ea.v, ref cpu_m);

            cpu_m = (byte)((cpu_m << 1) & 0xFE);

            Write(ref cpu_reg_ea.v, ref cpu_m);

            cpu_flag_n = (cpu_m & 0x80) == 0x80;
            cpu_flag_z = (cpu_m == 0);
        }
        private static void ASL_A()
        {
            cpu_flag_c = (cpu_reg_a & 0x80) == 0x80;

            cpu_reg_a = (byte)((cpu_reg_a << 1) & 0xFE);

            cpu_flag_n = (cpu_reg_a & 0x80) == 0x80;
            cpu_flag_z = (cpu_reg_a == 0);
        }
        private static void BCC__()
        {
            cpu_bool_tmp = !cpu_flag_c;
            Branch(ref cpu_bool_tmp);
        }
        private static void BCS__()
        {
            Branch(ref cpu_flag_c);
        }
        private static void BEQ__()
        {
            Branch(ref cpu_flag_z);
        }
        private static void BIT__()
        {
            cpu_flag_n = (cpu_m & 0x80) != 0;
            cpu_flag_v = (cpu_m & 0x40) != 0;
            cpu_flag_z = (cpu_m & cpu_reg_a) == 0;
        }
        private static void BRK__()
        {
            Read(ref cpu_reg_pc.v, out cpu_dummy);
            cpu_reg_pc.v++;
            Interrupt();
        }
        private static void BPL__()
        {
            cpu_bool_tmp = !cpu_flag_n;
            Branch(ref cpu_bool_tmp);
        }
        private static void BNE__()
        {
            cpu_bool_tmp = !cpu_flag_z;
            Branch(ref cpu_bool_tmp);
        }
        private static void BMI__()
        {
            Branch(ref cpu_flag_n);
        }
        private static void BVC__()
        {
            cpu_bool_tmp = !cpu_flag_v;
            Branch(ref cpu_bool_tmp);
        }
        private static void BVS__()
        {
            Branch(ref cpu_flag_v);
        }
        private static void SED__()
        {
            cpu_flag_d = true;
        }
        private static void CLC__()
        {
            cpu_flag_c = false;
        }
        private static void CLD__()
        {
            cpu_flag_d = false;
        }
        private static void CLV__()
        {
            cpu_flag_v = false;
        }
        private static void CMP__()
        {
            cpu_int_temp = cpu_reg_a - cpu_m;
            cpu_flag_n = (cpu_int_temp & 0x80) == 0x80;
            cpu_flag_c = (cpu_reg_a >= cpu_m);
            cpu_flag_z = (cpu_int_temp == 0);
        }
        private static void CPX__()
        {
            cpu_int_temp = cpu_reg_x - cpu_m;
            cpu_flag_n = (cpu_int_temp & 0x80) == 0x80;
            cpu_flag_c = (cpu_reg_x >= cpu_m);
            cpu_flag_z = (cpu_int_temp == 0);
        }
        private static void CPY__()
        {
            cpu_int_temp = cpu_reg_y - cpu_m;
            cpu_flag_n = (cpu_int_temp & 0x80) == 0x80;
            cpu_flag_c = (cpu_reg_y >= cpu_m);
            cpu_flag_z = (cpu_int_temp == 0);
        }
        private static void CLI__()
        {
            cpu_flag_i = false;
        }
        private static void DCP__()
        {
            Write(ref cpu_reg_ea.v, ref cpu_m);

            cpu_m--;
            Write(ref cpu_reg_ea.v, ref cpu_m);

            cpu_int_temp = cpu_reg_a - cpu_m;

            cpu_flag_n = (cpu_int_temp & 0x80) != 0;
            cpu_flag_z = cpu_int_temp == 0;
            cpu_flag_c = (~cpu_int_temp >> 8) != 0;
        }
        private static void DEC__()
        {
            Write(ref cpu_reg_ea.v, ref cpu_m);
            cpu_m--;
            Write(ref cpu_reg_ea.v, ref cpu_m);
            cpu_flag_n = (cpu_m & 0x80) == 0x80;
            cpu_flag_z = (cpu_m == 0);
        }
        private static void DEY__()
        {
            cpu_reg_y--;
            cpu_flag_z = (cpu_reg_y == 0);
            cpu_flag_n = (cpu_reg_y & 0x80) == 0x80;
        }
        private static void DEX__()
        {
            cpu_reg_x--;
            cpu_flag_z = (cpu_reg_x == 0);
            cpu_flag_n = (cpu_reg_x & 0x80) == 0x80;
        }
        private static void EOR__()
        {
            cpu_reg_a ^= cpu_m;
            cpu_flag_n = (cpu_reg_a & 0x80) == 0x80;
            cpu_flag_z = (cpu_reg_a == 0);
        }
        private static void INC__()
        {
            Write(ref cpu_reg_ea.v, ref cpu_m);
            cpu_m++;
            Write(ref cpu_reg_ea.v, ref cpu_m);
            cpu_flag_n = (cpu_m & 0x80) == 0x80;
            cpu_flag_z = (cpu_m == 0);
        }
        private static void INX__()
        {
            cpu_reg_x++;
            cpu_flag_z = (cpu_reg_x == 0);
            cpu_flag_n = (cpu_reg_x & 0x80) == 0x80;
        }
        private static void INY__()
        {
            cpu_reg_y++;
            cpu_flag_n = (cpu_reg_y & 0x80) == 0x80;
            cpu_flag_z = (cpu_reg_y == 0);
        }
        private static void ISC__()
        {
            Read(ref cpu_reg_ea.v, out cpu_byte_temp);

            Write(ref cpu_reg_ea.v, ref cpu_byte_temp);

            cpu_byte_temp++;

            Write(ref cpu_reg_ea.v, ref cpu_byte_temp);

            cpu_int_temp = cpu_byte_temp ^ 0xFF;
            cpu_int_temp1 = (cpu_reg_a + cpu_int_temp + (cpu_flag_c ? 1 : 0));

            cpu_flag_n = (cpu_int_temp1 & 0x80) != 0;
            cpu_flag_v = ((cpu_int_temp1 ^ cpu_reg_a) & (cpu_int_temp1 ^ cpu_int_temp) & 0x80) != 0;
            cpu_flag_z = (cpu_int_temp1 & 0xFF) == 0;
            cpu_flag_c = (cpu_int_temp1 >> 0x8) != 0;
            cpu_reg_a = (byte)(cpu_int_temp1 & 0xFF);
        }
        private static void JMP__()
        {
            cpu_reg_pc.v = cpu_reg_ea.v;
        }
        private static void JMP_I()
        {
            // Fetch pointer
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.h);

            Read(ref cpu_reg_ea.v, out cpu_reg_pc.l);
            cpu_reg_ea.l++; // only increment the low byte, causing the "JMP ($nnnn)" bug
            Read(ref cpu_reg_ea.v, out cpu_reg_pc.h);
        }
        private static void JSR__()
        {
            Read(ref cpu_reg_pc.v, out cpu_reg_ea.l);
            cpu_reg_pc.v++;

            // Store EAL at SP, see http://users.telenet.be/kim1-6502/6502/proman.html (see the JSR part)
            Write(ref cpu_reg_sp.v, ref cpu_reg_ea.l);

            Push(ref cpu_reg_pc.h);
            Push(ref cpu_reg_pc.l);

            Read(ref cpu_reg_pc.v, out cpu_reg_ea.h);
            cpu_reg_pc.v = cpu_reg_ea.v;

        }
        private static void LAR__()
        {
            cpu_reg_sp.l &= cpu_m;
            cpu_reg_a = cpu_reg_sp.l;
            cpu_reg_x = cpu_reg_sp.l;

            cpu_flag_n = (cpu_reg_sp.l & 0x80) != 0;
            cpu_flag_z = (cpu_reg_sp.l & 0xFF) == 0;
        }
        private static void LAX__()
        {
            cpu_reg_x = cpu_reg_a = cpu_m;

            cpu_flag_n = (cpu_reg_x & 0x80) != 0;
            cpu_flag_z = (cpu_reg_x & 0xFF) == 0;
        }
        private static void LDA__()
        {
            cpu_reg_a = cpu_m;
            cpu_flag_n = (cpu_reg_a & 0x80) == 0x80;
            cpu_flag_z = (cpu_reg_a == 0);
        }
        private static void LDX__()
        {
            cpu_reg_x = cpu_m;
            cpu_flag_n = (cpu_reg_x & 0x80) == 0x80;
            cpu_flag_z = (cpu_reg_x == 0);
        }
        private static void LDY__()
        {
            cpu_reg_y = cpu_m;
            cpu_flag_n = (cpu_reg_y & 0x80) == 0x80;
            cpu_flag_z = (cpu_reg_y == 0);
        }
        private static void LSR_A()
        {
            cpu_flag_c = (cpu_reg_a & 1) == 1;
            cpu_reg_a >>= 1;
            cpu_flag_z = (cpu_reg_a == 0);
            cpu_flag_n = (cpu_reg_a & 0x80) != 0;
        }
        private static void LSR_M()
        {
            cpu_flag_c = (cpu_m & 1) == 1;
            Write(ref cpu_reg_ea.v, ref cpu_m);
            cpu_m >>= 1;

            Write(ref cpu_reg_ea.v, ref cpu_m);
            cpu_flag_z = (cpu_m == 0);
            cpu_flag_n = (cpu_m & 0x80) != 0;
        }
        private static void NOP__()
        {
            // Do nothing.
        }
        private static void ORA__()
        {
            cpu_reg_a |= cpu_m;
            cpu_flag_n = (cpu_reg_a & 0x80) == 0x80;
            cpu_flag_z = (cpu_reg_a == 0);
        }
        private static void PHA__()
        {
            Push(ref cpu_reg_a);
        }
        private static void PHP__()
        {
            cpu_dummy = register_pb();
            Push(ref cpu_dummy);
        }
        private static void PLA__()
        {
            Read(ref cpu_reg_sp.v, out cpu_dummy);
            Pull(out cpu_reg_a);
            cpu_flag_n = (cpu_reg_a & 0x80) == 0x80;
            cpu_flag_z = (cpu_reg_a == 0);
        }
        private static void PLP__()
        {
            Read(ref cpu_reg_sp.v, out cpu_dummy);
            Pull(out cpu_dummy);
            register_p = cpu_dummy;
        }
        private static void RLA__()
        {
            Read(ref cpu_reg_ea.v, out cpu_byte_temp);

            Write(ref cpu_reg_ea.v, ref cpu_byte_temp);

            cpu_dummy = (byte)((cpu_byte_temp << 1) | (cpu_flag_c ? 0x01 : 0x00));

            Write(ref cpu_reg_ea.v, ref cpu_dummy);

            cpu_flag_n = (cpu_dummy & 0x80) != 0;
            cpu_flag_z = (cpu_dummy & 0xFF) == 0;
            cpu_flag_c = (cpu_byte_temp & 0x80) != 0;

            cpu_reg_a &= cpu_dummy;
            cpu_flag_n = (cpu_reg_a & 0x80) != 0;
            cpu_flag_z = (cpu_reg_a & 0xFF) == 0;
        }
        private static void ROL_A()
        {
            cpu_byte_temp = (byte)((cpu_reg_a << 1) | (cpu_flag_c ? 0x01 : 0x00));

            cpu_flag_n = (cpu_byte_temp & 0x80) != 0;
            cpu_flag_z = (cpu_byte_temp & 0xFF) == 0;
            cpu_flag_c = (cpu_reg_a & 0x80) != 0;

            cpu_reg_a = cpu_byte_temp;
        }
        private static void ROL_M()
        {
            Write(ref cpu_reg_ea.v, ref cpu_m);
            cpu_byte_temp = (byte)((cpu_m << 1) | (cpu_flag_c ? 0x01 : 0x00));

            Write(ref cpu_reg_ea.v, ref cpu_byte_temp);
            cpu_flag_n = (cpu_byte_temp & 0x80) != 0;
            cpu_flag_z = (cpu_byte_temp & 0xFF) == 0;
            cpu_flag_c = (cpu_m & 0x80) != 0;
        }
        private static void ROR_A()
        {
            cpu_byte_temp = (byte)((cpu_reg_a >> 1) | (cpu_flag_c ? 0x80 : 0x00));

            cpu_flag_n = (cpu_byte_temp & 0x80) != 0;
            cpu_flag_z = (cpu_byte_temp & 0xFF) == 0;
            cpu_flag_c = (cpu_reg_a & 0x01) != 0;

            cpu_reg_a = cpu_byte_temp;
        }
        private static void ROR_M()
        {
            Write(ref cpu_reg_ea.v, ref cpu_m);

            cpu_byte_temp = (byte)((cpu_m >> 1) | (cpu_flag_c ? 0x80 : 0x00));
            Write(ref cpu_reg_ea.v, ref cpu_byte_temp);

            cpu_flag_n = (cpu_byte_temp & 0x80) != 0;
            cpu_flag_z = (cpu_byte_temp & 0xFF) == 0;
            cpu_flag_c = (cpu_m & 0x01) != 0;
        }
        private static void RRA__()
        {
            Read(ref cpu_reg_ea.v, out cpu_byte_temp);

            Write(ref cpu_reg_ea.v, ref cpu_byte_temp);

            cpu_dummy = (byte)((cpu_byte_temp >> 1) | (cpu_flag_c ? 0x80 : 0x00));

            Write(ref cpu_reg_ea.v, ref cpu_dummy);

            cpu_flag_n = (cpu_dummy & 0x80) != 0;
            cpu_flag_z = (cpu_dummy & 0xFF) == 0;
            cpu_flag_c = (cpu_byte_temp & 0x01) != 0;

            cpu_byte_temp = cpu_dummy;
            cpu_int_temp = (cpu_reg_a + cpu_byte_temp + (cpu_flag_c ? 1 : 0));

            cpu_flag_n = (cpu_int_temp & 0x80) != 0;
            cpu_flag_v = ((cpu_int_temp ^ cpu_reg_a) & (cpu_int_temp ^ cpu_byte_temp) & 0x80) != 0;
            cpu_flag_z = (cpu_int_temp & 0xFF) == 0;
            cpu_flag_c = (cpu_int_temp >> 0x8) != 0;
            cpu_reg_a = (byte)(cpu_int_temp);
        }
        private static void RTI__()
        {
            Read(ref cpu_reg_sp.v, out cpu_dummy);

            Pull(out cpu_dummy);
            register_p = cpu_dummy;

            Pull(out cpu_reg_pc.l);
            Pull(out cpu_reg_pc.h);
        }
        private static void RTS__()
        {
            Read(ref cpu_reg_sp.v, out cpu_dummy);
            Pull(out cpu_reg_pc.l);
            Pull(out cpu_reg_pc.h);

            cpu_reg_pc.v++;

            Read(ref cpu_reg_pc.v, out cpu_dummy);
        }
        private static void SAX__()
        {
            cpu_dummy = (byte)(cpu_reg_x & cpu_reg_a);
            Write(ref cpu_reg_ea.v, ref cpu_dummy);
        }
        private static void SBC__()
        {
            cpu_m ^= 0xFF;
            cpu_int_temp = (cpu_reg_a + cpu_m + (cpu_flag_c ? 1 : 0));

            cpu_flag_n = (cpu_int_temp & 0x80) != 0;
            cpu_flag_v = ((cpu_int_temp ^ cpu_reg_a) & (cpu_int_temp ^ cpu_m) & 0x80) != 0;
            cpu_flag_z = (cpu_int_temp & 0xFF) == 0;
            cpu_flag_c = (cpu_int_temp >> 0x8) != 0;
            cpu_reg_a = (byte)(cpu_int_temp);
        }
        private static void SEC__()
        {
            cpu_flag_c = true;
        }
        private static void SEI__()
        {
            cpu_flag_i = true;
        }
        private static void SHX__()
        {
            cpu_byte_temp = (byte)(cpu_reg_x & (cpu_reg_ea.h + 1));

            Read(ref cpu_reg_ea.v, out cpu_dummy);
            cpu_reg_ea.l += cpu_reg_y;

            if (cpu_reg_ea.l < cpu_reg_y)
                cpu_reg_ea.h = cpu_byte_temp;

            Write(ref cpu_reg_ea.v, ref cpu_byte_temp);
        }
        private static void SHY__()
        {
            cpu_byte_temp = (byte)(cpu_reg_y & (cpu_reg_ea.h + 1));

            Read(ref cpu_reg_ea.v, out cpu_dummy);
            cpu_reg_ea.l += cpu_reg_x;

            if (cpu_reg_ea.l < cpu_reg_x)
                cpu_reg_ea.h = cpu_byte_temp;
            Write(ref cpu_reg_ea.v, ref cpu_byte_temp);
        }
        private static void SLO__()
        {
            Read(ref cpu_reg_ea.v, out cpu_byte_temp);

            cpu_flag_c = (cpu_byte_temp & 0x80) != 0;

            Write(ref cpu_reg_ea.v, ref cpu_byte_temp);

            cpu_byte_temp <<= 1;

            Write(ref cpu_reg_ea.v, ref cpu_byte_temp);

            cpu_flag_n = (cpu_byte_temp & 0x80) != 0;
            cpu_flag_z = (cpu_byte_temp & 0xFF) == 0;

            cpu_reg_a |= cpu_byte_temp;
            cpu_flag_n = (cpu_reg_a & 0x80) != 0;
            cpu_flag_z = (cpu_reg_a & 0xFF) == 0;
        }
        private static void SRE__()
        {
            Read(ref cpu_reg_ea.v, out cpu_byte_temp);

            cpu_flag_c = (cpu_byte_temp & 0x01) != 0;

            Write(ref cpu_reg_ea.v, ref cpu_byte_temp);

            cpu_byte_temp >>= 1;

            Write(ref cpu_reg_ea.v, ref cpu_byte_temp);

            cpu_flag_n = (cpu_byte_temp & 0x80) != 0;
            cpu_flag_z = (cpu_byte_temp & 0xFF) == 0;

            cpu_reg_a ^= cpu_byte_temp;
            cpu_flag_n = (cpu_reg_a & 0x80) != 0;
            cpu_flag_z = (cpu_reg_a & 0xFF) == 0;
        }
        private static void STA__()
        {
            Write(ref cpu_reg_ea.v, ref cpu_reg_a);
        }
        private static void STX__()
        {
            Write(ref cpu_reg_ea.v, ref cpu_reg_x);
        }
        private static void STY__()
        {
            Write(ref cpu_reg_ea.v, ref cpu_reg_y);
        }
        private static void TAX__()
        {
            cpu_reg_x = cpu_reg_a;
            cpu_flag_n = (cpu_reg_x & 0x80) == 0x80;
            cpu_flag_z = (cpu_reg_x == 0);
        }
        private static void TAY__()
        {
            cpu_reg_y = cpu_reg_a;
            cpu_flag_n = (cpu_reg_y & 0x80) == 0x80;
            cpu_flag_z = (cpu_reg_y == 0);
        }
        private static void TSX__()
        {
            cpu_reg_x = cpu_reg_sp.l;
            cpu_flag_n = (cpu_reg_x & 0x80) != 0;
            cpu_flag_z = cpu_reg_x == 0;
        }
        private static void TXA__()
        {
            cpu_reg_a = cpu_reg_x;
            cpu_flag_n = (cpu_reg_a & 0x80) == 0x80;
            cpu_flag_z = (cpu_reg_a == 0);
        }
        private static void TXS__()
        {
            cpu_reg_sp.l = cpu_reg_x;
        }
        private static void TYA__()
        {
            cpu_reg_a = cpu_reg_y;
            cpu_flag_n = (cpu_reg_a & 0x80) == 0x80;
            cpu_flag_z = (cpu_reg_a == 0);
        }
        private static void XAA__()
        {
            cpu_reg_a = (byte)(cpu_reg_x & cpu_m);
            cpu_flag_n = (cpu_reg_a & 0x80) != 0;
            cpu_flag_z = (cpu_reg_a & 0xFF) == 0;
        }
        private static void XAS__()
        {
            cpu_reg_sp.l = (byte)(cpu_reg_a & cpu_reg_x /*& ((dummyVal >> 8) + 1)*/);
            Write(ref cpu_reg_ea.v, ref cpu_reg_sp.l);
        }
        #endregion

        private static void CPUWriteState(ref System.IO.BinaryWriter bin)
        {
            bin.Write(cpu_reg_pc.v);
            bin.Write(cpu_reg_sp.v);
            bin.Write(cpu_reg_ea.v);
            bin.Write(cpu_reg_a);
            bin.Write(cpu_reg_x);
            bin.Write(cpu_reg_y);
            bin.Write(cpu_flag_n);
            bin.Write(cpu_flag_v);
            bin.Write(cpu_flag_d);
            bin.Write(cpu_flag_i);
            bin.Write(cpu_flag_z);
            bin.Write(cpu_flag_c);
            bin.Write(cpu_m);
            bin.Write(cpu_opcode);
            bin.Write(cpu_byte_temp);
            bin.Write(cpu_int_temp);
            bin.Write(cpu_int_temp1);
            bin.Write(cpu_dummy);
            bin.Write(cpu_bool_tmp);
            bin.Write(temp_add.v);
            bin.Write(CPU_IRQ_PIN);
            bin.Write(CPU_NMI_PIN);
            bin.Write(cpu_suspend_nmi);
            bin.Write(cpu_suspend_irq);
        }
        private static void CPUReadState(ref System.IO.BinaryReader bin)
        {
            cpu_reg_pc.v = bin.ReadUInt16();
            cpu_reg_sp.v = bin.ReadUInt16();
            cpu_reg_ea.v = bin.ReadUInt16();
            cpu_reg_a = bin.ReadByte();
            cpu_reg_x = bin.ReadByte();
            cpu_reg_y = bin.ReadByte();
            cpu_flag_n = bin.ReadBoolean();
            cpu_flag_v = bin.ReadBoolean();
            cpu_flag_d = bin.ReadBoolean();
            cpu_flag_i = bin.ReadBoolean();
            cpu_flag_z = bin.ReadBoolean();
            cpu_flag_c = bin.ReadBoolean();
            cpu_m = bin.ReadByte();
            cpu_opcode = bin.ReadByte();
            cpu_byte_temp = bin.ReadByte();
            cpu_int_temp = bin.ReadInt32();
            cpu_int_temp1 = bin.ReadInt32();
            cpu_dummy = bin.ReadByte();
            cpu_bool_tmp = bin.ReadBoolean();
            temp_add.v = bin.ReadUInt16();
            CPU_IRQ_PIN = bin.ReadBoolean();
            CPU_NMI_PIN = bin.ReadBoolean();
            cpu_suspend_nmi = bin.ReadBoolean();
            cpu_suspend_irq = bin.ReadBoolean();
        }
    }
}
