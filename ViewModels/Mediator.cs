using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR_Cartographers.ViewModels
{
    public static class Mediator
    {
        public static string FirstPlayerName { get; set; } = "";
        public static string SecondPlayerName { get; set; } = "";
        public static string Login { get; set; } = "";
        public static bool IsMuted { get; set; } = true;
        public static string Background {  get; set; }
        public static int UserId { get; set; }
        public static int PageId { get; set; }
    }
}
