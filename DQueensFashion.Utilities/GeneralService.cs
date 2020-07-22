﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DQueensFashion.Utilities
{
    public class GeneralService
    {
        public string GenerateItemNameAsParam(int Id, string Name)
        {
            string phrase = string.Format("{0}-{1}", Name, Id);// Creates in the specific pattern  
            string str = GetByteArray(phrase).ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");// Remove invalid characters for param  
            str = Regex.Replace(str, @"\s+", "-").Trim(); // convert multiple spaces into one hyphens   
           // str = str.Substring(0, str.Length <= 30 ? str.Length : 30).Trim(); //Trim to max 30 char  
            str = Regex.Replace(str, @"\s", "-"); // Replaces spaces with hyphens     
            return str;
        }

        public string GetDateInString(DateTime date, bool showTime=false, bool breakLine=false)
        {
            if (showTime)
            {
                if(breakLine)
                    return date.ToString("MMM dd, yyyy\r\nhh:mm tt");
                else
                    return date.ToString("MMM dd, yyyy - hh:mm tt");
            }
                
            else
                return date.ToString("MMM dd, yyyy");
        }

        private string GetByteArray(string text)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }


    }
}
