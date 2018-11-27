﻿/*
Printer++ Virtual Printer Processor
Copyright (C) 2012 - Printer++

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/
using Microsoft.Win32;

namespace PrinterPlusPlusSetupCustomAction
{
    public static class AutorunHelper
    {
        public static void AddToStartup()
        {
            var value = "C:\\PrinterPlusPlus\\PrinterPlusPlus.exe -silent";
            LogHelper.Log("Adding Printer++ to Startup");
            var regKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            regKey.SetValue("Printer++", (string)value, RegistryValueKind.String);
        }
    }
}
