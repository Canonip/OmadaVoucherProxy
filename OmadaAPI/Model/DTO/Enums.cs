using System;
using System.Collections.Generic;
using System.Text;

namespace OmadaAPI.Model.DTO
{
    public enum SortOrder
    {
        asc, desc
    }
    public enum SortName
    {
        createdTime, codeAlias, note, duration
    }
    public enum LaunchStatus
    {
        /// <summary>
        /// If CloudConnector.ConnectToCloudControllerAsync hasn't been queried yet
        /// </summary>
        NotStarted = 0,
        /// <summary>
        /// After CloudConnector.ConnectToCloudControllerAsync has been queried, but the device is not ready yet
        /// </summary>
        Starting = 1,
        /// <summary>
        /// Device is Ready
        /// </summary>
        Ready = 2,
        /// <summary>
        /// Unknown Code
        /// </summary>
        Unknown = -1
    }
}
