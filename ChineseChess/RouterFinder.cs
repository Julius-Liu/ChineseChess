using System;
using System.Collections.Generic;
using System.Text;

using UPNPLib;

namespace ChineseChess
{
    /// <summary>
    /// 查找路由器
    /// </summary>
    class RouterFinder
    {
        /// <summary>
        /// 20140714
        /// 搜索路由器
        /// 先不使用该函数，等到确定了在没有路由器的情况下，
        /// private IUPnPNAT nat = (UPnPNAT)new UPnPNATClass();
        /// nat的状态是怎么样的
        /// 再决定是否要搜索路由器
        /// </summary>
        public void SearchRouter()
        {
            UPnPDeviceFinder finder = new UPNPLib.UPnPDeviceFinderClass();
            string deviceType = "upnp:rootdevice";
            UPNPLib.IUPnPDevices allDevice = new UPNPLib.UPnPDevicesClass();
            allDevice = finder.FindByType(deviceType, 0);

            UPNPLib.IUPnPDevice device = new UPNPLib.UPnPDeviceClass();

            foreach (UPNPLib.UPnPDevice p in allDevice)
            {
                if (p.Type.Equals("urn:schemas-upnp-org:device:InternetGatewayDevice:1"))
                {
                    device = p;
                    break;
                }
            }
            if (device != null)
            {
                //labelRouterFind.Text = "已发现路由器！";
                //labelRouterFind.ForeColor = Color.Blue;
            }
        }
    }
}
