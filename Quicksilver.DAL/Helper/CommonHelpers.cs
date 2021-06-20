using System;
using System.Collections.Generic;
using System.Text;

namespace Quicksilver.DAL.Helper
{
    public class CommonHelpers
    {
        public string CreatePassword(int length=8)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length --)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
