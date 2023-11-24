using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace インスタ寺川宿
{
    class Moniter
    {
        const int SC_MONITORPOWER = 0xf170;
        const int WM_SYSCOMMAND = 0x112;
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(int hWnd, uint Msg, int wParam, int lParam);
        public static void PowerSave()
        {
            //省電力
            SendMessage(-1, WM_SYSCOMMAND, SC_MONITORPOWER, 1);
        }
        public static void PowerOff()
        {
            //モニター停止
            SendMessage(-1, WM_SYSCOMMAND, SC_MONITORPOWER, 2);
        }
        public static void PowerOn()
        {
            //モニター復帰
            SendMessage(-1, WM_SYSCOMMAND, SC_MONITORPOWER, -1);
        }
    }
}
